using System.Collections.Generic;
using UnityEngine;

public static class Shapes
{
    public static Vector2Int[] GridLine(Vector2Int from, Vector2Int to)
    {
        var delta = to - from;
        var length = delta.x + delta.y;
        var m = Mathf.Abs(delta.x) > 0 ? delta.y / (float)delta.x : delta.y;
        var y = from.y;
        var dy = 0f;
        var positions = new List<Vector2Int>();
        for (int x = from.x; x <= to.x; x++)
        {
            dy += m;
            while (dy >= 1)
            {
                positions.Add(new(x, y));
                y++;
                dy--;
            }
            positions.Add(new(x, y));
        }
        return positions.ToArray();
    }

    public static Vector2Int[] GridCircle(Vector2Int center, int radius)
    {
        var tiles = new List<Vector2Int>();
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y > radius * radius) continue;
                tiles.Add(center + new Vector2Int(x, y));
            }
        return tiles.ToArray();
    }
}