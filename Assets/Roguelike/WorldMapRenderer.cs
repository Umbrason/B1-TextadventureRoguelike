using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapRenderer : MonoBehaviour
{
    public Button LocationNodeTemplate;
    public LineRenderer LocationEdgeTemplate;

    [SerializeField] private Sprite CombatLocationSprite;
    [SerializeField] private Sprite DefaultLocationSprite;
    private Dictionary<Type, Sprite> cached_LocationSprites;
    public Dictionary<Type, Sprite> LocationSprites => cached_LocationSprites ??= new()
    {
        {typeof(CombatLocation), CombatLocationSprite},
    };

    public void Start()
    {
        Render();
    }

    public void Move(int depth, int index)
    {
        var map = RunManager.ReadOnlyRunInfo.ReadOnlyWorldMap;
        var seed = map.Seed;
        var layerSize = map.ReadOnlyMapLocations.ElementAt(depth).Count;
        var worldPos = DepthIndexToWorld(depth, index, layerSize, seed);
        Camera.main.transform.position = worldPos;
        Render();
    }

    private readonly List<GameObject> visuals = new();
    public void ClearVisuals()
    {
        var queue = new Queue<GameObject>(visuals);
        while (queue.Count > 0) Destroy(queue.Dequeue());
        visuals.Clear();
    }

    public void Render()
    {
        ClearVisuals();
        var map = RunManager.ReadOnlyRunInfo.ReadOnlyWorldMap;
        var currentDepth = RunManager.ReadOnlyRunInfo.CurrentDepth;
        var currentIndex = RunManager.ReadOnlyRunInfo.CurrentIndex;
        var seed = map.Seed;
        var locations = map.ReadOnlyMapLocations;
        for (int depth = 0; depth < locations.Count; depth++)
        {
            var layerCount = locations.ElementAt(depth).Count;
            for (int index = 0; index < layerCount; index++)
            {
                var location = locations.ElementAt(depth).ElementAt(index);
                var instance = Instantiate(LocationNodeTemplate, transform);
                ((Image)instance.targetGraphic).sprite = LocationSprites[location.GetType()];
                instance.interactable = (depth == currentDepth + 1);
                instance.enabled = !(RunManager.ReadOnlyRunInfo.Path.Contains((depth, index)) || (depth == currentDepth && index == currentIndex));
                var locationAdvanceIndex = index;
                instance.onClick.AddListener(() => RunManager.AdvanceLocation(locationAdvanceIndex));
                instance.transform.position = DepthIndexToWorld(depth, index, layerCount, seed);
                visuals.Add(instance.gameObject);
            }
        }
        var edges = map.ReadOnlyMapConnections;
        for (int depth = 0; depth < edges.Count; depth++)
        {
            var layerCount = edges.ElementAt(depth).Count;
            var nextLayerCount = locations.ElementAt(depth + 1).Count;
            for (int index = 0; index < layerCount; index++)
            {
                var edgeCount = edges.ElementAt(depth).ElementAt(index);
                foreach (var edge in edgeCount)
                {
                    var from = DepthIndexToWorld(depth, index, layerCount, seed) + Vector2.right * 1.5f;
                    var to = DepthIndexToWorld(depth + 1, edge, nextLayerCount, seed) - Vector2.right * 1.5f;
                    var lr = Instantiate(LocationEdgeTemplate, transform);
                    var containsFrom = RunManager.ReadOnlyRunInfo.Path.Contains((depth, index));
                    var containsTo = RunManager.ReadOnlyRunInfo.Path.Contains((depth + 1, edge)) || (depth == currentDepth && edge == currentIndex);
                    lr.startColor = lr.endColor =
                    containsFrom & containsTo ?
                        Color.white :
                        depth == currentDepth ?
                            new Color(.5f, .5f, .5f, 1) :
                            new Color(.2f, .2f, .2f, 1);
                    lr.SetPositions(new Vector3[] { from, to });
                    visuals.Add(lr.gameObject);
                }
            }
        }
    }


    private Vector2 Offset;

    public float perlinScale = .4214124f;

    const float vspacing = 3;
    const float hspacing = 8;
    private Vector2 DepthIndexToWorld(int depth, int index, int layerSize, int seed)
    {
        var position = new Vector2(depth * hspacing, Mathf.FloorToInt(((((float)index / layerSize * 8 - (8 - 8 / layerSize) / 2)) * vspacing)));
        var perlinX = Mathf.PerlinNoise(position.x * perlinScale, position.y * perlinScale + seed * 813.56213f);
        var perlinY = Mathf.PerlinNoise(position.x * perlinScale, position.y * perlinScale + 645.19f + seed * -373.56213f);
        var offset = new Vector2((perlinX - .5f) * hspacing, (perlinY - .5f) * Mathf.Clamp(Mathf.Log((8 - layerSize), 2), 1, 8) * vspacing) * .5f;
        var snappedOffset = (Vector2)Vector2Int.RoundToInt(offset * 9) / 9f;
        return position + snappedOffset;
    }
}

