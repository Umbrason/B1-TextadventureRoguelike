public class Burning : IStatusEffect
{
    public int Duration { get; set; } = 2;

    public void OnApply(ICombatActor actor, CombatState state) { }
    public void OnBeginTurn(ICombatActor actor, CombatState state)
    {
        actor.Health -= Balancing.FIRE_DAMAGE;
    }
    public void OnMove(ICombatActor actor, CombatState state) { }
    public void OnRemove(ICombatActor actor, CombatState state) { }
    public void OnUseSkill(ICombatActor actor, CombatState state) { }
}

