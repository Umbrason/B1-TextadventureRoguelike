using System.Linq;

public class TriggerMushrooms : ISkill, ISkillWithRange, ISkillWithDamage, ISkillWithRadius
{
    public string Description => "Trigger all nearby mushroom to release spores";
    public ClampedInt Cooldown { get; set; } = new(0, 2, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };

    public SkillGroup SkillGroup => SkillGroup.MYCOMANCY;

    public int Range { get; set; } = 20;
    public int Radius { get; set; } = 4;
    public int Damage { get; set; } = 4;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var nearby = combatState.CombatActors.Values.Where(actor => actor.Alignment == user.Alignment && actor is Mushroom).Where(actor => (actor.Position - user.Position).sqrMagnitude < Range * Range).ToArray();
        foreach (var mushroom in nearby)
        {
            var targetPosition = mushroom.Position;
            var area = Shapes.GridCircle(targetPosition, Radius);
            var actorsInArea = area.Where(p => combatState.ActorPositions.ContainsKey(p))
                                .Select(p => combatState.CombatActors[combatState.ActorPositions[p]])
                                .Where(a => a.Alignment != user.Alignment)
                                .ToArray();
            var element = (mushroom.StatusEffects.FirstOrDefault(s => s is ElementalSpores) as ElementalSpores)?.element ?? Element.FUNGAL;
            foreach (var target in actorsInArea) combatState.DealDamage(user, target, DamageSources.FUNGAL.WithElement(element).WithDamageAmount(Damage));
        }
    }
}
