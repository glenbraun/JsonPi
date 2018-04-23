open System

open JsonPi
open JsonPi.Data

type TennisApi (data:obj option) = 
    interface IPiExtension with
        member this.OnOutput (channel:PiJsonObject) (outNames:PiJsonArray) = 
            match outNames with
            | [| api; a; b; c; |] ->
                match (api, a, b, c) with
                | (PiName (apiId, _, _), player, date, PiName (resultId, _, _)) when apiId = "Signup" -> 
                    // RobinswoodFridayTennis<Signup, player, date, result>
                    let proc = 
                         match PiParser.ParseFromString (resultId + "<Confirmed>;") with
                         | AssemblyEntryProcess p -> p
                         | _ -> failwith "bad"
                            
                    Some( proc )
                | _ ->
                    None
            | _ -> 
                None
        
        member this.OnInput (channel:PiJsonObject) (outNames:PiJsonArray) (inpNames:PiJsonArray) : PiJsonObject option =
            printfn "Input extension"
            None

let TennisResolver (nameType:PiIdentifier) (data:obj option) =
    match nameType with
    | "TennisApi" -> Some( TennisApi(data) :> IPiExtension)
    | _ -> None

[<EntryPoint>]
let main argv = 
    let program = PiParser.ParseFromFile "..\\..\\..\\Signup.jpi"
    let pjson = WriteJsonToString program

    let pp = PiProcessor(Some(TennisResolver))
    pp.RunProgram(program)

    0 // return an integer exit code
