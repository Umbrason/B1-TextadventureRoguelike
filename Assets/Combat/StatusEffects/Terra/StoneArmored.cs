using UnityEngine;

public class StoneArmored : IStatusEffect
{
    public int Duration { get; set; } = 3;
    const int ArmorAmount = 5;
    public void OnApply(ICombatActor actor, CombatState state)
    {
        actor.Armor.Max += ArmorAmount;
        actor.Armor.Value += ArmorAmount;
    }
    public void OnRemove(ICombatActor actor, CombatState state)
    {
        actor.Armor.Value -= ArmorAmount;
        actor.Armor.Max -= ArmorAmount;
    }
    public DamageInfo OnBeforeRecieveDamage(ICombatActor actor, DamageInfo attackInfo) => attackInfo.WithDamageAmount(Mathf.CeilToInt(attackInfo.amount / 2f));
}

