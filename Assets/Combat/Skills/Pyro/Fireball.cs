using System.IO;
using System.Linq;
using UnityEngine;

public class Fireball : ISkill, ISkillWithRadius, ISkillWithRange, ISkillWithDamage
{
    public string Description => "qouth the wizard: I CAST FIREBALL!";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 3;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(requiresLOS: true, range: Range),
    };
    public int Radius { get; set; } = 3;
    public int Range { get; set; } = 30;
    public int Damage { get; set; } = 7;

    public SkillGroup SkillGroup => SkillGroup.PYROMANCY;

    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteClampedInt(Cooldown);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Cooldown = stream.ReadClampedInt();
        }
    }

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
        var area = Shapes.GridCircle(targetPosition, Radius);
        var actorsInArea = area.Where(p => combatState.ActorPositions.ContainsKey(p)).Select(p => combatState.CombatActors[combatState.ActorPositions[p]]).ToArray();
        foreach (var tile in area)
        {
            combatState.SetTileModifier(tile, TileModifiers.FIRE);
        }
        foreach (var target in actorsInArea)
        {
            var result = combatState.DealDamage(user, target, DamageSources.FIRE.WithDamageAmount(Damage));
            if (result.armorBroken) combatState.ApplyStatus(target, new Burning());
        }
    }
}
