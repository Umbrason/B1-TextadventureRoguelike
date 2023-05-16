using System.IO;
using System.Linq;

public class Worldmap : IBinarySerializable
{
    private IWorldLocation[][] mapLocations;
    private int[][][] connections;

    private Worldmap(IWorldLocation[][] locations, int[][][] connections)
    {
        this.mapLocations = locations;
        this.connections = connections;
    }

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteEnumerable(mapLocations, (array) => stream.WriteEnumerable(array, stream.WriteIBinarySerializable));
            stream.WriteEnumerable(connections, (array) => stream.WriteEnumerable(array, (intarray) => stream.WriteEnumerable(intarray, stream.WriteInt)));
            return stream.GetAllBytes();
        }

        set
        {
            var stream = new MemoryStream(value);
            mapLocations = stream.ReadEnumerable<IWorldLocation[]>(() => stream.ReadEnumerable<IWorldLocation>(stream.ReadIBinarySerializable<IWorldLocation>));
            connections = stream.ReadEnumerable<int[][]>(() => stream.ReadEnumerable<int[]>(() => stream.ReadEnumerable<int>(stream.ReadInt)));
        }
    }

    private IWorldLocation[] getConnectedLocations(int depth, int index)
    {
        var nextDepth = mapLocations[depth + 1];
        var connectedIndices = connections[depth][index];
        return connectedIndices.Select((i) => nextDepth[i]).ToArray();
    }
}
