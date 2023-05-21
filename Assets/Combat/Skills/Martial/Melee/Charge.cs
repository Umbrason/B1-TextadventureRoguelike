using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Charge : ISkill, ISkillWithRange, ISkillWithDamage, ISkillWithRadius
{
    public string Description => "Charge towards target location, damaging enemies in your path. 'LEEROOOOY JEEEEENKINS'";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, true, Range)
    };

    public int Radius { get; set; } = 3;
    public int Damage { get; set; } = 4;
    public int Range { get; set; } = 10;

    public SkillGroup SkillGroup => SkillGroup.MELEE;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPos = (Vector2Int)parameters[0];
        var gridline = Shapes.GridLine(user.Position, targetPos);
        var hashset = new HashSet<Guid>();
        foreach (var pos in gridline)
        {
            var area = Shapes.GridCircle(pos, Radius);
            var actorsInArea = area.Where(p => combatState.ActorPositions.ContainsKey(p))
                            .Select(p => combatState.CombatActors[combatState.ActorPositions[p]])
                            .Where(actor => !hashset.Contains(actor.Guid))
                            .Where(actor => actor.Alignment != user.Alignment)
                            .ToArray();
            foreach (var enemy in actorsInArea)
            {
                hashset.Add(enemy.Guid);
                combatState.DealDamage(user, enemy, DamageSources.PHYSICAL.WithDamageAmount(Damage));
            }
        }
        combatState.TeleportActor(user, targetPos);
    }
}
