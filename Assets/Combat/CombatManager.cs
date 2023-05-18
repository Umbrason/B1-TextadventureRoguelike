
using System;

public static class CombatManager
{
    public static CombatLog CombatLog { get; private set; }
    public static event Action<IReadOnlyCombatState> OnCombatStateChanged;
    public static event Action<IReadOnlyCombatState> OnVisualUpdate;
    
    public static void StartCombat(CombatState state)
    {
        CombatLog = new CombatLog(state);
        CombatLog.OnStateChanged += (state) => OnCombatStateChanged?.Invoke(state);
        CombatLog.OnVisualUpdate += (state) => OnVisualUpdate?.Invoke(state);

        CombatLog.BuildTurnQueue();
        CombatLog.BeginNextTurn();
    }
}