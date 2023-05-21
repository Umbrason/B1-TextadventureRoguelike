using System.Linq;
using UnityEngine;

public class SummonBarricade : ISkill, ISkillWithRange, ISkillWithMultitarget
{
    public string Description => "";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors =>
    Enumerable.Range(0, TargetCount).Select(
        _ => new RangedPositionSelector(true, false, Range)
    ).ToArray();

    public int Range { get; set; } = 3;
    public int TargetCount { get; set; } = 3;

    public SkillGroup SkillGroup => SkillGroup.RANGED;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var positions = parameters.Select(p => (Vector2Int)p);

        foreach (var pos in positions)
        {
            if (!combatState.ActorPositions.TryGetValue(pos, out var actorGuid))
            {
                var barricade = new Barricade();
                barricade.Position = pos;
                barricade.Alignment = user.Alignment;
                combatState.AddActor(barricade);
                continue;
            }
            var actor = combatState.CombatActors[actorGuid];
            if (!(actor is Barricade)) continue;
            actor.Health = new(0, actor.Health.Value + Barricade.HEALTH, actor.Health.Max + Barricade.HEALTH);
        }
    }
}
