using UnityEngine;

public class Ignite : ISkill, ISkillWithRange
{
    public string Description => "Ignite a flamable surface or creature";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(requiresLOS: true, requiresTileWalkable: true, range: Range)
    };
    public int Range { get; set; } = 5;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
    }
}
