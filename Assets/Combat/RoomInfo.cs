using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RoomInfo : IBinarySerializable, IReadOnlyRoomInfo
{
    private Dictionary<Vector2Int, Tile> Layout { get; set; }
    public (Vector2Int, Tile)[] Tiles => Layout.Select(pair => (pair.Key, pair.Value)).ToArray();
    public Dictionary<Vector2Int, ITileModifier> TileModifiers { get; private set; }
    public Vector2Int MinCorner { get; private set; }
    public Vector2Int MaxCorner { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vector2Int Size => new(Width, Height);
    IReadOnlyDictionary<Vector2Int, ITileModifier> IReadOnlyRoomInfo.TileModifiers => TileModifiers;

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteVector2Int(MinCorner);
            stream.WriteVector2Int(MaxCorner);
            stream.WriteInt(Width);
            stream.WriteInt(Height);
            stream.WriteDictionary(Layout, stream.WriteVector2Int, stream.WriteEnum<Tile>);
            stream.WriteDictionary(TileModifiers, stream.WriteVector2Int, stream.WriteIBinarySerializable);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            MinCorner = stream.ReadVector2Int();
            MaxCorner = stream.ReadVector2Int();
            Width = stream.ReadInt();
            Height = stream.ReadInt();
            Layout = stream.ReadDictionary(stream.ReadVector2Int, stream.ReadEnum<Tile>);
            TileModifiers = stream.ReadDictionary(stream.ReadVector2Int, stream.ReadIBinarySerializable<ITileModifier>);
        }
    }

    public Tile this[Vector2Int key] => Layout.ContainsKey(key) ? Layout[key] : Tile.VOID;

    private static void RoomFromString(string layout, out Dictionary<Vector2Int, Tile> layoutDict, out Dictionary<Vector2Int, ITileModifier> tileModifiers)
    {
        var tileModifierLines = layout.Split("#").Skip(1).ToArray();
        var tileModifierNames = new Dictionary<int, string>();

        var layoutLines = layout.Split("#")[0].Split("\n");
        layoutDict = new Dictionary<Vector2Int, Tile>();
        tileModifiers = new Dictionary<Vector2Int, ITileModifier>();
        for (int h = 0; h < layoutLines.Length; h++)
        {
            for (int w = 0; w < layoutLines[h].Length; w++)
            {
                var ch = layoutLines[h][w];
                var tileType = ch switch
                {
                    'W' => Tile.WALL,
                    'w' => Tile.WALL,
                    'F' => Tile.FLOOR,
                    'f' => Tile.FLOOR,
                    '1' => Tile.FLOOR,
                    _ => Tile.VOID,
                };
                if (ch > '0' && ch < '9')
                {
                    tileModifiers[new(w, h)] = IBinarySerializableFactory<ITileModifier>.CreateDefault(tileModifierNames[(int)ch]);
                    tileType = Tile.FLOOR;
                }
                if (tileType == Tile.VOID) continue;
                layoutDict.Add(new(w, layoutLines.Length - h - 1), tileType);
            }
        }
    }
    public RoomInfo() { }
    public RoomInfo(string layout)
    {
        RoomFromString(layout, out var layoutDict, out var tileModifiersDict);
        this.Layout = layoutDict;
        this.TileModifiers = tileModifiersDict;
        var min = new Vector2Int(int.MaxValue, int.MaxValue);
        var max = new Vector2Int(int.MinValue, int.MinValue);
        foreach (var key in layoutDict.Keys)
        {
            min.x = key.x < min.x ? key.x : min.x;
            min.y = key.y < min.y ? key.y : min.y;
            max.x = key.x > max.x ? key.x : max.x;
            max.y = key.y > max.y ? key.y : max.y;
        }
        MinCorner = min;
        MaxCorner = max;
        Width = max.x - min.x + 1;
        Height = max.y - min.y + 1;
    }
    public enum Tile
    {
        WALL, FLOOR, VOID
    }
}

public interface IReadOnlyRoomInfo
{
    public (Vector2Int, RoomInfo.Tile)[] Tiles { get; }
    public IReadOnlyDictionary<Vector2Int, ITileModifier> TileModifiers { get; }
    public Vector2Int MinCorner { get; }
    public Vector2Int MaxCorner { get; }
    public int Width { get; }
    public int Height { get; }
    public Vector2Int Size => new(Width, Height);
    public RoomInfo.Tile this[Vector2Int key] { get; }
}

