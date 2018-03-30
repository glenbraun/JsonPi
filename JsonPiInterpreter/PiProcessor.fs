namespace JsonPi

open System
open System.Collections.Generic

open JsonPi.Data
open JsonPi.PiRuntime


type PiProcessor () =
    let mutable outExtensions = Map.empty<PiIdentifier, PiExtension>
    let mutable inpExtensions = Map.empty<PiIdentifier, PiExtension>
    let pitrace = PiObservable<PiTraceEvent>()
    let assemblies = ResizeArray<PiJsonObject>()
    let ws = Stack<PiContext>()
    let pns = PiNamespace()

    let rec GetNextRestrictedId (id:PiIdentifier) =
        match id.IndexOf("`") with
        | -1 -> String.Intern(id + "`1")
        | index -> 
            let num = Int32.Parse(id.Substring(index+1))
            sprintf "%s`%d" (id.Substring(0, index)) (num+1)

    let AddAssembly asm = 
        if not(Seq.exists ((=) asm) (assemblies)) then
            assemblies.Add asm

    let PushProcess (context:PiContext) =
        pitrace.Next(PiTraceEvent.PushProcess(GetMemberValue<string> (context.CurrentProcess) "Type"))
        ws.Push(context)

    let rec MatchSummationPrefix (guard:GuardedContext) = 
        match guard.Prefix with
        | PrefixOutput (channel, outNames) ->
            match pns.TrySend(channel, guard) with
            | None ->
                TransitionContext.NoTransition
            | Some(matchedInput) ->
                TransitionContext.ChannelMatch(guard, matchedInput)
        | PrefixInput (channel, inNames) ->
            match pns.TryReceive(channel, guard) with
            | None ->
                TransitionContext.NoTransition
            | Some(matchedOutput) ->
                TransitionContext.ChannelMatch(matchedOutput, guard)
        | PrefixMatch (np1, np2, matchPfx) ->
            match (np1, np2) with
            | (PiName(id1, _, _), PiName(id2, _, _)) when id1 = id2 ->
                let matchGuard = 
                    { 
                        CurrentContext  = guard.CurrentContext;
                        Prefix          = matchPfx;
                        Continuation    = guard.Continuation
                    }
                MatchSummationPrefix matchGuard
            | _ ->
                TransitionContext.NoTransition
        | PrefixUnobservable ->
            TransitionContext.Unobservable(guard)
        | _ -> failwith "bad"

    let rec RunProcessSummation (context:PiContext) (ls:PiJsonArray) index : TransitionContext =
        if index < ls.Length then
            let whenSummation = ls.[index] 
            match whenSummation with
            | SummationPrefix (whenPfx, whenNext) -> 
                let guard = 
                    { 
                        CurrentContext  = context;
                        Prefix          = whenPfx;
                        Continuation    = whenNext
                    }
                match MatchSummationPrefix guard with
                | TransitionContext.NoTransition ->
                    RunProcessSummation context ls (index+1)
                | _ as tc -> tc
            | _ -> failwith "Unexpected summation type in sum."
        else
            TransitionContext.NoTransition

    let rec RunProcess (context:PiContext) : TransitionContext =
        pitrace.Next(PiTraceEvent.RunProcess(GetMemberValue<string> (context.CurrentProcess) "Type"))

        match context.CurrentProcess with
        | ProcessSummation summation -> 
            match summation with
            | SummationInaction ->
                TransitionContext.NoTransition
            | SummationPrefix (pfx, continuation) ->
                let guard = 
                    { 
                        CurrentContext  = context;
                        Prefix          = pfx;
                        Continuation    = continuation;
                    }
                MatchSummationPrefix guard
            | SummationSum (ls) ->
                RunProcessSummation context ls 0
            | _ -> failwith "bad"
        | ProcessComposition (left, right) ->
            let (contextLeft, contextRight) = context.SplitComposition(left, right)
            PushProcess contextLeft
            PushProcess contextRight
            TransitionContext.NoTransition
        | ProcessBinding (id, proc, continuation) ->
            context.NextChild(continuation)
            PushProcess context
            TransitionContext.NoTransition
        | ProcessBindingRef id ->
            match context.TryFindBinding(id, assemblies) with
            | Some(bindProc) ->
                let bindClone = CloneJson bindProc
                context.NextProcess(bindClone)
                PushProcess context
                TransitionContext.NoTransition
            | None ->
                failwith "Unable to resolve process reference."
        | ProcessModuleRef (id, inAsm, continuation) -> 
            match context.TryFindModule(id, inAsm, assemblies) with
            | Some(pimod) ->
                let modProcClone = CloneJson pimod
                let newComp = CreateProcessComposition continuation modProcClone
                context.NextChild(newComp)
                PushProcess context
                TransitionContext.NoTransition
            | None ->
                failwith "unable to find module"
        | ProcessRestriction (name, continuation) ->
            let id = GetMemberValue name "Id"
            let findName = CreateName id None None
            let restrictedId = GetNextRestrictedId id
            let restrictedName = CreateName restrictedId (TryGetMemberValue name "NameType") (TryGetMemberValue name "Data")
            Substitute context.CurrentProcess [| restrictedName :> obj |] [| findName :> obj |]
            context.NextChild(continuation)
            PushProcess context
            TransitionContext.NoTransition
        | ProcessReplication repProc -> 
            let repProcClone = CloneJson repProc
            context.NextChild(repProcClone)
            PushProcess context
            TransitionContext.NoTransition
        | _ -> failwith "bad"

    let TransitionOut (context:PiContext) (channel:PiJsonObject) (outNames:PiJsonArray) (continuation:PiJsonObject)  =
        pitrace.Next(PiTraceEvent.TransitionOut(channel, outNames))

        match channel with
        | PiName (id, nameTypeOpt, _) ->
            match nameTypeOpt with
            | Some(nt) -> 
                match outExtensions |> Map.tryFind nt with
                | Some(PiExtension.OnOutput(fExtOut)) -> 
                    let extResult = fExtOut channel outNames
                    match extResult with
                    | Some(extContinuation) ->
                        // TODO: let continue ...
                        context.NextProcess(extContinuation)
                    | None ->
                        context.NextProcess(continuation)
                | _ ->
                    context.NextProcess(continuation)
            | None ->
                context.NextProcess(continuation)
        | _ -> failwith "bad"

    let TransitionInp (context:PiContext) (channel:PiJsonObject) (outNames:PiJsonArray) (inpNames:PiJsonArray) (continuation:PiJsonObject)  =
        pitrace.Next(PiTraceEvent.TransitionInp(channel, outNames, inpNames))

        match channel with
        | PiName (id, nameTypeOpt, _) ->
            match nameTypeOpt with
            | Some(nt) -> 
                match inpExtensions |> Map.tryFind nt with
                | Some(PiExtension.OnInput(fExtInp)) -> 
                    let extResult = fExtInp channel outNames inpNames
                    match extResult with
                    | Some(extContinuation) ->
                        // TODO: let continue ...
                        context.NextProcess(extContinuation)
                    | None ->
                        context.NextProcess(continuation)
                | _ ->
                    context.NextProcess(continuation)
            | None ->
                context.NextProcess(continuation)
        | _ -> failwith "bad"

        Substitute (context.CurrentProcess) outNames inpNames

    let TransitionTau (context:PiContext) (continuation:PiJsonObject) =
        pitrace.Next(PiTraceEvent.TransitionTau)

        context.NextProcess(continuation)

    let TransitionSum (guard:GuardedContext) (summation:PiJsonObject) (whenPfx:PiJsonObject) =
        pitrace.Next(PiTraceEvent.TransitionSum(summation, whenPfx))
        
        match summation with
        | SummationSum ls ->
            for i = 0 to ls.Length - 1 do
                match ls.[i] with
                | SummationPrefix (sumPfx, _) when sumPfx <> whenPfx ->
                    let channel = GetPrefixChannel sumPfx
                    pitrace.Next(PiTraceEvent.RemoveSummation(summation))
                    pns.RemoveSummation(channel, guard.CurrentContext)
                | _ -> ()
        | _ -> failwith "Unexpected summation type."

    let TransitionRep (guard:GuardedContext) (repProc:PiJsonObject) =
        let {CurrentContext = context; Continuation=continuation; } = guard

        match context.CurrentProcess with
        | ProcessSummation(SummationInaction) ->
            pitrace.Next(PiTraceEvent.TransitionRep("Inaction"))
            let repProcClone = CloneJson repProc
            context.NextProcess(repProcClone)
        | _ ->
            pitrace.Next(PiTraceEvent.TransitionRep("Continuation"))
            let left = CloneJson repProc
            let right = continuation
            let comp = CreateProcessComposition left right
            context.NextProcess(comp)

    let (|TxSum|_|) (guard:GuardedContext) =
        let {CurrentContext = context; Prefix=whenPfx; Continuation=continuation; } = guard

        match context.CurrentProcess with
        | ProcessSummation summation ->
            match summation with
            | SummationSum _ ->
                let fSum = fun () ->
                    TransitionSum guard summation whenPfx
                Some(fSum)
            | _ -> None
        | _ -> None

    let (|TxRep|_|) (guard:GuardedContext) =
        let {CurrentContext = context; Continuation=continuation; } = guard
        match context.Parent with
        | Some(parent) ->
            match parent with
            | ProcessReplication(repProc) ->
                let fRep = fun () ->
                    TransitionRep guard repProc
                Some(fRep)
            | _ -> 
                None
        | _ -> 
            None

    let (|TxOut|_|) (outGuard:GuardedContext) =
        let {CurrentContext = outContext; Prefix=pfxOut; Continuation=continuation; } = outGuard

        match pfxOut with
        | PrefixOutput (channel, outNames) ->
            let fSum = 
                match outGuard with
                | TxSum f -> f
                | _ -> function () -> ()

            let fRep =
                match outGuard with
                | TxRep f -> f
                | _ -> function () -> ()

            let fOut = fun () -> 
                fSum()
                TransitionOut outContext channel outNames continuation
                fRep()

            Some(fOut)
        | _ -> None
 
    let (|TxInp|_|) (outGuard:GuardedContext, inpGuard:GuardedContext) =
        let {CurrentContext = outContext; Prefix=pfxOut; Continuation=outContinuation; } = outGuard
        let {CurrentContext = inpContext; Prefix=pfxInp; Continuation=inpContinuation; } = inpGuard

        match pfxOut with
        | PrefixOutput (_, outNames) ->
            match pfxInp with
            | PrefixInput (channel, inpNames) ->
                let fSum = 
                    match inpGuard with
                    | TxSum f -> f
                    | _ -> function () -> ()

                let fRep =
                    match inpGuard with
                    | TxRep f -> f
                    | _ -> function () -> ()

                let fInp = fun () -> 
                    fSum()
                    TransitionInp inpContext channel outNames inpNames inpContinuation
                    fRep()

                Some(fInp)
            | _ -> 
                None
        | _ -> 
            None

    let (|TxTau|_|) (guard:GuardedContext) =
        let {CurrentContext = context; Prefix=pfx; Continuation=continuation; } = guard

        match pfx with
        | PrefixUnobservable ->
            let fSum = 
                match guard with
                | TxSum f -> f
                | _ -> function () -> ()

            let fRep =
                match guard with
                | TxRep f -> f
                | _ -> function () -> ()

            let fTau = fun () -> 
                fSum()
                TransitionTau context continuation 
                fRep()

            Some(fTau)
        | _ ->
            None

    let (|TxComm|_|) (outGuard:GuardedContext, inpGuard:GuardedContext) =
        match ( outGuard,  (outGuard, inpGuard)) with
        | (TxOut fOut, TxInp fInp) -> 
            let fComm = fun () ->
                let pOut = fOut()
                let pInp = fInp()
                PushProcess (outGuard.CurrentContext)
                PushProcess (inpGuard.CurrentContext)
            Some(fComm)
        | _ ->
            None

    let TransitionProcess (tc:TransitionContext) =
        match tc with
        | TransitionContext.NoTransition -> ()
        | TransitionContext.ChannelMatch(outGuard, inpGuard) -> 
            match (outGuard, inpGuard) with
            | TxComm f -> f()
            | _ -> failwith "Unexpexted transition context."
        | TransitionContext.Unobservable(guard) ->
            match guard with
            | TxTau f -> 
                f()
                PushProcess (guard.CurrentContext)
            | _ -> failwith "Unexpexted transition context."

    member internal this.RunStack () =
        match ws.Count with
        | 0 ->
            pitrace.Completed()
        | _ ->
            let next = ws.Pop()
            let tcl = RunProcess next
            TransitionProcess tcl
            this.RunStack()

    member this.RunString(pitext:string) =
        let program = PiParser.ParseFromString pitext
        AddAssembly program

        match program with
        | AssemblyEntryProcess pstart ->
            let context = PiContext(pstart)
            PushProcess context
        | _ -> failwith "bad"

        this.RunStack()

    member this.AddExtension(nameType:PiIdentifier, extension:PiExtension) =
        match extension with
        | PiExtension.OnOutput(_) ->
            match Map.containsKey nameType outExtensions with
            | false ->
                outExtensions <- Map.add nameType extension outExtensions
            | true ->
                outExtensions <- Map.add nameType extension (Map.remove nameType outExtensions)
        | PiExtension.OnInput(_) ->
            match Map.containsKey nameType inpExtensions with
            | false ->
                inpExtensions <- Map.add nameType extension inpExtensions
            | true ->
                inpExtensions <- Map.add nameType extension (Map.remove nameType inpExtensions)

    member this.AsObservable() = pitrace.AsObservable