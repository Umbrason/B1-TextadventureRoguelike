public class EagleEye : ISkill
{
    public string Description => "Increases the range of other ranged skills used this turn";
    public ClampedInt Cooldown { get; set; } = new(0, 2, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new PositionTargetSelector()
    };

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {

    }
}
