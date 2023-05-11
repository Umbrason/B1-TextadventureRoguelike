using System;

public interface IStatusEffect
{
    int Duration { get; set; }
    void OnApply(ICombatActor actor, CombatLog log);
    void OnRemove(ICombatActor actor, CombatLog log);
    void OnBeginTurn(ICombatActor actor, CombatLog log);
    void OnMove(ICombatActor actor, CombatLog log);
    void OnUseSkill(ICombatActor actor, CombatLog log);
}
