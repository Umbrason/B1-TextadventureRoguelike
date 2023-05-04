using System.Collections.Generic;

public class CombatLoop
{
    private List<ICombatActor> combatActors;
    private Queue<ICombatActor> turnQueue;

    private CombatLog log;


    private ICombatActor activeActor;
    private void EndTurn()
    {
        activeActor = null;
        NextTurn();
    }

    private void NextTurn()
    {
        activeActor = turnQueue.Dequeue();
        var currentActiveActor = activeActor;
        activeActor.NotifyTurnStart(new(() => { if (this.activeActor == currentActiveActor) EndTurn(); }, log));
    }
}
