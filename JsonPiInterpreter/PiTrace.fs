namespace JsonPi

open System
open System.Diagnostics

open JsonPi
open JsonPi.Data

type PiTraceEvent =
    | PushProcess       of Process:PiJsonObject
    | RunProcess        of Process:PiJsonObject
    | PutPrefix         of Prefix:PiJsonObject *
                           Continuation:PiJsonObject
    | GetPrefix         of Prefix:PiJsonObject *
                           Continuation:PiJsonObject
    | TransitionOut     of Channel:PiJsonObject *
                           OutNames:PiJsonArray *
                           Continuation:PiJsonObject
    | TransitionInp     of Channel:PiJsonObject *
                           OutNames:PiJsonArray *
                           InpNames:PiJsonArray *
                           Continuation:PiJsonObject
    | TransitionSum     of Summation:PiJsonObject *
                           WhenPfx:PiJsonObject
    | TransitionTau
    | TransitionRep     of string
    | RemoveSummation   of Summation:PiJsonObject

type PiObservable<'T>() =
    let thisLock = new obj()
    let mutable finished = false
    let mutable key = 0
    let mutable subscriptions = Map.empty : Map<int, IObserver<'T>>

    let protect f =
        let mutable ok = false
        try 
            f()
            ok <- true
        finally
            Debug.Assert(ok, "IObserver method threw an exception.")

    let next(obs) = 
        subscriptions |> Seq.iter (fun (KeyValue(_, value)) -> 
            protect (fun () -> value.OnNext(obs)))

    let completed() = 
        subscriptions |> Seq.iter (fun (KeyValue(_, value)) -> 
            protect (fun () -> value.OnCompleted()))

    let error(err) = 
        subscriptions |> Seq.iter (fun (KeyValue(_, value)) -> 
            protect (fun () -> value.OnError(err)))

    let obs = 
        { new IObservable<'T> with
            member this.Subscribe(obs) =
                let key1 =
                    lock thisLock (fun () ->
                        let key1 = key
                        key <- key + 1
                        subscriptions <- subscriptions.Add(key1, obs)
                        key1)
                { new IDisposable with 
                    member this.Dispose() = 
                        lock thisLock (fun () -> 
                            subscriptions <- subscriptions.Remove(key1)) } }

    member this.Next(obs) =
        Debug.Assert(not finished, "IObserver is already finished")
        next obs

    member this.Completed() =
        Debug.Assert(not finished, "IObserver is already finished")
        finished <- true
        completed()
        finished <- false

    member this.Error(err) =
        Debug.Assert(not finished, "IObserver is already finished")
        finished <- true
        error err

    member this.AsObservable = obs
