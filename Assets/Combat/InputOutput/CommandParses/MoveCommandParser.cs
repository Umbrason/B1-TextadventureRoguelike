using System.Collections.Generic;
using UnityEngine;

public class MoveCommandParser : ICommandParser
{
    public string HelpText => @"Move to a specified location.
args: int x, y";

    private PositionTargetSelector targetSelector = new();

    ICommandParser.ParseResult ICommandParser.Parse(ref Queue<string> args, CombatLog log)
    {
        if (!targetSelector.IsDone)
        {
            var parseResult = targetSelector.ParseInput(ref args, log.CurrentReadOnlyCombatState);
            if(parseResult == ITargetSelector.TargetSelectionResult.INVALID) return ICommandParser.ParseResult.CANCEL;
        }
        if (!targetSelector.IsDone) return ICommandParser.ParseResult.INCOMPLETE;
        log.MoveActor(log.CurrentReadOnlyCombatState.ActiveActor.Guid, (Vector2Int)targetSelector.Value);
        return ICommandParser.ParseResult.FINISHED;
    }
}