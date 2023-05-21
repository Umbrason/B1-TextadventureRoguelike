using System.Linq;
using UnityEngine;

public class SporeCloud : ISkill, ISkillWithRange, ISkillWithRadius, ISkillWithDamage
{
    public string Description => "Throw a mushroom at your enemies. Infect enemies within its cloud of spores!";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };

    public int Range { get; set; } = 20;
    public int Radius { get; set; } = 10;
    public int Damage { get; set; } = 5;

    public SkillGroup SkillGroup => SkillGroup.MYCOMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
        var area = Shapes.GridCircle(targetPosition, Radius);
        var actorsInArea = area.Where(p => combatState.ActorPositions.ContainsKey(p))
        .Select(p => combatState.CombatActors[combatState.ActorPositions[p]])
        .Where(p => p.Alignment == user.Alignment)
        .ToArray();
        foreach (var target in actorsInArea)
        {
            var result = combatState.DealDamage(user, target, DamageSources.FUNGAL.WithDamageAmount(Damage));
            if (result.armorBroken) combatState.ApplyStatus(target, new SporeInfected(user.Alignment));
        }
    }
}
