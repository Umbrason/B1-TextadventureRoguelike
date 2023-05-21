public class Bleeding : IStatusEffect
{
    public int Duration { get; set; } = 2;    
    bool IReadOnlyStatusEffect.IsStackable => true;
    public void OnBeginTurn(ICombatActor actor, CombatState state)
    {
        state.DealDamage(null, actor, DamageSources.BLEED);
    }
}
