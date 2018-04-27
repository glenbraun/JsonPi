open System

open JsonPi
open JsonPi.Data
open PiTest

open Generated_BasicTests

type MyExtension () =
    interface IPiExtension with
        member this.OnOutput (channel:PiJsonObject) (outNames:PiJsonArray) : PiJsonObject option =
            printfn "Output extension"
            match channel with
            | PiName (id, _, _) when id = "ex`2" -> 
                let program = PiParser.ParseFromString "b(g) (continue)"
                match program with
                | AssemblyEntryProcess p ->
                    Some (p)
                | _ -> failwith "bad"
            | _ -> None

        member this.OnInput (channel:PiJsonObject) (outNames:PiJsonArray) (inpNames:PiJsonArray) : PiJsonObject option =
            printfn "Input extension"
            match channel with
            | PiName (id, _, _) when id = "ex`2" -> 
                let program = PiParser.ParseFromString "b<x> (continue)"
                match program with
                | AssemblyEntryProcess p ->
                    Some (p)
                | _ -> failwith "bad"
            | _ -> None


let resolver : PiExtensionResolver = 
    fun (nameType:PiIdentifier) (data:obj option) ->
        match nameType with
        | "MyExtension" -> Some((new MyExtension()) :> IPiExtension)
        | _ -> None
         
[<EntryPoint>]
let main argv = 
    let pp = PiProcessor(Some(resolver))
    pp.RunString("new (ex) new (ex:MyExtension) ex(x) (* b<x> *) c(h);|ex<y> (* b(g) *) c<g>;")
    
    let basictests = [
            PiTestCase.AsString("TestInaction", ";");
            PiTestCase.AsString("TestComposeInaction", ";|;");
            PiTestCase.AsString("TestSimpleComm", "a<x>; | a(y);");
            PiTestCase.AsString("TestSimpleCommMultiParam2to2", "x<a,b>; | x(f,g) y<f,g>; | y(m,n);");
            PiTestCase.AsString("TestSimpleCommMultiParam2To1", "x<a,b>; | x(f) y<f>; | y(m,n);");
            PiTestCase.AsString("TestSimpleCommMultiParam3To2", "x<a,b,c>; | x(f,g) y<g>; | y(m,n);");
            PiTestCase.AsString("TestSimpleCommMultiParam1To2", "x<a>; | x(f,g) y<f,g>; | y(m,n);");
            PiTestCase.AsString("TestSimpleCommMultiParam2To3", "x<a,b>; | x(f,g,h) y<f,g,h>; | y(m,n,o);");
            PiTestCase.AsString("TestMultiComm", "(x(z) z<a>; | x<w> y<w>;) | y(v) v(u);");
            PiTestCase.AsString("TestSummationComposition", "a<x> b<x> c<x> d<x>; | a(y) b(y) c(y) d(y);")
            PiTestCase.AsString("TestCommWithSimpleMatch", "c(x);|[b=b]c<y>;");
            PiTestCase.AsString("TestCommWithMatch", "x(z) [z=y]a(b);|x<y>;|a<c>;");
            PiTestCase.AsString("TestCommWithMatchMultipleParams", "x<a,b> y(c); | x(f) [f=a,b]y<g>;")
            PiTestCase.AsString("TestLeftSumOut", "(choose when b<x> then ; when a<x> then ; end) | a(y);");
            PiTestCase.AsString("TestRightSumOut", "a(y); | (choose when b<x> then ; when a<x> then ; end)");
            PiTestCase.AsString("TestLeftSumInp", "(choose when b(x) then ; when a(x) then ; end) | a<y>;");
            PiTestCase.AsString("TestRightSumInp", "a<y>; | (choose when b(x) then ; when a(x) then ; end)");
            PiTestCase.AsString("TestSumMatchSum", "(choose when c<x> then ; when a(y) then ; end) | (choose when b<x> then ; when a<x> then ; end)");
            PiTestCase.AsString("TestLeftDefaultSumOut", "choose when a<x> then ; default b<y>; end | b(z);");
            PiTestCase.AsString("TestRightDefaultSumOut", "b(z); | choose when a<x> then ; default b<y>; end");
            PiTestCase.AsString("TestLeftDefaultSumInp", "choose when a(x) then ; default b(y); end | b<z>;");
            PiTestCase.AsString("TestRightDefaultSumInp", "b<z>; | choose when a(x) then ; default b(y); end");
            PiTestCase.AsString("TestDefaultSumMatchDefaultSum", "choose when a<x> then ; default b<z>; end | choose when a<x> then ; default b(y); end");
            PiTestCase.AsString("TestBindingAndRef", "let P = a<x>; $P | a(y);");
            PiTestCase.AsString("TestModuleAndRef", "module A a<x>; using A a(y);");
            PiTestCase.AsString("TestModuleRefAndBindingRef", "module A let P = a<x>; ; using A $P | a(y);");
            PiTestCase.AsString("TestModuleRefAndBindingRef2", "module A let P1 = a<x>; let P2 = a<b>; ; using A $P2 | a(y);");
            PiTestCase.AsString("TestSimpleRestriction", "new (a) new (a) a(x);|a<y>;");
            PiTestCase.AsString("TestRestrictionWithSubstitute", "x<a>; | x(f) (new (f) new (f) y<f>;) | y(b);")
            PiTestCase.AsStringWithExtensions("TestSimpleRestrictionWithExtension", "new (a) new (a:MyExtension) a(x);|a<y>;", resolver);
            PiTestCase.AsStringWithExtensions("TestSimpleRestrictionWithExtensionAndJson", "new (a:MyExtension) = [true, 5, -12, 1.2093, -93.5, 1.8001e7, -391.8001e-7, {\"TestLabel\":\"TestValue\"}] a<x>; | a(y);", resolver);
            PiTestCase.AsStringWithExtensions("TestSimpleRestrictionWithExtensionReturn", "new (ex) new (ex:MyExtension) ex(x) (* b<x> *) c(h);|ex<y> (* b(g) *) c<g>;", resolver);
            PiTestCase.AsString("TestSimpleReplication", "(!(x(y);)) | (x<a> x<b> x<c>;)");
            PiTestCase.AsString("TestReplication", "(!(x<a> y(b);)) | (x(c) y<d> x(e);)");

            // Milner examples, https://pdfs.semanticscholar.org/5d25/0a3a14f68abb1ae0111d35b8220b4472b277.pdf
            PiTestCase.AsString("Milner_Page_6_Section_2_2", "x<y>; | x(u) u<v>; | x<z>;");
            PiTestCase.AsString("Milner_Page_7_1", "(new (x) ( x<y>; | x(u) u<v>;)) | x<z>;");
            // Same as Section 2.2 but has a replication.
            PiTestCase.AsString("Milner_Page_7_2", "x<y>; | (!(x(u) u<v>;)) | x<z>;");
            PiTestCase.AsString("Milner_Page_10_Section_3_1", "x(y,z); | x<y1, z1>; | x<y2, z2>;");
        ]

    let basicmod = PiTestModule("..\..\\..\\BasicTests.fs", basictests)

    let generate = false

    if generate then
        basicmod.Generate()
    else 
        let bb = BasicTests()
        //let r = basicmod.RunOne("TestSimpleCommMultiParam2to2", bb)
        let r = basicmod.RunAll(bb)
        if r <> PiTestResult.Passed then
            failwith "test failed"
        
    0 // return an integer exit code
