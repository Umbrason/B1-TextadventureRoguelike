using UnityEngine;

public class Petrified : IStatusEffect
{
    public int Duration { get; set; } = 2;
    public void OnApply(ICombatActor actor, CombatState state)
    {
        actor.StunSources++;
    }
    public void OnRemove(ICombatActor actor, CombatState state)
    {
        actor.StunSources--;
    }
    public DamageInfo OnBeforeRecieveDamage(ICombatActor actor, DamageInfo attackInfo) => attackInfo.WithDamageAmount(Mathf.CeilToInt(attackInfo.amount / 1.5f));
}

