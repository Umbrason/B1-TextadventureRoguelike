public class FavourableWind : IStatusEffect
{
    public int Duration { get; set; } = 3;

    private const int MPBoost = 5;

    public void OnApply(ICombatActor actor, CombatState state)
    {
        actor.MovementPoints.Max += MPBoost;
        actor.MovementPoints.Value += MPBoost;

    }
    public void OnRemove(ICombatActor actor, CombatState state)
    {
        actor.MovementPoints.Value -= MPBoost;
        actor.MovementPoints.Max -= MPBoost;
    }
}

