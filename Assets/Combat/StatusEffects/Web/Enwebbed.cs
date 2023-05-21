public class Enwebbed : IStatusEffect
{
    public int Duration { get; set; } = 2;    

    public void OnApply(ICombatActor actor, CombatState state)
    {
        actor.MovementPoints.Value = 0;
    }
    public void OnBeginTurn(ICombatActor actor, CombatState state)
    {
        actor.MovementPoints.Value = 0;
    }
}

