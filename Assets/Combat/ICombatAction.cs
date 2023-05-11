using System;

public interface ICombatAction
{
    void Apply(CombatState state, CombatLog log);
    bool IsValid(CombatState state);
}