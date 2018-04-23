module JsonPi.Data

open System
open System.IO

type PiJsonPair = {
        Label : string;
        mutable Value : obj
    }

type PiJsonObject = PiJsonPair list

type PiJsonArray = obj array

type PiIdentifier = string

type IPiExtension =
    abstract member OnOutput : PiJsonObject -> PiJsonArray -> PiJsonObject option
    abstract member OnInput : PiJsonObject -> PiJsonArray -> PiJsonArray -> PiJsonObject option

type PiExtensionResolver = (PiIdentifier -> obj option -> IPiExtension option)

let GetMemberValue<'T> (json:PiJsonObject) (label:string) =
    let {Value = value} = json |> List.find ( fun {Label=l} -> l = label)
    value :?> 'T

let TryGetMemberValue<'T> (json:PiJsonObject) (label:string) = 
    match json |> List.tryFind ( fun {Label=l} -> l = label) with
    | Some({Value = value}) -> Some(value :?> 'T)
    | None -> None

let SetMemberValue (json:PiJsonObject) (label:string) (value:obj) =
    match json |> List.tryFind ( fun {Label=l} -> l = label) with
    | Some(m) -> 
        m.Value <- value        
    | None -> 
        failwith "Member not found"

let CreateName (id:PiIdentifier) (nametype:PiIdentifier option) (data:obj option) = 
    match (nametype, data) with
    | (None, None) ->  
        [
            { Label="Type"; Value="PiName" :> obj};
            { Label="Id"; Value=id :> obj};
        ]
    | (Some(t), None) ->
        [
            { Label=String.Intern("Type"); Value="PiName" :> obj};
            { Label=String.Intern("Id"); Value=id :> obj};
            { Label=String.Intern("NameType"); Value=t :> obj};
        ]
    | (None, Some(d)) ->
        [
            { Label=String.Intern("Type"); Value="PiName" :> obj};
            { Label=String.Intern("Id"); Value=id :> obj};
            { Label=String.Intern("Data"); Value=d};
        ]
    | (Some(t), Some(d)) ->
        [
            { Label=String.Intern("Type"); Value="PiName" :> obj};
            { Label=String.Intern("Id"); Value=id :> obj};
            { Label=String.Intern("NameType"); Value=t :> obj};
            { Label=String.Intern("Data"); Value=d};
        ]

let (|PiName|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "PiName" -> Some( (GetMemberValue<PiIdentifier> json "Id", TryGetMemberValue<PiIdentifier> json "NameType", TryGetMemberValue<obj> json "Data") )
        | _ -> None
    | _ -> None

let CreatePrefixUnobservable () =
    [
        { Label="Type"; Value="Prefix.Unobservable" :> obj};
    ];

let (|PrefixUnobservable|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Prefix.Unobservable" -> Some()
        | _ -> None
    | _ -> None

let CreatePrefixOutput (channel:PiJsonObject) (paramList: PiJsonObject list) =
    let paramArray = paramList |> List.map (fun p -> p :> obj) |> List.toArray
    [
        { Label="Type"; Value="Prefix.Output" :> obj};
        { Label="Channel"; Value=channel :> obj};
        { Label="Params"; Value=paramArray :> obj};
    ];

let (|PrefixOutput|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Prefix.Output" -> Some( (GetMemberValue<PiJsonObject> json "Channel", GetMemberValue<PiJsonArray> json "Params") )
        | _ -> None
    | _ -> None

let CreatePrefixInput (channel:PiJsonObject) (typedparamList:PiJsonObject list) = 
    let typedparamArray = typedparamList |> List.map (fun p -> p :> obj) |> List.toArray
    [
        { Label="Type"; Value="Prefix.Input" :> obj};
        { Label="Channel"; Value=channel :> obj};
        { Label="Params"; Value=typedparamArray :> obj};
    ];

let (|PrefixInput|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Prefix.Input" -> Some( (GetMemberValue<PiJsonObject> json "Channel", GetMemberValue<PiJsonArray> json "Params") )
        | _ -> None
    | _ -> None

let CreatePrefixMatch (leftParams:PiJsonObject list) (rightParams:PiJsonObject list) (prefix:PiJsonObject) =
    let leftParamArray = leftParams |> List.map (fun p -> p :> obj) |> List.toArray
    let rightParamArray = rightParams |> List.map (fun p -> p :> obj) |> List.toArray
    [
        { Label="Type"; Value="Prefix.Match" :> obj};
        { Label="ParamsLeft"; Value=leftParamArray :> obj};
        { Label="ParamsRight"; Value=rightParamArray :> obj};
        { Label="Prefix"; Value=prefix :> obj};
    ];

let (|PrefixMatch|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Prefix.Match" -> Some( (GetMemberValue<PiJsonArray> json "ParamsLeft", GetMemberValue<PiJsonArray> json "ParamsRight", GetMemberValue<PiJsonObject> json "Prefix") )
        | _ -> None
    | _ -> None

let rec GetPrefixChannel (prefix:PiJsonObject) =
    match prefix with
    | PrefixInput (channel, _)
    | PrefixOutput (channel, _) -> channel
    | PrefixMatch (_, _, matchPfx) -> GetPrefixChannel matchPfx
    | _ -> failwith "bad"

let CreateSummationInaction() = 
    [
        { Label="Type"; Value="Summation.Inaction" :> obj};
    ];

let (|SummationInaction|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Summation.Inaction" -> Some()
        | _ -> None
    | _ -> None

let CreateSummationPrefix (prefix:PiJsonObject) (proc:PiJsonObject) = 
    [
        { Label="Type"; Value="Summation.Prefix" :> obj};
        { Label="Prefix"; Value=prefix :> obj};
        { Label="Process"; Value=proc :> obj};
    ]; 

let (|SummationPrefix|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Summation.Prefix" -> Some( (GetMemberValue<PiJsonObject> json "Prefix", GetMemberValue<PiJsonObject> json "Process") )
        | _ -> None
    | _ -> None

let CreateSummationSum (sums:PiJsonObject list) =
    let sumsArray = sums |> List.map (fun s -> s :> obj) |> List.toArray
    [
        { Label="Type"; Value="Summation.Sum" :> obj};
        { Label="Summations"; Value=sumsArray :> obj};
    ]; 

let (|SummationSum|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Summation.Sum" -> Some(GetMemberValue<PiJsonArray> json "Summations" )
        | _ -> None
    | _ -> None

let CreateProcessBindingRef (id:PiIdentifier) =
    [
        { Label="Type"; Value="Process.BindingRef" :> obj};
        { Label="Id"; Value=id :> obj};
    ];

let (|ProcessBindingRef|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Process.BindingRef" -> Some(GetMemberValue<PiIdentifier> json "Id" )
        | _ -> None
    | _ -> None

let CreateProcessModuleRef (id:PiIdentifier) (inAssemblyOpt:PiIdentifier option) (continuation:PiJsonObject) =
    match inAssemblyOpt with
    | Some(inAssembly) -> 
        [
            { Label="Type"; Value="Process.ModuleRef" :> obj};
            { Label="Id"; Value=id :> obj};
            { Label="InAssembly"; Value=inAssembly :> obj};
            { Label="Continuation"; Value=continuation :> obj};
        ];
    | None ->
        [
            { Label="Type"; Value="Process.ModuleRef" :> obj};
            { Label="Id"; Value=id :> obj};
            { Label="Continuation"; Value=continuation :> obj};
        ];

let (|ProcessModuleRef|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Process.ModuleRef" -> Some( (GetMemberValue<PiIdentifier> json "Id", TryGetMemberValue<PiIdentifier> json "InAssembly", GetMemberValue<PiJsonObject> json "Continuation") )
        | _ -> None
    | _ -> None

let CreateProcessBinding (id:PiIdentifier) (proc:PiJsonObject) (continuation:PiJsonObject) =
    [
        { Label="Type"; Value="Process.Binding" :> obj};
        { Label="Id"; Value=id :> obj};
        { Label="Process"; Value=proc :> obj};
        { Label="Continuation"; Value=continuation :> obj};
    ];

let (|ProcessBinding|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Process.Binding" -> Some( (GetMemberValue<PiIdentifier> json "Id", GetMemberValue<PiJsonObject> json "Process", GetMemberValue<PiJsonObject> json "Continuation") )
        | _ -> None
    | _ -> None

let CreateProcessSummation (sum:PiJsonObject) =
    [
        { Label="Type"; Value="Process.Summation" :> obj};
        { Label="Summation"; Value=sum :> obj};
    ];

let (|ProcessSummation|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Process.Summation" -> Some( GetMemberValue<PiJsonObject> json "Summation" )
        | _ -> None
    | _ -> None

let CreateProcessRestriction (name:PiJsonObject) (continuation:PiJsonObject) =
    [
        { Label="Type"; Value="Process.Restriction" :> obj};
        { Label="Name"; Value=name :> obj};
        { Label="Continuation"; Value=continuation :> obj};
    ];

let (|ProcessRestriction|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Process.Restriction" -> Some( (GetMemberValue<PiJsonObject> json "Name", GetMemberValue<PiJsonObject> json "Continuation") )
        | _ -> None
    | _ -> None

let CreateProcessComposition (left:PiJsonObject) (right:PiJsonObject) = 
    [
        { Label="Type"; Value="Process.Composition" :> obj};
        { Label="ProcessLeft"; Value=left :> obj};
        { Label="ProcessRight"; Value=right :> obj};
    ];

let (|ProcessComposition|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Process.Composition" -> Some( (GetMemberValue<PiJsonObject> json "ProcessLeft", GetMemberValue<PiJsonObject> json "ProcessRight") )
        | _ -> None
    | _ -> None

let CreateProcessReplication (proc:PiJsonObject) = 
    [
        { Label="Type"; Value="Process.Replication" :> obj};
        { Label="Process"; Value=proc :> obj};
    ];

let (|ProcessReplication|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "Process.Replication" -> Some(GetMemberValue<PiJsonObject> json "Process" )
        | _ -> None
    | _ -> None

let CreateModule (id:PiIdentifier) (proc:PiJsonObject) =
    [
        { Label="Type"; Value="PiModule" :> obj};
        { Label="Id"; Value=id :> obj};
        { Label="Process"; Value=proc :> obj};
    ];

let (|PiModule|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json -> 
        match GetMemberValue<string> json "Type" with
        | "PiModule" -> Some( (GetMemberValue<PiIdentifier> json "Id", GetMemberValue<PiJsonObject> json "Process") )
        | _ -> None
    | _ -> None

let CreateAssembly (id:PiIdentifier) (modules: PiJsonObject list) =
    let modulesArray = modules |> List.map (fun m -> m :> obj) |> List.toArray
    [
        { Label="Type"; Value="PiAssembly" :> obj};
        { Label="Id"; Value=id :> obj};
        { Label="Modules"; Value=modulesArray :> obj};
    ];

let (|PiAssembly|_|) (data:obj) =
    match data with
    | :? PiJsonObject as json ->
        match GetMemberValue<string> json "Type" with
        | "PiAssembly" -> Some( (GetMemberValue<PiIdentifier> json "Id", GetMemberValue<PiJsonArray> json "Modules") )
        | _ -> None
    | _ -> None

let (|AssemblyEntryProcess|_|) (json:PiJsonObject) =
    match json with
    | PiAssembly (idAsm, modules) ->
        modules |> 
        Array.tryPick 
            ( function
              | PiModule (idMod, proc) when idMod = "" ->
                  Some(proc)
              | _ -> None
            )
    | _ -> None

type PiContext private (start:PiJsonObject, pl:PiJsonObject list) =
    let mutable proc = start
    let mutable (parents:PiJsonObject list) = pl

    let tryFindModule (modId:PiIdentifier) (inAsmOpt:PiIdentifier option) (assemblies:PiJsonObject seq) =
        assemblies |>
            Seq.rev |>
            Seq.tryPick (fun asm -> 
                                let asmId = GetMemberValue<PiIdentifier> asm "Id"
                                let idMatch = 
                                    match inAsmOpt with
                                    | Some(inAsm) -> 
                                        inAsm = asmId
                                    | None -> 
                                        true
                                if idMatch then
                                    let modules = GetMemberValue<PiJsonArray> asm "Modules"
                                    modules |>
                                        Seq.tryPick (fun m ->
                                                        match m with
                                                        | PiModule(id, proc) when modId = id ->
                                                            Some(proc)
                                                        | _ -> None
                                                     )
                                else 
                                    None
                            )

    let rec tryFindExport (bindingId:PiIdentifier) (json:PiJsonObject) (assemblies:PiJsonObject seq) =
        match json with
        | PiModule (id, proc) ->
            tryFindExport bindingId proc assemblies
        | ProcessBinding (id, proc, continuation) ->
            if id = bindingId then
                Some(proc)
            else
                tryFindExport bindingId continuation assemblies
        | ProcessModuleRef (modId, inAsmOpt, _) ->
            match tryFindModule modId inAsmOpt assemblies with
            | Some(m) -> tryFindExport bindingId m assemblies
            | None -> failwith "unable to find module"
        | _ -> None

    let rec tryFindBinding (id:PiIdentifier) (l:PiJsonObject list) (assemblies:PiJsonObject seq) =
        match l with
        | [] -> None
        | h :: t ->
            match h with
            | ProcessBinding (bindingId, proc, _) when bindingId = id -> 
                Some(proc)
            | ProcessModuleRef (modId, inAsmOpt, _) -> 
                // look for an export
                match tryFindModule modId inAsmOpt assemblies with
                | Some(m) -> tryFindExport id m assemblies
                | None -> failwith "unable to find module"
            | _ ->
                tryFindBinding id t assemblies

    new(start:PiJsonObject) = PiContext(start, [])

    member this.CurrentProcess with get() = proc

    member this.Parent
        with get() = 
            match parents with
            | [] -> None
            | h :: _ -> Some(h)

    member this.SplitComposition(left:PiJsonObject, right:PiJsonObject) =
        this.NextProcess(left)
        let contextRight = PiContext(right, parents)
        (this, contextRight)

    member this.TryFindBinding(id:PiIdentifier, assemblies:PiJsonObject seq) =
        tryFindBinding id parents assemblies

    member this.TryFindModule(modId:PiIdentifier, inAsmOpt:PiIdentifier option, assemblies:PiJsonObject seq) =
        tryFindModule modId inAsmOpt assemblies

    member this.NextChild(continuation:PiJsonObject) =
        parents <- proc :: parents
        proc <- continuation

    member this.NextProcess(continuation:PiJsonObject) =
        proc <- continuation

    member this.AsJson() : PiJsonObject =
        let parentsArray = parents |> List.map (fun m -> m :> obj) |> List.toArray
        [
            { Label="Type"; Value="PiContext" :> obj};
            { Label="Proc"; Value=proc :> obj};
            { Label="Parents"; Value=parentsArray :> obj};
        ];

    static member FromJson(json:PiJsonObject) =
        match TryGetMemberValue<string> json "Type" with
        | Some("PiContext") -> 
            let proc = 
                match TryGetMemberValue<PiJsonObject> json "Proc" with
                | Some(p) -> p
                | None -> failwith "bad"
            let parents = 
                match TryGetMemberValue<PiJsonArray> json "Parents" with
                | Some(p) -> p |> Array.toList |> List.map (fun x -> x :?> PiJsonObject)
                | None -> failwith "bad"
            PiContext(proc, parents)
        | _ -> failwith "bad"

type GuardedContext =
    {
        CurrentContext  : PiContext;
        Prefix          : PiJsonObject;
        Continuation    : PiJsonObject;
    } with
        member this.AsJson() : PiJsonObject =
            [
                { Label="Type"; Value="GuardedContext" :> obj};
                { Label="CurrentContext"; Value=this.CurrentContext.AsJson() :> obj};
                { Label="Prefix"; Value=this.Prefix :> obj};
                { Label="Continuation"; Value=this.Continuation :> obj};
            ];

        static member FromJson(json:PiJsonObject) = 
            match TryGetMemberValue<string> json "Type" with
            | Some("GuardedContext") -> 
                let context = 
                    match TryGetMemberValue<PiJsonObject> json "CurrentContext" with
                    | Some(c) -> PiContext.FromJson(c)
                    | None -> failwith "bad"
                let prefix = 
                    match TryGetMemberValue<PiJsonObject> json "Prefix" with
                    | Some(p) -> p 
                    | None -> failwith "bad"
                let continuation = 
                    match TryGetMemberValue<PiJsonObject> json "Continuation" with
                    | Some(c) -> c 
                    | None -> failwith "bad"
                { CurrentContext=context; Prefix=prefix; Continuation=continuation; }
            | _ -> failwith "bad"
            
type TransitionContext = 
    | ChannelMatch      of Out      : GuardedContext *
                           Inp      : GuardedContext
    | Unobservable      of Guard    : GuardedContext
    | NoTransition
    with
        member this.AsJson() : PiJsonObject =
            match this with
            | TransitionContext.ChannelMatch(outGuard, inpGuard) ->
                [
                    { Label="Type"; Value="TransitionContext.ChannelMatch" :> obj};
                    { Label="Out"; Value=outGuard.AsJson() :> obj };
                    { Label="Inp"; Value=inpGuard.AsJson() :> obj };
                ];
            | TransitionContext.Unobservable(guard) ->
                [
                    { Label="Type"; Value="TransitionContext.Unobservable" :> obj};
                    { Label="Guard"; Value=guard.AsJson() :> obj };
                ];
            | TransitionContext.NoTransition ->
                [
                    { Label="Type"; Value="TransitionContext.NoTransition" :> obj};
                ];

        static member FromJson(json:PiJsonObject) = 
            match TryGetMemberValue<string> json "Type" with
            | Some("TransitionContext.ChannelMatch") -> 
                let outGuard = 
                    match TryGetMemberValue<PiJsonObject> json "Out" with
                    | Some(g) -> GuardedContext.FromJson(g)
                    | None -> failwith "bad"
                let inpGuard = 
                    match TryGetMemberValue<PiJsonObject> json "Inp" with
                    | Some(g) -> GuardedContext.FromJson(g)
                    | None -> failwith "bad"
                TransitionContext.ChannelMatch(outGuard, inpGuard)
            | Some("TransitionContext.Unobservable") -> 
                let guard = 
                    match TryGetMemberValue<PiJsonObject> json "Guard" with
                    | Some(g) -> GuardedContext.FromJson(g)
                    | None -> failwith "bad"
                TransitionContext.Unobservable(guard)
            | Some("TransitionContext.NoTransition") -> 
                TransitionContext.NoTransition
            | _ -> failwith "bad"

type PiJsonWriter (tw:TextWriter, depthOpt:int option) =
    let mutable depth = 0

    let IncDepth() = depth <- depth + 1
    let DecDepth() = depth <- depth - 1
    
    new (tw:TextWriter) = PiJsonWriter(tw, None)

    member this.WriteNull() =
        tw.Write("null")

    member this.WriteBool(value:bool) =
        tw.Write(if value then "true" else "false")

    member this.WriteInt(value:int) =
        tw.Write(value)

    member this.WriteFloat(value:float) = 
        tw.Write(value)

    member this.WriteString(s:string) =
        // todo: better json encoding of string
        tw.Write("\"")
        tw.Write(s.Replace("\"", "\\\""))
        tw.Write("\"")

    member this.Write(json:PiJsonObject) =
        match depthOpt with
        | Some(maxdepth) when maxdepth <= depth ->
            tw.Write("{ ... }")
        | _ -> 
            IncDepth()
            tw.Write("{")
            let length = json.Length
            json |>
                List.iteri (fun i m -> 
                                this.Write(m)
                                if i < length - 1 then 
                                    tw.Write(",")
                           )
            tw.Write("}")
            DecDepth()
        
    member this.Write({Label=label; Value=value;}:PiJsonPair) =
        this.WriteString(label)
        tw.Write(":")
        this.Write(value)
    
    member this.Write(json:PiJsonArray) =
        match depthOpt with
        | Some(maxdepth) when maxdepth <= depth ->
            tw.Write("[ ... ]")
        | _ -> 
            IncDepth()
            tw.Write("[")
            match json.Length with
            | 0 -> ()
            | length ->
                for i = 0 to length - 2 do
                    this.Write(json.[i])
                    tw.Write(",")
                this.Write(json.[length-1])
            tw.Write("]")
            DecDepth()

    member this.Write(context:PiContext) = 
        let json = context.AsJson()
        this.Write(json)

    member this.Write(guard:GuardedContext) =
        let json = guard.AsJson()
        this.Write(json)

    member this.Write(txc:TransitionContext) =
        let json = txc.AsJson()
        this.Write(json)

    member this.Write(data:obj) =
        match data with
        | null -> this.WriteNull()
        | :? string as json -> this.WriteString(json)
        | :? int as json -> this.WriteInt(json)
        | :? float as json -> this.WriteFloat(json)
        | :? bool as json -> this.WriteBool(json)
        | :? PiJsonObject as json -> this.Write(json)
        | :? PiJsonArray as json -> this.Write(json)
        | :? PiContext as context -> this.Write(context)
        | :? GuardedContext as guard -> this.Write(guard)
        | :? TransitionContext as txc -> this.Write(txc)
        | _ -> failwith "unexpected json data type"


type PiFSharpWriter (tw:TextWriter, depthOpt:int option) =
    let mutable depth = 0

    let IncDepth() = depth <- depth + 1
    let DecDepth() = depth <- depth - 1
    
    new (tw:TextWriter) = PiFSharpWriter(tw, None)

    member this.WriteNull() =
        tw.Write("null")

    member this.WriteBool(value:bool) =
        tw.Write(if value then "true" else "false")

    member this.WriteInt(value:int) =
        tw.Write(value)

    member this.WriteFloat(value:float) = 
        tw.Write(value)

    member this.WriteString(s:string) =
        // todo: better json encoding of string
        tw.Write("\"")
        tw.Write(s.Replace("\"", "\\\""))
        tw.Write("\"")

    member this.Write(json:PiJsonObject) =
        match depthOpt with
        | Some(maxdepth) when maxdepth <= depth ->
            tw.Write("[]")
        | _ -> 
            IncDepth()
            tw.Write("[")
            let length = json.Length
            json |>
                List.iter (fun m -> 
                               this.Write(m)
                               tw.Write(";")
                           )
            tw.Write("]")
            DecDepth()
        
    member this.Write({Label=label; Value=value;}:PiJsonPair) =
        tw.Write("{Label=")
        this.WriteString(label)
        tw.Write(";Value=")
        this.Write(value)
        tw.Write(";}")
    
    member this.Write(json:PiJsonArray) =
        match depthOpt with
        | Some(maxdepth) when maxdepth <= depth ->
            tw.Write("[||]")
        | _ -> 
            IncDepth()
            tw.Write("[|")
            match json.Length with
            | 0 -> ()
            | length ->
                for i = 0 to length - 2 do
                    this.Write(json.[i])
                    tw.Write(" :> obj;")
                this.Write(json.[length-1])
                tw.Write(":>obj")
            tw.Write("|]")
            DecDepth()

    member this.Write(context:PiContext) = 
        let json = context.AsJson()
        this.Write(json)

    member this.Write(guard:GuardedContext) =
        let json = guard.AsJson()
        this.Write(json)

    member this.Write(txc:TransitionContext) =
        let json = txc.AsJson()
        this.Write(json)

    member this.Write(data:obj) =
        match data with
        | null -> this.WriteNull()
        | :? string as json -> this.WriteString(json)
        | :? int as json -> this.WriteInt(json)
        | :? float as json -> this.WriteFloat(json)
        | :? bool as json -> this.WriteBool(json)
        | :? PiJsonObject as json -> this.Write(json)
        | :? PiJsonArray as json -> this.Write(json)
        | :? PiContext as context -> this.Write(context)
        | :? GuardedContext as guard -> this.Write(guard)
        | :? TransitionContext as txc -> this.Write(txc)
        | _ -> failwith "unexpected json data type"

type PiJsonCloner() =
    member this.Clone(json:PiJsonObject) : PiJsonObject =
        json |> List.map (fun m -> this.Clone(m))
        
    member this.Clone({Label=label; Value=value;}:PiJsonPair) =
        {Label=label; Value=this.Clone(value);}
    
    member this.Clone(json:PiJsonArray) : PiJsonArray =
        json |> Array.map (fun e -> this.Clone(e))

    member this.Clone(data:obj) =
        match data with
        | null -> null
        | :? string
        | :? int
        | :? float -> data
        | :? PiJsonObject as json -> this.Clone(json) :> obj
        | :? PiJsonArray as json -> this.Clone(json) :> obj
        | _ -> failwith "unexpected json data type"

let WriteJson (tw:TextWriter) (json:obj) =
    let ptw = PiJsonWriter(tw)
    ptw.Write(json)

let WriteJsonToString (json:obj) =
    use sw = new StringWriter()
    let ptw = PiJsonWriter(sw)
    ptw.Write(json)
    sw.Flush()
    sw.ToString()

let WriteJsonToFSharp (json:obj) =
    use sw = new StringWriter()
    let ptw = PiFSharpWriter(sw)
    ptw.Write(json)
    sw.Flush()
    sw.ToString()

let WriteJsonWithMaxDepth (tw:TextWriter) (json:obj) (maxdepth:int) =
    let ptw = PiJsonWriter(tw, Some(maxdepth))
    ptw.Write(json)

let CloneJson (json:PiJsonObject) = 
    let jsc = PiJsonCloner()
    jsc.Clone(json)

