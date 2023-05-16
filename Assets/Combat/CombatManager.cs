
using System;

public class CombatManager : SingletonBehaviour<CombatManager>
{
    public CombatLog CombatLog { get; private set; }
    public event Action<IReadOnlyCombatState> OnCombatStateChanged;
    public event Action<IReadOnlyCombatState> OnVisualUpdate;
    
    public void StartCombat(CombatState state)
    {
        this.CombatLog = new CombatLog(state);
        CombatLog.OnStateChanged += (state) => this.OnCombatStateChanged?.Invoke(state);
        CombatLog.OnVisualUpdate += (state) => this.OnVisualUpdate?.Invoke(state);

        CombatLog.BuildTurnQueue();
        CombatLog.BeginNextTurn();
    }
}