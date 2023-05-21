using UnityEngine;

public class Flamewall : ISkill, ISkillWithRange, ISkillWithRadius
{
    public string Description => "Create a wall of fire between two positions in sight.";
    public ClampedInt Cooldown { get; set; } = new(0, 2, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new LineTargetSelector()
    };

    public int Radius { get; set; } = 10;
    public int Range { get; set; } = 15;

    public SkillGroup SkillGroup => SkillGroup.PYROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var positions = (Vector2Int[])parameters[0];
        var p1 = positions[0];
        var p2 = positions[1];
        var line = Shapes.GridLine(p1, p2);
        foreach (var pos in line) combatState.SetTileModifier(pos, new Fire());
    }
}
