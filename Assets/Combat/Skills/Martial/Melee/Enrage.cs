public class Enrage : ISkill
{
    public string Description => "Feel the rage coursing through your body!";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };
    public SkillGroup SkillGroup => SkillGroup.MELEE;
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters) => combatState.ApplyStatus(user, new Enraged());
}
