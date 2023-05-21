using UnityEngine;

public class ConvergeFlames : ISkill, ISkillWithRange, ISkillWithRadius, ISkillWithDamage
{
    public string Description => "Cause all nearby flames to converge on a target in sight. Damage is proportional to the amount of nearby fire";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range)
    };

    public int Range { get; set; } = 15;
    public int Damage { get; set; } = 1;
    public int Radius { get; set; } = 5;
    public SkillGroup SkillGroup => SkillGroup.PYROMANCY;
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetLocation = (Vector2Int)parameters[0];
        var targetActor = combatState.CombatActors[combatState.ActorPositions[targetLocation]];
        int flameCount = 0;
        var circle = Shapes.GridCircle(targetLocation, Radius);
        foreach (var tile in circle)
        {
            if (combatState.Room.TileModifiers.TryGetValue(tile, out var modifier) && modifier is Fire)
            {
                flameCount++;
                combatState.RemoveTileModifier(tile);
            }
        }
        combatState.DealDamage(user, targetActor, DamageSources.FIRE.WithDamageAmount(Damage * (flameCount + 1)));
    }
}
