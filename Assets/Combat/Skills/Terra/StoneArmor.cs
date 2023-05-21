public class StoneArmor : ISkill
{
    public string Description => "Cover yourself in an armor made of stone, slighty increasing armor and halving incoming damage";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };

    public SkillGroup SkillGroup => SkillGroup.TERRAMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        combatState.ApplyStatus(user, new StoneArmored());
    }
}
