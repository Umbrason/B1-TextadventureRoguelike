public class ArmorUp : ISkill
{
    public string Description => "Brace yourself for incoming hits.";
    public ClampedInt Cooldown { get; set; } = new(0, 2, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };
    public SkillGroup SkillGroup => SkillGroup.MELEE;
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters) => combatState.ApplyStatus(user, new ArmoredUp());
}
