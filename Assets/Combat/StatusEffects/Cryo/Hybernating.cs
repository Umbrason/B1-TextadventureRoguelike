public class Hybernating : IStatusEffect
{
    public int Duration { get; set; } = 1;

    public void OnApply(ICombatActor actor, CombatState state) { actor.StunSources++; }
    public void OnRemove(ICombatActor actor, CombatState state)
    {
        actor.Health.Value = actor.Health.Max;
        foreach (var status in actor.StatusEffects)
            state.RemoveStatus(actor, status);
        actor.StunSources--;
    }
    public DamageInfo OnBeforeRecieveDamage(ICombatActor actor, DamageInfo attackInfo) => attackInfo.WithDamageAmount(0);

}

