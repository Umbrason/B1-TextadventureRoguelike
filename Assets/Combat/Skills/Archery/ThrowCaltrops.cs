public class ThrowCaltrops : ISkill, ISkillWithRange
{
    public string Description => "Throw some caltrops to stop enemies from approaching";
    public ClampedInt Cooldown { get; set; } = new(0, 5, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };
    public int Range { get; set; }
    
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {

    }
}
