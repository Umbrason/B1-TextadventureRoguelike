using System;
using System.Collections.Generic;
using System.Linq;

public class CombatLoop
{
    private IReadOnlyDictionary<Guid, ICombatActor> combatActors => CombatLog.CurrentState.CombatActors;
    private readonly Queue<ICombatActor> turnQueue = new();

    private CombatLog CombatLog;


    public CombatLoop(CombatLog log)
    {
        this.CombatLog = log;
        NextTurn();
    }

    private ICombatActor activeActor;
    private void EndTurn()
    {
        activeActor = null;
        NextTurn();
    }

    private void BuildTurnQueue()
    {
        var teamDict = new SortedList<int, SortedList<int, ICombatActor>>();
        foreach (var actor in combatActors.Values)
        {
            var bucket = teamDict.ContainsKey(actor.Alignment) ? teamDict[actor.Alignment] : teamDict[actor.Alignment] = new();
            bucket.Add(int.MaxValue - actor.Initiative, actor);
        }
        var alignments = teamDict.Keys.OrderBy(alignment => teamDict[alignment].Keys[0]).ToArray();
        int teamIndex = 0;
        while (turnQueue.Count < combatActors.Count)
        {
            var bucket = teamDict[alignments[teamIndex]];
            if (bucket.Count > 0)
            {
                turnQueue.Enqueue(bucket.Values[bucket.Count - 1]);
                bucket.RemoveAt(bucket.Count - 1);
            }
            teamIndex = ++teamIndex % alignments.Length;
        }
    }

    private void NextTurn()
    {
        if (turnQueue.Count == 0) BuildTurnQueue();
        if (turnQueue.Count == 0) return;
        activeActor = turnQueue.Dequeue();
        var currentActiveActor = activeActor;
        activeActor.NotifyTurnStart(new(() => { if (this.activeActor == currentActiveActor) EndTurn(); }, CombatLog));
    }
}
