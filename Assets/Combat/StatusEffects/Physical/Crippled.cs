public class Crippled : IStatusEffect
{
    public int Duration { get; set; } = 1;

    public void OnApply(ICombatActor actor, CombatState state)
    {
        actor.MovementPoints.Value = 0;
    }
    public void OnBeginTurn(ICombatActor actor, CombatState state)
    {
        actor.MovementPoints.Value = 0;
    }
    public void OnMove(ICombatActor actor, CombatState state) { }
    public void OnRemove(ICombatActor actor, CombatState state) { }
    public void OnUseSkill(ICombatActor actor, CombatState state) { }
}
