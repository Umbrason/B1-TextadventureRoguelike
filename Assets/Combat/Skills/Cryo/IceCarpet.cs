using UnityEngine;

public class IceCarpet : ISkill, ISkillWithRange
{
    public string Description => "Lay out a carpet of ice infront of you.";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };

    public SkillGroup SkillGroup => SkillGroup.CRYOMANCY;
    public int Range { get; set; } = 10;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPos = (Vector2Int)parameters[0];
        var coneshape = Shapes.GridCone(user.Position, targetPos, 45);
        foreach (var position in coneshape)
            combatState.SetTileModifier(position, new Ice());
    }
}
