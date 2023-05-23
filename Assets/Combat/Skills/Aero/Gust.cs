using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gust : ISkill, ISkillWithRange, ISkillWithDamage
{
    public string Description => "Pushes back enemies in a coneshape";
    public ClampedInt Cooldown { get; set; } = new(0, 0, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(false, false, Range)
    };
    public SkillGroup SkillGroup => SkillGroup.AEROMANCY;
    public int Range { get; set; } = 8;
    public int Damage { get; set; } = 3;

    private const float Angle = 45f;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
        var shape = Shapes.GridCone(user.Position, targetPosition, Angle);
        var actorsInShape = shape.Where(combatState.ActorPositions.ContainsKey)
                                .Select(combatState.ActorPositions.GetValueOrDefault)
                                .Select(combatState.CombatActors.GetValueOrDefault)
                                .Where(actor => actor.Alignment != user.Alignment)
                                .OrderByDescending(actor => (actor.Position - user.Position).sqrMagnitude);
        foreach (var actor in actorsInShape)
        {
            var delta = actor.Position - user.Position;
            var pushTarget = Vector2Int.RoundToInt(actor.Position + ((Vector2)delta).normalized * 5f);
            var pushbackLine = new Queue<Vector2Int>(Shapes.GridLine(actor.Position, pushTarget).Skip(1));
            combatState.DealDamage(user, actor, DamageSources.AIR.WithDamageAmount(Damage));
            while (pushbackLine.Count > 0 && combatState.IsTileWalkable(pushbackLine.Peek()))
                combatState.TeleportActor(actor, pushbackLine.Dequeue());
        }
    }
}
