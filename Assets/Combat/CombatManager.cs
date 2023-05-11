
using System;

public class CombatManager : SingletonBehaviour<CombatManager>
{
    public CombatLog CombatLog { get; private set; }
    public event Action<CombatState> OnCombatStateChanged;

    public void StartCombat(CombatState state)
    {
        this.CombatLog = new CombatLog(state);
        CombatLog.OnStateChanged += OnCombatStateChanged;
        CombatLog.BeginNextTurn();
        OnCombatStateChanged.Invoke(CombatLog.CurrentState);
    }
}
