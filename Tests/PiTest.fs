module PiTest

open System
open System.IO

open JsonPi
open JsonPi.Data
open JsonPi.PiRuntime

type PiTestCase =
    | AsString                  of Name : string *
                                   Code : string
    | AsStringWithExtensions    of Name : string *
                                   Code : string *
                                   Resolver : PiExtensionResolver
    | AsFile                    of Name : string *
                                   Path : string
    | AsFileWithExtensions      of Name : string *
                                   Path : string  *
                                   Resolver : PiExtensionResolver

type PiTestResult =
    | Unknown
    | Passed
    | Failed of string

type ITextExpected =
    abstract member GetExpected : string -> PiTraceEvent list

type PiTestModule (path:string, cases:PiTestCase list) =
    
    let PrintPiEvent (sw:TextWriter) (ev:PiTraceEvent) =
        let fsw = PiFSharpWriter(sw)

        match ev with
        | PiTraceEvent.PushProcess(p) ->
            sw.Write("            PiTraceEvent.PushProcess([]")
            //fsw.Write(p)
            sw.WriteLine(")")
        | PiTraceEvent.RunProcess(p) ->
            sw.Write("            PiTraceEvent.RunProcess([]")
            //fsw.Write(p)
            sw.WriteLine(")")
        | PiTraceEvent.PutPrefix(pfx, continuation) ->
            sw.Write("            PiTraceEvent.PutPrefix(")
            fsw.Write(pfx)
            sw.Write(",[]")
            //fsw.Write(continuation)
            sw.WriteLine(")")
        | PiTraceEvent.GetPrefix(pfx, continuation) ->
            sw.Write("            PiTraceEvent.GetPrefix(")
            fsw.Write(pfx)
            sw.Write(",[]")
            //fsw.Write(continuation)
            sw.WriteLine(")")
        | PiTraceEvent.TransitionOut(channel, outNames, _) ->
            sw.Write("            PiTraceEvent.TransitionOut(")
            fsw.Write(channel)
            sw.Write(",")
            fsw.Write(outNames)
            sw.WriteLine(", [])")
        | PiTraceEvent.TransitionInp(channel, outNames, inpNames, _) ->
            sw.Write("            PiTraceEvent.TransitionInp(")
            fsw.Write(channel)
            sw.Write(",")
            fsw.Write(outNames)
            sw.Write(",")
            fsw.Write(inpNames)
            sw.WriteLine(", [])")
        | PiTraceEvent.TransitionSum(summation, whenPfx) ->
            sw.Write("            PiTraceEvent.TransitionSum(")
            fsw.Write(summation)
            sw.Write(",")
            fsw.Write(whenPfx)
            sw.WriteLine(")")
        | PiTraceEvent.TransitionTau ->
            sw.WriteLine("            PiTraceEvent.TransitionTau")
        | PiTraceEvent.TransitionRep(s) ->
            sw.Write("            PiTraceEvent.TransitionRep(")
            fsw.Write(s)
            sw.WriteLine(")")
        | PiTraceEvent.RemoveSummation(summation) -> 
            sw.Write("            PiTraceEvent.RemoveSummation(")
            fsw.Write(summation)
            sw.WriteLine(")")

    let GenerateTestCase (sw:StreamWriter) (test:PiTestCase) =
        let pp = 
            match test with
            | PiTestCase.AsFile(_) 
            | PiTestCase.AsString(_) ->
                PiProcessor()
            | PiTestCase.AsFileWithExtensions(_, _, resolver)
            | PiTestCase.AsStringWithExtensions(_, _, resolver) ->
                PiProcessor(Some(resolver))

        use subscription =  pp.AsObservable() |> Observable.subscribe (PrintPiEvent sw)
        
        match test with
        | PiTestCase.AsString(name, code)
        | PiTestCase.AsStringWithExtensions(name, code, _) -> 
            Printf.fprintfn sw "    let %s_Observations = [" name

            pp.RunString(code)

            Printf.fprintfn sw "        ]"
            Printf.fprintfn sw ""
        | PiTestCase.AsFile(name, file)
        | PiTestCase.AsFileWithExtensions(name, file, _) ->
            Printf.fprintfn sw "    let %s_Observations = [" name

            pp.RunFile(file)

            Printf.fprintfn sw "        ]"
            Printf.fprintfn sw ""
            

    let GenerateTestMatch (sw:StreamWriter) (test:PiTestCase) =
        match test with
        | PiTestCase.AsString(name, _)
        | PiTestCase.AsStringWithExtensions(name, _, _) -> 
            Printf.fprintfn sw "            | \"%s\" -> %s_Observations" name name
        | _ -> ()

    let getTag (a:'a) = 
        let (uc,_) = Microsoft.FSharp.Reflection.FSharpValue.GetUnionFields(a, typeof<'a>)
        uc.Name

    let CompareEvents (ev:PiTraceEvent) (exp:PiTraceEvent) =
        use sw1 = new StringWriter()
        PrintPiEvent sw1 ev
        sw1.Flush()

        use sw2 = new StringWriter()
        PrintPiEvent sw2 exp
        sw2.Flush()

        let same = ((sw1.ToString()) = (sw2.ToString()))
        if same
            then 0 
            else -1

    let RunTestCase (ex:ITextExpected) (test:PiTestCase) =
        let name = 
            match test with
            | PiTestCase.AsString (n, _) -> n
            | PiTestCase.AsStringWithExtensions(n, _, _) -> n
            | PiTestCase.AsFile (n, _) -> n
            | PiTestCase.AsFileWithExtensions(n, _, _) -> n

        let expected = ex.GetExpected(name)

        let mutable result = PiTestResult.Unknown
        let mutable events : PiTraceEvent list = []

        let addevent () (ev:PiTraceEvent) = 
            events <- ev :: events

        let resolver =
            match test with
            | PiTestCase.AsStringWithExtensions(_, _, r) 
            | PiTestCase.AsFileWithExtensions(_, _, r) -> Some(r)
            | _ -> None

        let pp = PiProcessor(resolver)
        let subscription =  
            pp.AsObservable() 
                |> Observable.scan addevent ()
                |> Observable.subscribe (fun () -> ())

        match test with 
        | PiTestCase.AsString(n, code) 
        | PiTestCase.AsStringWithExtensions(n, code, _) ->
            pp.RunString(code)
        | _ -> failwith "unexpected"

        events <- List.rev events
        let comp = 
            List.compareWith CompareEvents expected events

        match comp with
        | 0 ->
            PiTestResult.Passed
        | _ ->
            PiTestResult.Failed(name)

    member this.Generate() =
        use modfile = File.CreateText(path)
        Printf.fprintfn modfile "namespace Generated_%s" (Path.GetFileNameWithoutExtension(path))
        Printf.fprintfn modfile ""
        Printf.fprintfn modfile "open System"
        Printf.fprintfn modfile ""
        Printf.fprintfn modfile "open JsonPi"
        Printf.fprintfn modfile "open JsonPi.Data"
        Printf.fprintfn modfile "open JsonPi.PiRuntime"
        Printf.fprintfn modfile ""
        Printf.fprintfn modfile "open PiTest"
        Printf.fprintfn modfile ""
        
        Printf.fprintfn modfile "type %s () =" (Path.GetFileNameWithoutExtension(path))

        cases |>
            List.iter (GenerateTestCase modfile)

        Printf.fprintfn modfile "    interface ITextExpected with"
        Printf.fprintfn modfile "        member this.GetExpected(name:string) ="
        Printf.fprintfn modfile "            match name with"

        cases |>
            List.iter (GenerateTestMatch modfile)
        
        Printf.fprintfn modfile "            | _ -> failwith \"unexpected\""
    
    member this.RunOne(name:string, ex:ITextExpected) =
        let test = 
            cases |> List.find 
                (function 
                 | PiTestCase.AsString(n, _) 
                 | PiTestCase.AsStringWithExtensions(n, _, _)
                 | PiTestCase.AsFile(n, _) 
                 | PiTestCase.AsFileWithExtensions(n, _, _) when n = name -> true
                 | _ -> false
                )

        RunTestCase ex test

    member this.RunAll(ex:ITextExpected) =
        let failedCases = 
            cases 
                |> List.choose 
                    (fun test -> 
                        match RunTestCase ex test with
                        | PiTestResult.Passed -> None
                        | r -> Some(r) 
                    )

        match failedCases.Length with
        | 0 -> PiTestResult.Passed
        | _ -> failedCases.Item(0)
