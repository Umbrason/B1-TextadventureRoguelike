using System.Linq;
using UnityEngine;

public class HurlBoulders : ISkill, ISkillWithRadius, ISkillWithRange, ISkillWithMultitarget, ISkillWithDamage
{
    public string Description => "Hurl multiple boulders at your foes. Can cripple unarmored foes";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors =>
    Enumerable.Range(0, TargetCount).Select(
        _ => new RangedPositionSelector(true, false, Range)
    ).ToArray();

    public int Range { get; set; } = 15;
    public int Radius { get; set; } = 2;
    public int TargetCount { get; set; } = 3;
    public int Damage { get; set; } = 2;

    public SkillGroup SkillGroup => SkillGroup.TERRAMANCY;


    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPositions = parameters.Select(p => (Vector2Int)p);
        foreach (var targetPosition in targetPositions)
        {
            var area = Shapes.GridCircle(targetPosition, Radius);
            var actorsInArea = area.Where(p => combatState.ActorPositions.ContainsKey(p)).Select(p => combatState.CombatActors[combatState.ActorPositions[p]]).ToArray();
            foreach (var target in actorsInArea)
            {
                var result = combatState.DealDamage(user, target, DamageSources.EARTH.WithDamageAmount(Damage));
                if (result.armorBroken) combatState.ApplyStatus(target, new Crippled());
            }
        }
    }
}
