using System;

public class ProneStatus : IStatusEffect
{
    public int Duration { get; set; }

    public void OnApply(ICombatActor actor, CombatLog log)
    {

    }
    public void OnBeginTurn(ICombatActor actor, CombatLog log)
    {
        
    }
    public void OnMove(ICombatActor actor, CombatLog log) { }
    public void OnRemove(ICombatActor actor, CombatLog log) { }
    public void OnUseSkill(ICombatActor actor, CombatLog log) { }
}