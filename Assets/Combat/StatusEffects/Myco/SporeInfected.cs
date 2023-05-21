using System.IO;

public class SporeInfected : IStatusEffect
{
    public int Duration { get; set; } = -1;

    int alignmentSource;

    public SporeInfected() {}
    public SporeInfected(int alignmentSource)
    {
        this.alignmentSource = alignmentSource;
    }

    public void OnDie(ICombatActor actor, CombatState state)
    {
        var mushroom = new Mushroom();
        mushroom.Position = actor.Position;
        mushroom.Alignment = alignmentSource;
        state.AddActor(mushroom);
    }

        byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteInt(Duration);
            stream.WriteInt(alignmentSource);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Duration = stream.ReadInt();
            alignmentSource = stream.ReadInt();
        }
    }
}

