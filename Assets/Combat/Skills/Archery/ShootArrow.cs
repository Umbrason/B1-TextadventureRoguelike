public class ShootArrow : ISkill, ISkillWithRange
{
    public string Description => "Shoots an arrow at a target in sight";
    public ClampedInt Cooldown { get; set; } = new(0, 0, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };
    public int Range { get; set; }

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {

    }
}
