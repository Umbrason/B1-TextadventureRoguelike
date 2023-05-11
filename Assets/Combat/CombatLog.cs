using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatLog : IReadOnlyCombatLog
{
    private readonly Stack<CombatState> stateStack = new();
    public CombatState CurrentState => stateStack.Peek().DeepCopy();
    public IEnumerable<CombatState> CombatStateHistory => stateStack;
    public event Action<CombatState> OnStateChanged;

    public CombatLog(CombatState initialState)
    {
        stateStack.Push(initialState);
    }

    public bool TryPerformAction(ICombatAction action)
    {
        if (!action.IsValid(CurrentState)) return false;
        action.Apply(CurrentState, this);
        return true;
    }

    public void MoveActor(Guid actorGuid, Vector2Int targetLocation)
    {
        var state = CurrentState;
        var Pathfinder = new Pathfinder(state.IsTileWalkable);
        var actor = state.CombatActors[actorGuid];
        var budget = actor.MovementPoints;
        var path = Pathfinder.FromTo(actor.Position, targetLocation);

        if (path.Length == 0) ConsoleOutput.Println("Cannot reach destination!");
        if (path.Length > budget) ConsoleOutput.Println("Destination is too far!");
        if (path.Length == 0 || path.Length > budget) return;

        if (state.TileModifiers.TryGetValue(targetLocation, out var modifier)) modifier.OnExit(actor, this);
        actor.Position = targetLocation;
        foreach (var statuseffect in actor.StatusEffects) statuseffect.OnMove(actor, this);
        if (state.TileModifiers.TryGetValue(targetLocation, out modifier)) modifier.OnEnter(actor, this);

        for (int i = 0; i < path.Length; i++)
            SubmitState(state.MoveActor(actorGuid, path[i]));
    }

    public void BeginNextTurn()
    {
        var nextGuid = CurrentState.TurnQueue.Dequeue();
        var actor = CurrentState.ActiveActor;
        var statusUpdateQueue = new Queue<IStatusEffect>(actor.StatusEffects);
        while (statusUpdateQueue.Count > 0)
        {
            var status = statusUpdateQueue.Dequeue();
            status.OnBeginTurn(actor, this);
        }
    }

    public void EndTurn()
    {
        var actor = CurrentState.ActiveActor;
        var statusUpdateQueue = new Queue<IStatusEffect>(actor.StatusEffects);
        while (statusUpdateQueue.Count > 0)
        {
            var status = statusUpdateQueue.Dequeue();
            status.Duration--;
            if (status.Duration == 0)
            {
                status.OnRemove(actor, this);
                actor.StatusEffects.Remove(status);
                SubmitState(CurrentState.WithModifiedActor(actor));
            }
        }
        SubmitState(CurrentState.WithActiveActor(null));
    }

    public void SubmitState(CombatState state)
    {
        stateStack.Push(state.DeepCopy());
        OnStateChanged?.Invoke(CurrentState);
    }

    private void BuildTurnQueue()
    {
        var state = CurrentState;
        var turnQueue = new Queue<Guid>();
        var teamDict = new SortedList<int, SortedList<int, ICombatActor>>();
        var actors = state.CombatActors;
        foreach (var actor in actors.Values)
        {
            var bucket = teamDict.ContainsKey(actor.Alignment) ? teamDict[actor.Alignment] : teamDict[actor.Alignment] = new();
            bucket.Add(int.MaxValue - actor.Initiative, actor);
        }
        var alignments = teamDict.Keys.OrderBy(alignment => teamDict[alignment].Keys[0]).ToArray();
        int teamIndex = 0;
        while (turnQueue.Count < actors.Count)
        {
            var bucket = teamDict[alignments[teamIndex]];
            if (bucket.Count > 0)
            {
                turnQueue.Enqueue(bucket.Values[bucket.Count - 1].Guid);
                bucket.RemoveAt(bucket.Count - 1);
            }
            teamIndex = ++teamIndex % alignments.Length;
        }
        SubmitState(state.WithTurnQueue(turnQueue));
    }
}
public interface IReadOnlyCombatLog
{
    public IEnumerable<CombatState> CombatStateHistory { get; }
    CombatState CurrentState { get; }
}