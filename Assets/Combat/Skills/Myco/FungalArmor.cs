using System.Linq;

public class FungalArmor : ISkill, ISkillWithRange
{
    public string Description => "Nearby mushrooms grant you armor made out of their spores.";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };

    public int Range { get; set; } = 15;

    public SkillGroup SkillGroup => SkillGroup.MYCOMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var nearby = combatState.CombatActors.Values.Where(actor => actor.Alignment == user.Alignment && actor is Mushroom).Where(actor => (actor.Position - user.Position).sqrMagnitude < Range * Range).ToArray();
        combatState.ApplyStatus(user, new SporeArmor(nearby.Length + 1));
    }
}
