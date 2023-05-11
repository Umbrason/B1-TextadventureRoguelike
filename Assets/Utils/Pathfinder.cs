using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder
{
    private Func<Vector2Int, bool> walkable;
    public Pathfinder(Func<Vector2Int, bool> walkable)
    {
        this.walkable = walkable;
    }

    private readonly Vector2Int[] n4offsets = new Vector2Int[] {
        Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down
    };

    public Vector2Int[] FromTo(Vector2Int from, Vector2Int to, int budget = -1)
    {
        var seen = new HashSet<Vector2Int>();
        var discoverQueue = new Queue<Vector2Int>();
        discoverQueue.Enqueue(from);
        seen.Add(from);
        var nodeDict = new Dictionary<Vector2Int, Node>();
        nodeDict.Add(from, new(from, from, 0));
        while (discoverQueue.Count > 0)
        {
            var parentPosition = discoverQueue.Dequeue();
            var totalDistance = nodeDict[parentPosition].distance + 1;
            if (budget >= 0 && totalDistance > budget) continue;
            foreach (var offset in n4offsets)
            {
                var childPosition = parentPosition + offset;
                if (seen.Contains(childPosition)) continue;
                seen.Add(childPosition);
                if (!walkable(childPosition)) continue;

                nodeDict[childPosition] = new(parentPosition, childPosition, totalDistance + 1);
                discoverQueue.Enqueue(childPosition);
                if (childPosition == to) break;
            }
        }
        if (!nodeDict.ContainsKey(to)) return new Vector2Int[0];
        var path = new List<Vector2Int>();
        var node = nodeDict[to];
        while (node.position != from)
        {
            path.Add(node.position);
            node = nodeDict[node.parentPosition];
        }
        return path.AsEnumerable().Reverse().ToArray();
    }

    private struct Node
    {
        public Vector2Int parentPosition;
        public Vector2Int position;
        public int distance;
        public Node(Vector2Int parentPosition, Vector2Int position, int distance)
        {
            this.parentPosition = parentPosition;
            this.position = position;
            this.distance = distance;
        }
    }
}