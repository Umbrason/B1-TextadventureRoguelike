
using UnityEngine;

public class Smokecover : ISkill, ISkillWithRange, ISkillWithRadius
{
    public string Description => "Cover target area in thick smoke, that blocks vision";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };

    public int Radius { get; set; } = 5;
    public int Range { get; set; } = 15;

    public SkillGroup SkillGroup => SkillGroup.PYROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var center = (Vector2Int)parameters[0];
        var circle = Shapes.GridCircle(center, Radius);
        foreach (var tile in circle) combatState.SetTileModifier(tile, new Smoke());
    }
}
