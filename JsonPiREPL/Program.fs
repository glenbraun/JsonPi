module JsonPiRepl

open System

open JsonPi
open JsonPi.Data
open Microsoft.FSharp.Text.Lexing
open PiRepl
open PiReplParser

[<EntryPoint>]
let main argv =

    let lexbuf = LexBuffer<char>.FromFunction(PiRepl.Read)
    let result = PiReplParser.start PiReplLexer.repl lexbuf

    0 // return an integer exit code
