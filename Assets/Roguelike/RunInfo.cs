using System.Collections.Generic;
using System.IO;
using System.Linq;

public class RunInfo : IBinarySerializable, IReadOnlyRunInfo
{
    public Worldmap map;
    public PartyInfo party;
    public IReadOnlyWorldMap ReadOnlyWorldMap => map;
    public IReadOnlyPartyInfo ReadOnlyPartyInfo => party;

    public int CurrentDepth { get; private set; } = -1;
    public int CurrentIndex { get; private set; }
    public IWorldLocation CurrentLocation => map.MapLocations[CurrentDepth][CurrentIndex];

    private List<(int, int)> pathList = new();
    public IReadOnlyCollection<(int, int)> Path => pathList;

    public void AdvanceLocation(int pathChoice)
    {
        pathList.Add((CurrentDepth, CurrentIndex));
        CurrentDepth++;
        CurrentIndex = map.Connections[CurrentDepth][CurrentIndex][pathChoice];
    }

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteIBinarySerializable(map);
            stream.WriteIBinarySerializable(party);
            stream.WriteInt(CurrentDepth);
            stream.WriteInt(CurrentIndex);
            stream.WriteEnumerable(pathList, (pair) => { stream.WriteInt(pair.Item1); stream.WriteInt(pair.Item2); });
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            map = stream.ReadIBinarySerializable<Worldmap>();
            party = stream.ReadIBinarySerializable<PartyInfo>();
            CurrentDepth = stream.ReadInt();
            CurrentIndex = stream.ReadInt();
            pathList = stream.ReadEnumerable(() => (stream.ReadInt(), stream.ReadInt())).ToList();
        }
    }
}

public interface IReadOnlyRunInfo
{
    public IReadOnlyWorldMap ReadOnlyWorldMap { get; }
    public IReadOnlyPartyInfo ReadOnlyPartyInfo { get; }
    public int CurrentDepth { get; }
    public int CurrentIndex { get; }
    public IReadOnlyCollection<(int, int)> Path { get; }
    public IReadOnlyWorldLocation CurrentLocation => ReadOnlyWorldMap.ReadOnlyMapLocations.ElementAt(CurrentDepth).ElementAt(CurrentIndex);
}