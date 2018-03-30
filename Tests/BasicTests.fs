namespace Generated_BasicTests

open System

open JsonPi
open JsonPi.Data
open JsonPi.PiRuntime

open PiTest

type BasicTests () =
    let TestInaction_Observations = [
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestComposeInaction_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestSimpleComm_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestMultiComm_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="w";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="w";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="y";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="w";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="y";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="w";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="v";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="w";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="a";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="w";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="a";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="u";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestSummationComposition_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="c";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="c";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="d";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="d";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestCommWithSimpleMatch_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="c";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="c";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestCommWithMatch_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="c";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="c";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="b";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestLeftSumOut_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};])
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestRightSumOut_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};])
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestLeftSumInp_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestRightSumInp_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestSumMatchSum_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};])
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="c";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="c";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};]:>obj|];};])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestLeftDefaultSumOut_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Unobservable";};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};])
            PiTraceEvent.TransitionTau
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestRightDefaultSumOut_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Unobservable";};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};])
            PiTraceEvent.TransitionTau
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestLeftDefaultSumInp_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Unobservable";};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};])
            PiTraceEvent.TransitionTau
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestRightDefaultSumInp_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Unobservable";};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};])
            PiTraceEvent.TransitionTau
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestDefaultSumMatchDefaultSum_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Unobservable";};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Input";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};])
            PiTraceEvent.TransitionTau
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionSum([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};],[{Label="Type";Value="Prefix.Unobservable";};])
            PiTraceEvent.RemoveSummation([{Label="Type";Value="Summation.Sum";};{Label="Summations";Value=[|[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="a";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};] :> obj;[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Unobservable";};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Prefix";};{Label="Prefix";Value=[{Label="Type";Value="Prefix.Output";};{Label="Channel";Value=[{Label="Type";Value="PiName";};{Label="Id";Value="b";};];};{Label="Params";Value=[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|];};];};{Label="Process";Value=[{Label="Type";Value="Process.Summation";};{Label="Summation";Value=[{Label="Type";Value="Summation.Inaction";};];};];};];};];};]:>obj|];};])
            PiTraceEvent.TransitionTau
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="b";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="z";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestBindingAndRef_Observations = [
            PiTraceEvent.PushProcess("Process.Binding")
            PiTraceEvent.RunProcess("Process.Binding")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.BindingRef")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.BindingRef")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestModuleAndRef_Observations = [
            PiTraceEvent.PushProcess("Process.ModuleRef")
            PiTraceEvent.RunProcess("Process.ModuleRef")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestModuleRefAndBindingRef_Observations = [
            PiTraceEvent.PushProcess("Process.ModuleRef")
            PiTraceEvent.RunProcess("Process.ModuleRef")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Binding")
            PiTraceEvent.RunProcess("Process.Binding")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.BindingRef")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.BindingRef")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestModuleRefAndBindingRef2_Observations = [
            PiTraceEvent.PushProcess("Process.ModuleRef")
            PiTraceEvent.RunProcess("Process.ModuleRef")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Binding")
            PiTraceEvent.RunProcess("Process.Binding")
            PiTraceEvent.PushProcess("Process.Binding")
            PiTraceEvent.RunProcess("Process.Binding")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.BindingRef")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.BindingRef")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="b";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="b";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestSimpleRestriction_Observations = [
            PiTraceEvent.PushProcess("Process.Restriction")
            PiTraceEvent.RunProcess("Process.Restriction")
            PiTraceEvent.PushProcess("Process.Restriction")
            PiTraceEvent.RunProcess("Process.Restriction")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a`2";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a`2";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestSimpleRestrictionWithExtension_Observations = [
            PiTraceEvent.PushProcess("Process.Restriction")
            PiTraceEvent.RunProcess("Process.Restriction")
            PiTraceEvent.PushProcess("Process.Restriction")
            PiTraceEvent.RunProcess("Process.Restriction")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a`2";};{Label="NameType";Value="MyExtension";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a`2";};{Label="NameType";Value="MyExtension";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestSimpleRestrictionWithExtensionAndJson_Observations = [
            PiTraceEvent.PushProcess("Process.Restriction")
            PiTraceEvent.RunProcess("Process.Restriction")
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="a`1";};{Label="NameType";Value="MyExtension";};{Label="Data";Value=[|true :> obj;5 :> obj;-12 :> obj;1.2093 :> obj;-93.5 :> obj;18001000 :> obj;-3.918001E-05 :> obj;[{Label="\"TestLabel\"";Value="\"TestValue\"";};]:>obj|];};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="a`1";};{Label="NameType";Value="MyExtension";};{Label="Data";Value=[|true :> obj;5 :> obj;-12 :> obj;1.2093 :> obj;-93.5 :> obj;18001000 :> obj;-3.918001E-05 :> obj;[{Label="\"TestLabel\"";Value="\"TestValue\"";};]:>obj|];};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="x";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestSimpleReplication_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Replication")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Replication")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="a";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="a";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionRep("Inaction")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="b";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="b";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionRep("Inaction")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="c";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="c";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="y";};]:>obj|])
            PiTraceEvent.TransitionRep("Inaction")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    let TestReplication_Observations = [
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Replication")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Replication")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="a";};]:>obj|])
            PiTraceEvent.TransitionRep("Continuation")
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="a";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="c";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="y";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="d";};]:>obj|])
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="y";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="d";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="b";};]:>obj|])
            PiTraceEvent.TransitionRep("Inaction")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.TransitionOut([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="a";};]:>obj|])
            PiTraceEvent.TransitionRep("Continuation")
            PiTraceEvent.TransitionInp([{Label="Type";Value="PiName";};{Label="Id";Value="x";};],[|[{Label="Type";Value="PiName";};{Label="Id";Value="a";};]:>obj|],[|[{Label="Type";Value="PiName";};{Label="Id";Value="e";};]:>obj|])
            PiTraceEvent.PushProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Composition")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.PushProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
            PiTraceEvent.RunProcess("Process.Summation")
        ]

    interface ITextExpected with
        member this.GetExpected(name:string) =
            match name with
            | "TestInaction" -> TestInaction_Observations
            | "TestComposeInaction" -> TestComposeInaction_Observations
            | "TestSimpleComm" -> TestSimpleComm_Observations
            | "TestMultiComm" -> TestMultiComm_Observations
            | "TestSummationComposition" -> TestSummationComposition_Observations
            | "TestCommWithSimpleMatch" -> TestCommWithSimpleMatch_Observations
            | "TestCommWithMatch" -> TestCommWithMatch_Observations
            | "TestLeftSumOut" -> TestLeftSumOut_Observations
            | "TestRightSumOut" -> TestRightSumOut_Observations
            | "TestLeftSumInp" -> TestLeftSumInp_Observations
            | "TestRightSumInp" -> TestRightSumInp_Observations
            | "TestSumMatchSum" -> TestSumMatchSum_Observations
            | "TestLeftDefaultSumOut" -> TestLeftDefaultSumOut_Observations
            | "TestRightDefaultSumOut" -> TestRightDefaultSumOut_Observations
            | "TestLeftDefaultSumInp" -> TestLeftDefaultSumInp_Observations
            | "TestRightDefaultSumInp" -> TestRightDefaultSumInp_Observations
            | "TestDefaultSumMatchDefaultSum" -> TestDefaultSumMatchDefaultSum_Observations
            | "TestBindingAndRef" -> TestBindingAndRef_Observations
            | "TestModuleAndRef" -> TestModuleAndRef_Observations
            | "TestModuleRefAndBindingRef" -> TestModuleRefAndBindingRef_Observations
            | "TestModuleRefAndBindingRef2" -> TestModuleRefAndBindingRef2_Observations
            | "TestSimpleRestriction" -> TestSimpleRestriction_Observations
            | "TestSimpleRestrictionWithExtension" -> TestSimpleRestrictionWithExtension_Observations
            | "TestSimpleRestrictionWithExtensionAndJson" -> TestSimpleRestrictionWithExtensionAndJson_Observations
            | "TestSimpleReplication" -> TestSimpleReplication_Observations
            | "TestReplication" -> TestReplication_Observations
            | _ -> failwith "unexpected"
