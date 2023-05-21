using System.IO;

public class SporeArmor : IStatusEffect
{
    public int Duration { get; set; } = 3;

    const int amountPerShroom = 2;
    private int mushroomCount;

    public SporeArmor() {}
    public SporeArmor(int mushroomCount)
    {
        this.mushroomCount = mushroomCount;
    }

    public void OnApply(ICombatActor actor, CombatState state)
    {
        actor.Armor.Max += mushroomCount * amountPerShroom;
        actor.Armor.Value += mushroomCount * amountPerShroom;
    }
    public void OnRemove(ICombatActor actor, CombatState state)
    {
        actor.Armor.Value -= mushroomCount * amountPerShroom;
        actor.Armor.Max -= mushroomCount * amountPerShroom;
    }

    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteInt(Duration);
            stream.WriteInt(mushroomCount);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Duration = stream.ReadInt();
            mushroomCount = stream.ReadInt();
        }
    }
}

