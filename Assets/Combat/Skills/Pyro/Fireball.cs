using System.IO;
using UnityEngine;

public class Fireball : ISkill, ISkillWithRadius, ISkillWithRange
{
    public string Description => "qouth the wizard: I CAST FIREBALL!";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 3;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(requiresLOS: true, range: Range),
    };
    public int Radius { get; set; } = 3;
    public int Range { get; set; } = 10;

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
    }
}
