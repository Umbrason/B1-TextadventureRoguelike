using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ConsoleTextInput))]
public class PlayerCombatActorController : MonoBehaviour
{
    private ConsoleTextInput cached_ConsoleTextInput;
    private ConsoleTextInput ConsoleTextInput => cached_ConsoleTextInput ??= GetComponent<ConsoleTextInput>();

    void Start()
    {
        ConsoleTextInput.OnSubmitLine += ParseCommand;
        PlayableCombatActor.OnCharacterBeginTurn += (value) => activeCombatActor = value;
    }

    private PlayableCombatActor activeCombatActor;

    private void ParseCommand(string command)
    {
        if(activeCombatActor == null) return;
        var combatLog = CombatManager.Instance.CombatLog;
        var keywords = command.Split(' ');
        var op = keywords[0];
        switch (op)
        {
            case "move":
            case "mv":
                var args = keywords.Skip(1).ToArray();
                int.TryParse(args[0], out int x);
                int.TryParse(args[1], out int y);
                var valid = combatLog.TryPerformAction(new MoveCombatAction(activeCombatActor.Guid, new(x, y)));
                if(valid) Debug.Log($"moving to {x},{y}");
                else Debug.Log("move not valid");
                break;
        }
    }
}
