public class Tailwind : ISkill
{
    public string Description => "Call upon favourable winds to speed up your movement.";
    public ClampedInt Cooldown { get; set; } = new(0, 5, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };

    public SkillGroup SkillGroup => SkillGroup.AEROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        combatState.ApplyStatus(user, new FavourableWind());
    }
}
