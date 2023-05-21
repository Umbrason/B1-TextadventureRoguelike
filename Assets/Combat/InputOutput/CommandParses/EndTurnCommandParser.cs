using System.Collections.Generic;
using UnityEngine;

public class EndTurnCommandParser : ICommandParser
{
    public string HelpText => @"Ends your turn.";    

    ICommandParser.ParseResult ICommandParser.Parse(ref Queue<string> args, CombatLog log)
    {        
        log.EndTurn();
        return ICommandParser.ParseResult.FINISHED;
    }
}