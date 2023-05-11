using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ConsoleTextInput))]
public class PlayerCombatActorController : MonoBehaviour
{
    private ConsoleTextInput cached_ConsoleTextInput;
    private ConsoleTextInput ConsoleTextInput => cached_ConsoleTextInput ??= GetComponent<ConsoleTextInput>();

    void Awake()
    {
        ConsoleTextInput.OnSubmitLine += ParseCommand;
    }

    private PlayableCombatActor activeCombatActor;

    private void ParseCommand(string command)
    {
        if (activeCombatActor == null) return;
        var combatLog = CombatManager.Instance.CombatLog;
        var keywords = command.Split(' ');
        var op = keywords[0];
        switch (op)
        {
            case "help":
                ConsoleOutput.Println(@"
                HELP - OPENS THIS MENU
                
                
                
                ");
                break;

            case "move":
            case "mv":
                var args = keywords.Skip(1).ToArray();
                int.TryParse(args[0], out int x);
                int.TryParse(args[1], out int y);
                combatLog.MoveActor(activeCombatActor.Guid, new(x, y));
                break;

            case "cast":
            case "c":
                break;
        }

    }
}
