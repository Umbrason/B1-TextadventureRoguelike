using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Worldmap : IBinarySerializable, IReadOnlyWorldMap
{
    public IWorldLocation[][] MapLocations { get; private set; }
    public int[][][] Connections { get; private set; }
    public int Seed { get; private set; }

    public IReadOnlyCollection<IReadOnlyCollection<IReadOnlyWorldLocation>> ReadOnlyMapLocations => MapLocations;
    IReadOnlyCollection<IReadOnlyCollection<IReadOnlyCollection<int>>> IReadOnlyWorldMap.ReadOnlyMapConnections => Connections;

    private Worldmap(IWorldLocation[][] locations, int[][][] connections)
    {
        this.MapLocations = locations;
        this.Connections = connections;
    }

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteEnumerable(MapLocations, (array) => stream.WriteEnumerable(array, stream.WriteIBinarySerializable));
            stream.WriteEnumerable(Connections, (array) => stream.WriteEnumerable(array, (intarray) => stream.WriteEnumerable(intarray, stream.WriteInt)));
            stream.WriteInt(Seed);
            return stream.GetAllBytes();
        }

        set
        {
            var stream = new MemoryStream(value);
            MapLocations = stream.ReadEnumerable<IWorldLocation[]>(() => stream.ReadEnumerable<IWorldLocation>(stream.ReadIBinarySerializable<IWorldLocation>));
            Connections = stream.ReadEnumerable<int[][]>(() => stream.ReadEnumerable<int[]>(() => stream.ReadEnumerable<int>(stream.ReadInt)));
            Seed = stream.ReadInt();
        }
    }


    private IWorldLocation[] getConnectedLocations(int depth, int index)
    {
        var nextDepth = MapLocations[depth + 1];
        var connectedIndices = Connections[depth][index];
        return connectedIndices.Select((i) => nextDepth[i]).ToArray();
    }

    const int forcedBattles = 5;
    const int fillerLocations = 2;

    static readonly float[] layerExpansionWeights = new float[] {
   .5f,.5f,.5f,.5f,
    0f, 0f, 0f,
    2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f };

    public static Worldmap Generate(int seed)
    {
        var mapLocationList = new List<IWorldLocation[]>();
        var mapConnectionList = new List<int[][]>();
        Random.InitState(seed);
        mapLocationList.Add(GenerateLocations(1, Locations.FillerLocations));
        for (int i = 0; i < forcedBattles; i++)
        {
            var combatPool = i < forcedBattles / 2 + Random.Range(-2, 2) ? Locations.CombatLocations : Locations.EliteCombatLocations;
            for (int j = 0; j < fillerLocations + 1; j++)
            {
                var currentLayer = mapLocationList.Last();
                var nextLayerCount = Mathf.Clamp(Mathf.RoundToInt(currentLayer.Length * (layerExpansionWeights[Random.Range(0, layerExpansionWeights.Length)])), 1, 8);
                if (i < 1 && nextLayerCount == 1) nextLayerCount = 2;
                var nextLayer = GenerateLocations(nextLayerCount, j == 0 ? combatPool : Locations.FillerLocations); //force battles on first iteration of inner loop
                mapLocationList.Add(nextLayer);
                var connections = new int[currentLayer.Length][];
                for (int c = 0; c < connections.Length; c++)
                {
                    var indices = new int[Mathf.Max(1, nextLayerCount / currentLayer.Length)]; //length = size ratio from this to next layer
                    for (int ci = 0; ci < indices.Length; ci++)
                        indices[ci] = Mathf.FloorToInt(c * (nextLayerCount / (float)currentLayer.Length)) + ci;
                    connections[c] = indices;
                }
                mapConnectionList.Add(connections);
            }
        }

        //add boss location
        mapConnectionList.Add(mapLocationList.Last().Select(l => new int[1]).ToArray());
        mapLocationList.Add(new IWorldLocation[] { Locations.BossCombatLocations.RandomElement() }); //bossLocation

        return new(mapLocationList.ToArray(), mapConnectionList.ToArray());
    }

    private static IWorldLocation[] GenerateLocations(int count, IWorldLocation[] pool)
    {
        var locations = new IWorldLocation[count];
        for (int i = 0; i < count; i++) locations[i] = pool.RandomElement();
        return locations;
    }

}

public interface IReadOnlyWorldMap
{
    IReadOnlyCollection<IReadOnlyCollection<IReadOnlyWorldLocation>> ReadOnlyMapLocations { get; }
    IReadOnlyCollection<IReadOnlyCollection<IReadOnlyCollection<int>>> ReadOnlyMapConnections { get; }
    public int Seed { get; }
}