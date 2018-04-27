module PiRepl

open System
open JsonPi
open JsonPi.Data
open System.IO

let mutable internal step = false
let mutable internal quit = false
let mutable internal multiline = false

let internal PrintTrace (ev:PiTraceEvent) =
    use sw = new StringWriter()
    let pcw = PiCodeWriter(sw)

    sw.Write("        ")
    match ev with
    | PiTraceEvent.PushProcess(p) ->
        sw.Write("Push  ")
        pcw.WriteProcess(p, 40)
    | PiTraceEvent.RunProcess(p) ->
        sw.Write("Eval  ")
        pcw.WriteProcess(p, 40)
    | PiTraceEvent.PutPrefix(pfx, continuation) ->
        sw.Write("Put   ")
        pcw.WritePrefix(pfx, 20)
        pcw.NextColumn(20)
        sw.Write("    (")
        pcw.WriteProcess(continuation, 20)
        sw.Write(")")
    | PiTraceEvent.GetPrefix(pfx, continuation) ->
        sw.Write("Get   ")
        pcw.WritePrefix(pfx, 20)
        pcw.NextColumn(20)
        sw.Write("    (")
        pcw.WriteProcess(continuation, 20)
        sw.Write(")")
    | PiTraceEvent.TransitionOut(channel, outNames, continuation) ->
        sw.Write("TxOut ")
        pcw.WriteName(channel, 20)
        pcw.Write("<", 20)
        pcw.WriteParams(outNames, 20)
        pcw.Write(">", 20)
        pcw.NextColumn(20)
        sw.Write("    (")
        pcw.WriteProcess(continuation, 20)
        sw.Write(")")
    | PiTraceEvent.TransitionInp(channel, outNames, inpNames, continuation) ->
        sw.Write("TxInp ")
        pcw.WriteName(channel, 20)
        pcw.Write("(", 20)
        pcw.WriteParams(outNames, 20)
        pcw.Write("/", 20)
        pcw.WriteParams(inpNames, 20)
        pcw.Write(")", 20)
        pcw.NextColumn(20)
        sw.Write("    (")
        pcw.WriteProcess(continuation, 20)
        sw.Write(")")
    | _ ->
        ()

    sw.Flush()
    let ps = sw.ToString()
    if ps.Trim().Length > 0 then
        Console.WriteLine(ps)

        if step 
            then Console.ReadKey(true) |> ignore

let mutable internal processor = PiProcessor()
let mutable internal subscription =  processor.AsObservable() |> Observable.subscribe PrintTrace

let Quit () = 
    quit <- true

let StepMode () = 
    step <- true

let RunMode () =
    step <- false

let ToggleMultiline () =
    multiline <- not(multiline)

let CommandUnknown (cmd:string) =
    Console.WriteLine("Unknown command '{0}'", cmd)

let Reset () = 
    subscription.Dispose()

    processor <- PiProcessor()
    subscription <- processor.AsObservable() |> Observable.subscribe PrintTrace

let ListPending () =
    let WritePending (guard:GuardedContext) =
        use sw = new StringWriter()
        let pcw = PiCodeWriter(sw)
        sw.Write("        ")
        pcw.WritePrefix(guard.Prefix, 30)
        pcw.NextColumn(30)
        pcw.Write("(", 30)
        pcw.WriteProcess(guard.Continuation, 40)
        pcw.Write(")", 40)
        sw.Flush()
        Console.WriteLine(sw.ToString())
        
    Console.WriteLine("        Prefix                            Continuation");
    Console.WriteLine("        ------                            ------------");
    processor.ListPending(WritePending)
    
let ExecutePi (pi:string) =
    try
        let program = PiParser.ParseFromString(pi)
        processor.RunProgram(program)
    with
        | exp -> Console.WriteLine("Exception: {0}", exp.Message)

let Help () =
    let msg = """JsonPi REPL Help
    Enter REPL Command or JsonPi statements. Commands start with a colon (':').
    
    :h or :help                 Display this help message.
    :q or :quit                 Exit from the JsonPi REPL program.
    :r or :run                  Enter into run mode. Execution runs continuously without stopping between steps.
    :s or :step                 Enter into step mode. Execution will wait for a key press between steps.
    :m or :multi                Toggles multiline mode. When in multiline mode enter 'go' to start execution.
    :l or :list                 Display the list of channels with pending receive or send continuations.
    :reset                      Reset the processor, clearing the list of pending recieve or send continuations.

    JsonPi Basic Syntax
    c<n1 [, n2...]>             Send name(s) on channel c.
    c(n1 [, n2...])             Receive name(s) on channel c.
    |                           Separates concurrent processes. ('|' vertical bar character)
    ;                           Terminates processes. (';' semicolon character)
    !P                          Replicate process P.
    new (n [':' t]) [ '=' JSON] Creates new name with optional type and optional JSON initialization data.
    
    See https://github.com/glenbraun/JsonPi readme for more detailed help.
    """
    Console.WriteLine(msg)


let rec Read (buffer, index, count) =
    if quit then
        0
    else 
        let prompt = 
            match (multiline, step) with
            | (false, false) -> "Run> "
            | (false, true) -> "Step> "
            | (true, false) -> "Run>> "
            | (true, true) -> "Step >> "
    
        Console.Write(prompt)

        let line = Console.ReadLine().Trim()
        
        let line2 = 
            if line = "go" 
            then Environment.NewLine
            else
                if multiline
                then 
                    if (line.StartsWith(":"))
                    then 
                        // this is a hack, but it is convienient to have these commands in multiline mode
                        match line with
                        | ":h" | ":help" -> Help(); " "
                        | ":l" | ":list" -> ListPending(); " "
                        | ":s" | ":step" -> StepMode(); " "
                        | ":r" | ":run" -> RunMode(); " "
                        | ":m" | ":multi" -> ToggleMultiline(); Environment.NewLine
                        | ":reset" -> Reset(); " "
                        | _ -> Console.WriteLine("Command not supported in multiline mode. Use :m to exit multiline mode."); " "
                    else line + " " 
                else line + Environment.NewLine    

        Array.blit (line2.ToCharArray()) 0 buffer index (line2.Length)
        line2.Length
