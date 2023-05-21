using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EarthSlam : ISkill
{
    public string Description => "Cause the ground infront of you to crack open and damage nearby enemies";
    public ClampedInt Cooldown { get; set; } = new(0, 2, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(false, false, Range)
    };
    public SkillGroup SkillGroup => SkillGroup.TERRAMANCY;
    public int Range { get; set; } = 8;
    public int Damage { get; set; } = 4;

    private const float Angle = 45f;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
        targetPosition = Vector2Int.RoundToInt(((Vector2)(targetPosition - user.Position)).normalized * Range);
        var shape = Shapes.GridCone(user.Position, targetPosition, Angle);
        var actorsInShape = shape.Where(combatState.ActorPositions.ContainsKey)
                                .Select(combatState.ActorPositions.GetValueOrDefault)
                                .Select(combatState.CombatActors.GetValueOrDefault)
                                .Where(actor => actor.Alignment != user.Alignment);
        foreach (var actor in actorsInShape)
        {
            var result = combatState.DealDamage(user, actor, DamageSources.EARTH.WithDamageAmount(Damage));
            if (result.armorBroken) combatState.ApplyStatus(actor, new Prone());
        }
    }
}
