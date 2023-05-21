using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Winterblast : ISkill, ISkillWithRange, ISkillWithRadius, ISkillWithDamage
{
    public string Description => "Blast your foes with a cold burst of ice.";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 3;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };

    public int Damage { get ; set ; } = 6;
    public int Radius { get ; set ; } = 5;
    public int Range { get ; set ; } = 20;
    public SkillGroup SkillGroup => SkillGroup.CRYOMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
        var area = Shapes.GridCircle(targetPosition, Radius);
        var actorsInArea = area.Where(p => combatState.ActorPositions.ContainsKey(p)).Select(p => combatState.CombatActors[combatState.ActorPositions[p]]).ToArray();
        foreach (var tile in area)
        {
            combatState.SetTileModifier(tile, new Ice());
        }
        foreach (var target in actorsInArea)
        {
            var result = combatState.DealDamage(user, target, DamageSources.ICE.WithDamageAmount(Damage));
            if (result.armorBroken) combatState.ApplyStatus(target, new Frozen());
        }
    }
}
