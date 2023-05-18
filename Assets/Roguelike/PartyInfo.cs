using System.Collections.Generic;
using System.IO;
using System.Linq;

public class PartyInfo : IBinarySerializable, IReadOnlyPartyInfo
{
    public PlayableCombatActor PartyLeader;
    public List<PlayableCombatActor> PartyMembers { get; private set; } = new();
    public ICombatActor[] CombatActors => PartyMembers.Prepend(PartyLeader).ToArray();

    public static PartyInfo Empty => new();

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteIBinarySerializable(PartyLeader);
            stream.WriteEnumerable(PartyMembers, stream.WriteIBinarySerializable);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            PartyLeader = stream.ReadIBinarySerializable<PlayableCombatActor>();
            PartyMembers = stream.ReadEnumerable(stream.ReadIBinarySerializable<PlayableCombatActor>).ToList();
        }
    }
}

public interface IReadOnlyPartyInfo
{

}