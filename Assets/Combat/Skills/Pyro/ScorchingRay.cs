using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class ScorchingRay : ISkill, ISkillWithRadius, ISkillWithDamage, ISkillWithRange
{
    public string Description => "Hit your enemies with a powerful ray of fire.";
    public ClampedInt Cooldown { get; set; } = new(0, 5, 0);
    public int APCost { get; set; } = 3;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, true, Range)
    };

    public int Radius { get; set; } = 2;
    public int Damage { get; set; } = 7;
    public int Range { get; set; } = 15;

    public SkillGroup SkillGroup => SkillGroup.PYROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPos = (Vector2Int)parameters[0];
        targetPos = Vector2Int.RoundToInt(((Vector2)(targetPos - user.Position)).normalized * Range);
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
            foreach (var tile in area)
                combatState.SetTileModifier(tile, new Fire());
            foreach (var enemy in actorsInArea)
            {
                hashset.Add(enemy.Guid);
                var result = combatState.DealDamage(user, enemy, DamageSources.FIRE.WithDamageAmount(Damage));
                if (result.armorBroken) combatState.ApplyStatus(enemy, new Burning());
            }
        }
    }
}
