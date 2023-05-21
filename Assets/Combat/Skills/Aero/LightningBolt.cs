using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningBolt : ISkill, ISkillWithRadius, ISkillWithRange, ISkillWithDamage, ISkillWithMultitarget
{
    public string Description => "Cast a lightning bolt that forks onto nearby enemies";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range)
    };

    public int Radius { get; set; } = 10;
    public int Range { get; set; } = 20;
    public int Damage { get; set; } = 7;
    public int TargetCount { get; set; } = 4;

    public SkillGroup SkillGroup => SkillGroup.AEROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.TryGetValue(targetPosition, out var guid)) return;
        var hits = new HashSet<Guid>();
        var target = combatState.CombatActors[guid];
        for (int i = 0; i < TargetCount; i++)
        {
            hits.Add(target.Guid);
            var result = combatState.DealDamage(user, target, DamageSources.AIR.WithDamageAmount(Damage));
            if (result.armorBroken) combatState.ApplyStatus(target, new Stunned());
            var closest = combatState.CombatActors.Values.Where(actor => !hits.Contains(actor.Guid) && actor.Alignment != user.Alignment).OrderBy(actor => (actor.Position - target.Position).sqrMagnitude).FirstOrDefault();
            if ((closest.Position - target.Position).sqrMagnitude > Radius * Radius) return;
            if (closest == null) return;
            target = closest;
        }
    }
}
