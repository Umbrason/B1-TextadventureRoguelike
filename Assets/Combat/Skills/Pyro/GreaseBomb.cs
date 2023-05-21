using UnityEngine;

public class GreaseBomb : ISkill
{
    public string Description => "Cover target area in flammable grease";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };

    public int Radius { get; set; } = 6;
    public int Range { get; set; } = 10;

    public SkillGroup SkillGroup => SkillGroup.PYROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var center = (Vector2Int)parameters[0];
        var circle = Shapes.GridCircle(center, Radius);
        foreach (var tile in circle) combatState.SetTileModifier(tile, new Grease());
    }
}
