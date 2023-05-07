public interface ICombatAction
{
    void Apply(ref CombatState state);
    bool IsValid(CombatState state);
}