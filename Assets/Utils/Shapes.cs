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

    public static Vector2Int[] GridCone(Vector2Int start, Vector2Int end, float angle)
    {
        var tiles = new List<Vector2Int>();
        var delta = end - start;
        var radius = Mathf.CeilToInt((delta).magnitude);
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var alpha = Vector2.Angle(delta, new(x, y));
                if (x * x + y * y > radius * radius || alpha > angle / 2f) continue;
                tiles.Add(start + new Vector2Int(x, y));
            }
        return tiles.ToArray();
    }

    public static Vector2Int[] GridSquare(Vector2Int center, int halfExtend)
    {
        var tiles = new List<Vector2Int>();
        for (int x = -halfExtend; x <= halfExtend; x++)
            for (int y = -halfExtend; y <= halfExtend; y++)
                tiles.Add(center + new Vector2Int(x, y));
        return tiles.ToArray();
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

    public static Vector2Int[] GridCircleLine(Vector2Int center, int radius)
    {
        var tiles = new List<Vector2Int>();
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y > radius * radius) continue;
                if (x * x + y * y < radius * radius - 1) continue;
                tiles.Add(center + new Vector2Int(x, y));
            }
        return tiles.ToArray();
    }
}