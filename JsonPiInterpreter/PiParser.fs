module JsonPi.PiParser

open Microsoft.FSharp.Text.Lexing

let ParseFromString text =
    let lexbuf = LexBuffer<char>.FromString text
    let result = PiParserInternal.start PiLexerInternal.pi lexbuf
    result

let ParseFromFile (fileName:string) = 
    let fi = System.IO.FileInfo(fileName)
    use textReader = new System.IO.StreamReader(fileName)
    let lexbuf = LexBuffer<char>.FromTextReader textReader
    let result = PiParserInternal.start PiLexerInternal.pi lexbuf
    //result.Id <- fi.Name
    result

