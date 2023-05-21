using UnityEngine;

public class ThrowCaltrops : ISkill, ISkillWithRange, ISkillWithRadius
{
    public string Description => "Throw some caltrops to stop enemies from approaching";
    public ClampedInt Cooldown { get; set; } = new(0, 5, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };
    public int Range { get; set; } = 7;
    public int Radius { get; set; } = 3;
    public SkillGroup SkillGroup => SkillGroup.RANGED;
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        var tiles = Shapes.GridCircle(position, Radius);
        foreach (var tile in tiles)
            combatState.SetTileModifier(tile, TileModifiers.CALTROPS);
    }
}
