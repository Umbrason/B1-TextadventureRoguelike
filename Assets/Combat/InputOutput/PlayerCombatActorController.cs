using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCombatActorController : MonoBehaviour
{    
    void Awake()
    {
        ConsoleTextInput.OnSubmitLine += ParseInput;
    }

    void OnDestroy()
    {
        ConsoleTextInput.OnSubmitLine -= ParseInput;
    }

    private class OPCode
    {
        public string[] Aliases { get; }
        private Type parserType;
        public OPCode(Type type, params string[] aliases)
        {
            parserType = type;
            this.Aliases = aliases;
        }
        public ICommandParser CommandParser => (ICommandParser)Activator.CreateInstance(parserType);
    }

    private List<OPCode> opcodes = new()
    {
        new OPCode(typeof(MoveCommandParser), "move", "mv"),
        new OPCode(typeof(CastCommandParser), "cast", "skill"),
    };

    private ICommandParser parser;
    private void ParseInput(string input)
    {
        var combatLog = CombatManager.CombatLog;
        var activeActor = combatLog.CurrentReadOnlyCombatState.ActiveActor;
        if (activeActor == null || activeActor.Alignment != 0) return;
        var keywords = new Queue<string>(input.Split(' '));
        if(keywords.Count == 0 || keywords.Peek().Length == 0) return;
        
        if (parser == null)
        {
            var op = keywords.Dequeue();
            var opCodeInfo = opcodes.FirstOrDefault(opcode => opcode.Aliases.Contains(op.ToLower()));
            if (opCodeInfo == null)
            {
                ConsoleOutput.Println($"No such command '{op}'");
                return;
            }
            parser = opCodeInfo.CommandParser;
        }
        var result = parser.Parse(ref keywords, combatLog);
        if(result == ICommandParser.ParseResult.CANCEL) ConsoleOutput.Println($"Canceled");
        parser = result == ICommandParser.ParseResult.INCOMPLETE ? parser : null;
        return;
    }
}
