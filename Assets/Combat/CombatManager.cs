
using System;
using System.Linq;

public static class CombatManager
{
    public static CombatLog CombatLog { get; private set; }
    public static event Action<IReadOnlyCombatState> OnCombatStateChanged;
    public static event Action<IReadOnlyCombatState> OnVisualUpdate;

    public static void StartCombat(CombatState state)
    {
        CombatLog = new CombatLog(state);
        CombatLog.OnStateChanged += CheckEnd;
        CombatLog.OnStateChanged += ForwardStateChanged;
        CombatLog.OnVisualUpdate += ForwardVisualUpdate;
        CombatLog.BuildTurnQueue();
        CombatLog.BeginNextTurn();
    }

    private static void ForwardStateChanged(IReadOnlyCombatState state) => OnCombatStateChanged?.Invoke(state);
    private static void ForwardVisualUpdate(IReadOnlyCombatState state) => OnVisualUpdate?.Invoke(state);
    private static void CheckEnd(IReadOnlyCombatState state)
    {
        var leaderGuid = RunManager.ReadOnlyRunInfo.ReadOnlyPartyInfo.PartyLeader.Guid;
        var leaderAlive = state.CombatActors.TryGetValue(leaderGuid, out var leader);
        var combatStillGoing = leaderAlive && state.CombatActors.Any(actor => actor.Value.Alignment != leader.Alignment);
        if (combatStillGoing) return;
        RunManager.EndCombat(leaderAlive);
        CombatLog.OnStateChanged -= ForwardStateChanged;
        CombatLog.OnVisualUpdate -= ForwardVisualUpdate;
        CombatLog = null;
    }
}