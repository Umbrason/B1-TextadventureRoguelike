public class Cryostasis : ISkill
{
    public string Description => "Encase yourself with ice, becoming immune for one turn and fully healing";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };

    public SkillGroup SkillGroup => SkillGroup.CRYOMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        combatState.ApplyStatus(user, new Hybernating());
    }
}
