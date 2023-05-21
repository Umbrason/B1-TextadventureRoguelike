using UnityEngine;

public class IceCage : ISkill, ISkillWithRange, ISkillWithRadius
{
    public string Description => "Imprison an enemy with a cage of ice spikes";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };
    public SkillGroup SkillGroup => SkillGroup.CRYOMANCY;

    public int Radius { get; set; } = 4;
    public int Range { get; set; } = 20;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var center = (Vector2Int)parameters[0];
        var circle = Shapes.GridCircle(center, Radius);
        var circleOutline = Shapes.GridCircleLine(center, Radius);
        foreach (var pos in circle)
            combatState.SetTileModifier(pos, new Ice());
        foreach (var pos in circleOutline)
        {
            var spike = new IceSpike();
            spike.Position = pos;
            combatState.AddActor(spike);
        }
    }
}
