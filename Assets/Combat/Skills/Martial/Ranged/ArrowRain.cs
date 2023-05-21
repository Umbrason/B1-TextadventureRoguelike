using System.Linq;
using UnityEngine;

public class ArrowRain : ISkill, ISkillWithRange, ISkillWithRadius, ISkillWithDamage
{
    public string Description => "Rain down arrows on everyone in a target area.";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };

    public int Range { get; set; } = 30;
    public int Radius { get; set; } = 5;

    public SkillGroup SkillGroup => SkillGroup.RANGED;
    
    public int Damage { get; set; } = 5;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var center = (Vector2Int)parameters[0];
        var circle = Shapes.GridCircle(center, Radius);
        var actorsInArea = circle.Where(p => combatState.ActorPositions.ContainsKey(p)).Select(p => combatState.CombatActors[combatState.ActorPositions[p]]).ToArray();
        foreach(var actor in actorsInArea)
        {
            combatState.DealDamage(user, actor, DamageSources.PHYSICAL.WithDamageAmount(Damage));
        }
    }
}
