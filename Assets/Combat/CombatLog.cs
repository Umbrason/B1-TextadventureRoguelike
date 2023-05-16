using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatLog
{
    private readonly Stack<IReadOnlyCombatState> stateStack = new();
    public IEnumerable<IReadOnlyCombatState> CombatStateHistory => stateStack;
    public event Action<IReadOnlyCombatState> OnStateChanged;
    public event Action<IReadOnlyCombatState> OnVisualUpdate;
    public IReadOnlyCombatState LatestPersistentState => stateStack.Peek();

    private CombatState CurrentState;
    public IReadOnlyCombatState CurrentReadOnlyCombatState => CurrentState;

    public CombatLog(CombatState initialState)
    {
        CurrentState = initialState;
        Persist();
    }

    public void MoveActor(IReadOnlyCombatActor actor, Vector2Int targetLocation) => MoveActor(actor.Guid, targetLocation);
    public void MoveActor(Guid actorGuid, Vector2Int targetLocation)
    {
        var Pathfinder = new Pathfinder(CurrentState.IsTileWalkable);
        var actor = CurrentState.CombatActors[actorGuid];
        var budget = actor.MovementPoints;
        var path = Pathfinder.FromTo(actor.Position, targetLocation);

        if (path.Length == 0) ConsoleOutput.Println("Cannot reach destination!");
        if (path.Length > budget) ConsoleOutput.Println("Destination is too far!");
        if (path.Length == 0 || path.Length > budget) return;

        for (int i = 0; i < path.Length; i++)
        {
            if (CurrentState.TileModifiers.TryGetValue(actor.Position, out var modifier)) modifier.OnExit(actor, CurrentState);
            CurrentState.ActorPositions.Remove(actor.Position);
            actor.Position = path[i];
            actor.MovementPoints -= 1;
            CurrentState.ActorPositions[actor.Position] = actor.Guid;
            foreach (var statuseffect in actor.StatusEffects) statuseffect.OnMove(actor, CurrentState);
            if (CurrentState.TileModifiers.TryGetValue(actor.Position, out modifier)) modifier.OnEnter(actor, CurrentState);
            ConsoleOutput.Println($"Moved to {path[i].x} {path[i].y}");
            if (CurrentState.ActiveActorStunned) { EndTurn(); return; }
            RaiseVisualUpdate();
        }
        Persist();
    }

    public void CastSkill(IReadOnlyCombatActor user, IReadOnlySkill skill, object[] targets) => CastSkill(user.Guid, skill, targets);
    public void CastSkill(Guid userGuid, IReadOnlySkill skill, ITargetSelector[] targets) => CastSkill(userGuid, skill, targets.Select(target => target.Value).ToArray());
    public void CastSkill(IReadOnlyCombatActor user, IReadOnlySkill skill, ITargetSelector[] targets) => CastSkill(user.Guid, skill, targets.Select(target => target.Value).ToArray());
    public void CastSkill(Guid userGuid, IReadOnlySkill skill, object[] targets)
    {
        var rwActor = CurrentState.CombatActors[userGuid];
        var rwSkill = rwActor.Skills.First(s => s.GetType() == skill.GetType());
        if (!rwSkill.CanUse(CurrentState, rwActor))
        {
            ConsoleOutput.Println($"Could not use {skill.GetType().Name}");
            return;
        }
        ConsoleOutput.Println($"{rwActor.Name} used {rwSkill.GetType().Name}.");
        rwSkill.Execute(CurrentState, rwActor, targets);
        rwSkill.Cooldown.Value = rwSkill.Cooldown.Max;
        Persist();
    }

    public void BeginNextTurn()
    {
        var nextGuid = CurrentState.TurnQueue.Dequeue();
        CurrentState.ActiveActorGuid = nextGuid;
        CurrentState.WithActiveActor(nextGuid);
        var actor = CurrentState.ActiveActor;
        var statusUpdateQueue = new Queue<IStatusEffect>(actor.StatusEffects);
        while (statusUpdateQueue.Count > 0)
        {
            var status = statusUpdateQueue.Dequeue();
            status.OnBeginTurn(actor, CurrentState);
        }
        if (CurrentState.ActiveActorStunned) EndTurn();
        else Persist();
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
                status.OnRemove(actor, CurrentState);
                actor.StatusEffects.Remove(status);
                CurrentState.CombatActors[actor.Guid] = actor;
            }
        }
        CurrentState.ActiveActorStunned = false;
        CurrentState.ActiveActorGuid = null;
        Persist();
    }

    private void RaiseVisualUpdate()
    {
        var copy = CurrentState.DeepCopy();
        OnVisualUpdate?.Invoke(copy);
    }

    private void Persist()
    {
        stateStack.Push(CurrentState.DeepCopy());
        OnVisualUpdate?.Invoke(LatestPersistentState);
        OnStateChanged?.Invoke(LatestPersistentState);
    }

    public void BuildTurnQueue()
    {
        var teamDict = new SortedList<int, List<ICombatActor>>();
        var actors = CurrentState.CombatActors;
        foreach (var actor in actors.Values)
        {
            var bucket = teamDict.ContainsKey(actor.Alignment) ? teamDict[actor.Alignment] : teamDict[actor.Alignment] = new();
            bucket.Add(actor);
            teamDict[actor.Alignment] = bucket.OrderByDescending(actor => actor.Initiative).ToList();
        }
        var alignments = teamDict.Keys.OrderBy(alignment => teamDict[alignment][0].Alignment).ToArray();
        int teamIndex = 0;
        while (CurrentState.TurnQueue.Count < actors.Count)
        {
            var bucket = teamDict[alignments[teamIndex]];
            if (bucket.Count > 0)
            {
                CurrentState.TurnQueue.Enqueue(bucket[bucket.Count - 1].Guid);
                bucket.RemoveAt(bucket.Count - 1);
            }
            teamIndex = ++teamIndex % alignments.Length;
        }
        Persist();
    }
}