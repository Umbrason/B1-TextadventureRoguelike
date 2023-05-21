public class Stunned : IStatusEffect
{
    public int Duration { get; set; } = 1;

    public void OnApply(ICombatActor actor, CombatState state) { actor.StunSources++; }
    public void OnRemove(ICombatActor actor, CombatState state) { actor.StunSources--; }
}

