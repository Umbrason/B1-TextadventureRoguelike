public class CrippledStatus : IStatusEffect
{
    public int Duration { get; set; } = 1;

    public void OnApply(ICombatActor actor, CombatLog log) { }
    public void OnBeginTurn(ICombatActor actor, CombatLog log)
    {
        actor.MovementPoints.Value = 0;
    }
    public void OnMove(ICombatActor actor, CombatLog log) { }
    public void OnRemove(ICombatActor actor, CombatLog log) { }
    public void OnUseSkill(ICombatActor actor, CombatLog log) { }
}
