using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Swiftstrike : ISkill, ISkillWithRange, ISkillWithRadius, ISkillWithDamage, ISkillWithMultitarget
{
    public string Description => "";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };

    public int Damage { get; set; } = 5;
    public int Radius { get; set; } = 10;
    public int Range { get; set; } = 15;

    public SkillGroup SkillGroup => SkillGroup.MELEE;

    public int TargetCount { get; set; } = 5;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.TryGetValue(targetPosition, out var guid)) return;
        var hits = new HashSet<Guid>();
        var target = combatState.CombatActors[guid];
        for (int i = 0; i < TargetCount; i++)
        {
            hits.Add(target.Guid);
            var result = combatState.DealDamage(user, target, DamageSources.PHYSICAL.WithDamageAmount(Damage));
            if (result.armorBroken) combatState.ApplyStatus(target, new Stunned());
            var closest = combatState.CombatActors.Values.Where(actor => !hits.Contains(actor.Guid) && actor.Alignment != user.Alignment).OrderBy(actor => (actor.Position - target.Position).sqrMagnitude).FirstOrDefault();
            if ((closest.Position - target.Position).sqrMagnitude > Radius * Radius) return;
            if (closest == null) return;
            target = closest;
        }
        combatState.TeleportActor(user, target.Position);
    }
}
