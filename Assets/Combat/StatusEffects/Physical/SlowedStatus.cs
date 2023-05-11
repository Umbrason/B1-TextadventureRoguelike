public class SlowedStatus : IStatusEffect
{
    public int Duration { get; set; } = 2;
    public void OnApply(ICombatActor actor, CombatLog log) { }

    public void OnBeginTurn(ICombatActor actor, CombatLog log)
    {
        if (actor.MovementPoints % 2 == 1) actor.MovementPoints += 1;
        actor.MovementPoints /= 2;
    }

    public void OnMove(ICombatActor actor, CombatLog log) { }

    public void OnRemove(ICombatActor actor, CombatLog log) { }

    public void OnUseSkill(ICombatActor actor, CombatLog log) { }
}
