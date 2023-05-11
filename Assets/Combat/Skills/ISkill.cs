
using System;

public interface ISkill : IBinarySerializable
{
    public int Cooldown { get; set; }
    public ITargetSelector[] TargetSelectors { get; }
    public interface ITargetSelector
    {
        public void OnBeginSelection(Action<object> NotifySelectionDone);
    }
    public bool CanUse(CombatState combatState, ICombatActor user);
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters);
}