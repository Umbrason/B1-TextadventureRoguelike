
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    private readonly IReadOnlyDictionary<Vector2Int, Tile> layout;
    public (Vector2Int, Tile)[] Tiles => layout.Select(pair => (pair.Key, pair.Value)).ToArray();

    public Vector2Int MinCorner { get; private set; }
    public Vector2Int MaxCorner { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vector2Int Size => new(Width, Height);

    public Tile this[Vector2Int key] => layout.ContainsKey(key) ? layout[key] : Tile.VOID;

    private static Dictionary<Vector2Int, Tile> LayoutDictFromString(string layout)
    {
        var lines = layout.Split("\n");
        var layoutDict = new Dictionary<Vector2Int, Tile>();
        for (int h = 0; h < lines.Length; h++)
        {
            for (int w = 0; w < lines[h].Length; w++)
            {
                var tileType = lines[h][w] switch
                {
                    'W' => Tile.WALL,
                    'w' => Tile.WALL,
                    'F' => Tile.FLOOR,
                    'f' => Tile.FLOOR,
                    _ => Tile.VOID,
                };
                if (tileType == Tile.VOID) continue;
                layoutDict.Add(new(w, lines.Length - h - 1), tileType);
            }
        }
        return layoutDict;
    }
    public Room(string layout) : this(LayoutDictFromString(layout)) { }
    public Room(Dictionary<Vector2Int, Tile> layout)
    {
        this.layout = layout;
        var min = new Vector2Int(int.MaxValue, int.MaxValue);
        var max = new Vector2Int(int.MinValue, int.MinValue);
        foreach (var key in layout.Keys)
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

