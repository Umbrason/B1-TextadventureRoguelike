using System.Collections.Generic;
using System.IO;
using System.Linq;

public class RunInfo : IBinarySerializable, IReadOnlyRunInfo
{
    public Worldmap map;
    public PartyInfo party;
    public IReadOnlyWorldMap ReadOnlyWorldMap => map;
    public IReadOnlyPartyInfo ReadOnlyPartyInfo => party;

    public int CurrentDepth => pathList.Count - 1;
    public int CurrentIndex => pathList.LastOrDefault();
    public int Gold { get; set; } = 50;
    public bool Won { get; set; }
    public IWorldLocation CurrentLocation => map.MapLocations[CurrentDepth][CurrentIndex];

    private List<int> pathList = new();
    public IReadOnlyCollection<int> Path => pathList;

    public void AdvanceLocation(int pathChoice)
    {
        if (!(CurrentDepth == -1 || map.Connections[CurrentDepth][CurrentIndex].Contains(pathChoice))) return;
        pathList.Add(pathChoice);
    }

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteIBinarySerializable(map);
            stream.WriteIBinarySerializable(party);
            stream.WriteEnumerable(pathList, stream.WriteInt);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            map = stream.ReadIBinarySerializable<Worldmap>();
            party = stream.ReadIBinarySerializable<PartyInfo>();
            pathList = stream.ReadEnumerable(stream.ReadInt).ToList();
        }
    }
}

public interface IReadOnlyRunInfo
{
    public IReadOnlyWorldMap ReadOnlyWorldMap { get; }
    public IReadOnlyPartyInfo ReadOnlyPartyInfo { get; }
    public int CurrentDepth { get; }
    public int CurrentIndex { get; }
    public int Gold { get; }
    public bool Won { get; }
    public IReadOnlyCollection<int> Path { get; }
    public IReadOnlyWorldLocation CurrentLocation => ReadOnlyWorldMap.ReadOnlyMapLocations.ElementAt(CurrentDepth).ElementAt(CurrentIndex);
}