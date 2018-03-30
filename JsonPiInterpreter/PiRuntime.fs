module JsonPi.PiRuntime

open System
open System.Collections.Generic

open JsonPi.Data

type PiNamespace() =
    let pendingInputs = Dictionary<PiIdentifier, GuardedContext list>()
    let pendingOutputs = Dictionary<PiIdentifier, GuardedContext list>()

    let TryGetList (dict:Dictionary<PiIdentifier, GuardedContext list>) id =
         match dict.ContainsKey(id) with
                | false -> None
                | true ->
                    Some(dict.[id])

    let WhereNotContext (context:PiContext) =
        List.where (fun {CurrentContext=c;} ->
                        c <> context
                   )

    member this.TrySend(channel:PiJsonObject, guard:GuardedContext) =
        match channel with
        | PiName (id, nameTypeOpt, _) ->
            match TryGetList pendingInputs id with
            | Some(l) ->
                match l with
                | h :: [] -> 
                    pendingInputs.Remove(id) |> ignore
                    Some(h)
                | h :: t -> 
                    pendingInputs.[id] <- t
                    Some(h)
                | [] -> failwith "unexpected"
            | None ->
                match TryGetList pendingOutputs id with
                | Some(l) ->
                    pendingOutputs.[id] <- guard :: l
                | None ->
                    pendingOutputs.Add(id, [guard])
                None
        | _ -> failwith "bad"

    member this.TryReceive(channel:PiJsonObject, guard:GuardedContext) =
        match channel with
        | PiName (id, nameTypeOpt, _) ->
            match TryGetList pendingOutputs id with
            | Some(l) ->
                match l with
                | h :: [] -> 
                    pendingOutputs.Remove(id) |> ignore
                    Some(h)
                | h :: t -> 
                    pendingOutputs.[id] <- t
                    Some(h)
                | [] -> failwith "unexpected"
            | None ->
                match TryGetList pendingInputs id with
                | Some(l) ->
                    pendingInputs.[id] <- guard :: l
                | None ->
                    pendingInputs.Add(id, [guard])
                None
        | _ -> failwith "bad"

    member this.RemoveSummation (channel:PiJsonObject, context:PiContext) =
        let id = GetMemberValue<string> channel "Id"

        match TryGetList pendingOutputs id with
        | Some(l) ->
            match WhereNotContext context l with
            | [] -> pendingOutputs.Remove(id) |> ignore
            | outl -> pendingOutputs.[id] <- outl
        | None -> 
            ()

        match TryGetList pendingInputs id with
        | Some(l) ->
            match WhereNotContext context l with
            | [] -> pendingInputs.Remove(id) |> ignore
            | outl -> pendingInputs.[id] <- outl
        | None -> 
            ()

let rec internal subpfx (pfx:PiJsonObject) (rn:PiJsonObject) (fn:PiJsonObject) =
    match pfx with
    | PrefixInput (c, x)
    | PrefixOutput(c, x) -> 
        match (c, fn) with
        | (PiName (id, _, _), PiName(fnid, _, _)) -> 
            if id = fnid then
                SetMemberValue pfx "Channel" rn

            let newParams = 
                x |> Array.map
                    (function
                     | PiName (nid, _, _) when nid = fnid -> rn :> obj
                     | _ as xn -> xn
                    )
            SetMemberValue pfx "Params" newParams

        | _ -> failwith "bad"

    | PrefixMatch (n1, n2, p1) -> 
        match (n1, n2, fn) with
        | (PiName (nid1, _, _), PiName(nid2, _, _), PiName(fnid, _, _)) ->
            if nid1 = fnid then
                SetMemberValue pfx "NameLeft" rn

            if nid2 = fnid then
                SetMemberValue pfx "NameRight" rn

            subpfx p1 rn fn
        | _ -> failwith "bad"
        
    | PrefixUnobservable -> ()
    | _ -> failwith "bad"

let rec subs s rn fn = 
    match s with
    | SummationInaction -> ()
    | SummationPrefix (pfx, p1) -> 
        subpfx pfx rn fn
        subp p1 rn fn
    | SummationSum(ls) -> 
        ls |> Array.iter (fun s1 -> subs s1 rn fn)
    | _ -> failwith "bad"

and subp p rn fn =
    match p with
    | ProcessSummation s -> 
        subs s rn fn
    | ProcessComposition (p1, p2) ->
        subp p1 rn fn
        subp p2 rn fn
    | ProcessReplication p1 ->
        subp p1 rn fn
    | ProcessRestriction (n, rp) ->
        match (n, fn) with
        | (PiName (id, _, _), PiName(fnid, _, _)) when id = fnid -> 
            SetMemberValue n "Id" (GetMemberValue rn "Id")
        | _ -> ()
        subp rp rn fn
    | ProcessBinding (id, p1, p2) ->
        subp p1 rn fn
        subp p2 rn fn
    | ProcessBindingRef id -> ()
    | ProcessModuleRef (id, inIdOpt, p1) -> 
        subp p1 rn fn
    | _ -> failwith "bad"

let rec internal Substitute (p:PiJsonObject) (r:PiJsonArray) (f:PiJsonArray) =
    for i = 0 to r.Length - 1 do
        if i < f.Length then
            subp p (r.[i] :?> PiJsonObject) (f.[i] :?> PiJsonObject)
