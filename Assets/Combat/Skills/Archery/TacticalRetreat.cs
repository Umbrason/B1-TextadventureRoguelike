public class TacticalRetreat : ISkill, ISkillWithRange
{
    public string Description => "Quickly jump to a more favourable position";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };
    public int Range { get; set; }

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {

    }
}
