open System

open JsonPi
open JsonPi.Data
open PiTest

open Generated_BasicTests

let extOutput = 
    PiExtension.OnOutput(
        fun channel outNames ->
            printfn "Output extension"
            None
    )

let extInput = 
    PiExtension.OnInput(
        fun channel outNames inNames ->
            printfn "Input extension"
            None
    )

let extensions = [("MyExtension", extOutput); ("MyExtension", extInput);]

[<EntryPoint>]
let main argv = 
 
    let pp = PiProcessor()
    pp.AddExtension("MyExtension", extOutput)
    pp.AddExtension("MyExtension", extInput)
    pp.RunString("(choose when b<x> then ; when a<x> then ; end) | a(y);")

    let basictests = [
            PiTestCase.AsString("TestInaction", ";");
            PiTestCase.AsString("TestComposeInaction", ";|;");
            PiTestCase.AsString("TestSimpleComm", "a<x>; | a(y);");
            PiTestCase.AsString("TestMultiComm", "(x(z) z<a>; | x<w> y<w>;) | y(v) v(u);");
            PiTestCase.AsString("TestSummationComposition", "a<x> b<x> c<x> d<x>; | a(y) b(y) c(y) d(y);")
            PiTestCase.AsString("TestCommWithSimpleMatch", "c(x);|[b=b]c<y>;");
            PiTestCase.AsString("TestCommWithMatch", "x(z) [z=y]a(b);|x<y>;|a<c>;");
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
            PiTestCase.AsStringWithExtensions("TestSimpleRestrictionWithExtension", "new (a) new (a:MyExtension) a(x);|a<y>;", extensions);
            PiTestCase.AsStringWithExtensions("TestSimpleRestrictionWithExtensionAndJson", "new (a:MyExtension) = [true, 5, -12, 1.2093, -93.5, 1.8001e7, -391.8001e-7, {\"TestLabel\":\"TestValue\"}] a<x>; | a(y);", extensions);
            PiTestCase.AsString("TestSimpleReplication", "(!(x(y);)) | (x<a> x<b> x<c>;)");
            PiTestCase.AsString("TestReplication", "(!(x<a> y(b);)) | (x(c) y<d> x(e);)");
        ]

    let basicmod = PiTestModule("..\\..\\BasicTests.fs", basictests)

    let generate = false

    if generate then
        basicmod.Generate()
    else 
        let bb = BasicTests()
        let r = basicmod.RunAll(bb)
        if r <> PiTestResult.Passed then
            failwith "test failed"
        
    0 // return an integer exit code
