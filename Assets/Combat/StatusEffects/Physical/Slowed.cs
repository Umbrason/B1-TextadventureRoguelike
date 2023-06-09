public class Slowed : IStatusEffect
{
    public int Duration { get; set; } = 2;
    public void OnApply(ICombatActor actor, CombatState state)
    {
        if (actor.MovementPoints % 2 == 1) actor.MovementPoints += 1;
        actor.MovementPoints /= 2;
    }
    public void OnBeginTurn(ICombatActor actor, CombatState state)
    {
        if (actor.MovementPoints % 2 == 1) actor.MovementPoints += 1;
        actor.MovementPoints /= 2;
    }
}
