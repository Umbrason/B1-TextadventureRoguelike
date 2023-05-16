using System.Collections.Generic;

public interface ICommandParser
{
    public ParseResult Parse(ref Queue<string> args, CombatLog log);
    public string HelpText { get; }

    public enum ParseResult
    {
        FINISHED, CANCEL, INCOMPLETE
    }
}