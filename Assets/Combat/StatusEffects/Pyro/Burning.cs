public class Burning : IStatusEffect
{
    public int Duration { get; set; } = 2;
    public void OnBeginTurn(ICombatActor actor, CombatState state)
    {
        state.DealDamage(null, actor, DamageSources.FIRE);
    }
}

