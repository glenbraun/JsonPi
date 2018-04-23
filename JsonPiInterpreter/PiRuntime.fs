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

let rec internal exts (s:PiJsonObject) (continuation:PiJsonObject) = 
    match s with
    | SummationInaction -> s
    | SummationPrefix (pfx, p1) -> 
        CreateSummationPrefix pfx (extp p1 continuation)
    | SummationSum(ls) -> 
        let sumlist = ls |> Seq.map (fun s1 -> exts (s1 :?> PiJsonObject) continuation) |> List.ofSeq
        CreateSummationSum sumlist
    | _ -> failwith "bad"

and internal extp (p:PiJsonObject) (continuation:PiJsonObject) =
    match p with
    | ProcessSummation s -> 
        CreateProcessSummation (exts s continuation)
    | ProcessComposition (p1, p2) ->
        CreateProcessComposition (extp p1 continuation) (extp p2 continuation)
    | ProcessReplication p1 ->
        CreateProcessReplication (extp p1 continuation)
    | ProcessRestriction (n, rp) ->
        CreateProcessRestriction n (extp rp continuation)
    | ProcessBinding (id, p1, p2) ->
        CreateProcessBinding id (extp p1 continuation) (extp p2 continuation)
    | ProcessBindingRef id -> 
        if id = "continue" then
            continuation
        else 
            p
    | ProcessModuleRef (id, inIdOpt, p1) -> 
        CreateProcessModuleRef id inIdOpt (extp p1 continuation)
    | _ -> failwith "bad"

and internal extcontinue (p:PiJsonObject) (continuation:PiJsonObject) =
    extp p continuation
    

let internal subparams (pa:PiJsonArray) (rn:obj) (fn:obj) = 
    let fnid =
        match fn with
        | PiName(id, _, _) -> id
        | _ -> failwith "bad"

    seq {
        for i = 0 to pa.Length - 1 do
            match pa.[i] with
            | PiName (nid, _, _) when nid = fnid -> 
                match rn with
                | :? PiJsonArray as rna ->
                    yield! rna
                | PiName(_,_,_) ->
                    yield rn
                | _ -> failwith "bad"
            | _ as xn -> 
                yield xn
    } |> Array.ofSeq


let rec internal subpfx (pfx:PiJsonObject) (rn:obj) (fn:obj) =
    match pfx with
    | PrefixInput (c, x)
    | PrefixOutput(c, x) -> 
        match (c, fn) with
        | (PiName (id, _, _), PiName(fnid, _, _)) -> 
            if id = fnid then
                SetMemberValue pfx "Channel" rn

            let newParams = subparams x rn fn            
            SetMemberValue pfx "Params" newParams

        | _ -> failwith "bad"

    | PrefixMatch (paramsLeft, paramsRight, p1) -> 
        let newParamsLeft = subparams paramsLeft rn fn
        let newParamsRight = subparams paramsRight rn fn

        SetMemberValue pfx "ParamsLeft" (newParamsLeft :> obj)
        SetMemberValue pfx "ParamsRight" (newParamsRight :> obj)

        subpfx p1 rn fn
        
    | PrefixUnobservable -> ()
    | _ -> failwith "bad"

let rec subs s rn fn (restriction:bool)  = 
    match s with
    | SummationInaction -> ()
    | SummationPrefix (pfx, p1) -> 
        subpfx pfx rn fn
        subp p1 rn fn restriction
    | SummationSum(ls) -> 
        ls |> Array.iter (fun s1 -> subs s1 rn fn restriction)
    | _ -> failwith "bad"

and subp p (rn:obj) (fn:obj) (restriction:bool) =
    match p with
    | ProcessSummation s -> 
        subs s rn fn restriction
    | ProcessComposition (p1, p2) ->
        subp p1 rn fn restriction
        subp p2 rn fn restriction
    | ProcessReplication p1 ->
        subp p1 rn fn restriction
    | ProcessRestriction (n, rp) ->
        match (n, fn) with
        | (PiName (id, _, _), PiName(fnid, _, _)) ->
            if restriction && id = fnid then
                SetMemberValue n "Id" (GetMemberValue (rn :?> PiJsonObject) "Id")
                subp rp rn fn restriction
            elif not(restriction) && id <> fnid then
                subp rp rn fn restriction
        | _ -> ()
    | ProcessBinding (id, p1, p2) ->
        subp p1 rn fn restriction
        subp p2 rn fn restriction
    | ProcessBindingRef id -> ()
    | ProcessModuleRef (id, inIdOpt, p1) -> 
        subp p1 rn fn restriction
    | _ -> failwith "bad"

let rec internal Substitute (p:PiJsonObject) (r:PiJsonArray) (f:PiJsonArray) (restriction:bool) =
    for i = 0 to f.Length - 1 do
        if r.Length > f.Length && i = f.Length - 1 then
            let ra = Array.sub r i (r.Length - i)
            subp p ra (f.[i]) restriction
        else
            if i < r.Length then
                subp p (r.[i]) (f.[i]) restriction

