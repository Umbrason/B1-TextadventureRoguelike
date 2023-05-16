public class Prone : IStatusEffect
{
    public int Duration { get; set; } = 1;
    public void OnApply(ICombatActor actor, CombatState state)
    {
        state.ActiveActorStunned = true;
    }
    public void OnBeginTurn(ICombatActor actor, CombatState state)
    {
        state.ActiveActorStunned = true;
    }
    public void OnMove(ICombatActor actor, CombatState state) { }
    public void OnRemove(ICombatActor actor, CombatState state) { }
    public void OnUseSkill(ICombatActor actor, CombatState state) { }
}