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
    [SerializeField] private Sprite EliteCombatLocationSprite;
    [SerializeField] private Sprite BossCombatLocationSprite;
    [SerializeField] private Sprite DefaultLocationSprite;
    [SerializeField] private Sprite Campfire;
    [SerializeField] private Sprite Shop;
    [SerializeField] private Sprite[] ShopIcons;

    private Dictionary<Type, Sprite> cached_LocationSprites;
    public Dictionary<Type, Sprite> LocationSprites => cached_LocationSprites ??= new()
    {
        {typeof(GoblinEncounter), CombatLocationSprite},
        {typeof(GoblinAmbush), EliteCombatLocationSprite},
        {typeof(GoblinLair), BossCombatLocationSprite},
        {typeof(Graveyard), CombatLocationSprite},
        {typeof(Chappel), EliteCombatLocationSprite},
        {typeof(Cathedral), BossCombatLocationSprite},
        {typeof(HellChunk), CombatLocationSprite},
        {typeof(HellIsland), EliteCombatLocationSprite},
        {typeof(ArchdemonLair), BossCombatLocationSprite},
        {typeof(SkillShop), Shop},
        {typeof(PowerWell), DefaultLocationSprite},
        {typeof(RestingPlace), Campfire}
    };

    public void Start()
    {
        Render();
    }

    public Vector2 CalcMapCenter()
    {
        var map = RunManager.ReadOnlyRunInfo.ReadOnlyWorldMap;
        var currentDepth = RunManager.ReadOnlyRunInfo.CurrentDepth;
        var currentIndex = RunManager.ReadOnlyRunInfo.CurrentIndex;
        if (currentDepth == -1) return default;
        var seed = map.Seed;
        var layerSize = map.ReadOnlyMapLocations.ElementAt(currentDepth).Count;
        return DepthIndexToWorld(currentDepth, currentIndex, layerSize, seed);
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
        transform.position = Vector3.zero;
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
                if (location is SkillShop)
                    ((Image)instance.targetGraphic).sprite = ShopIcons[(int)((SkillShop)location).SkillGroup];
                var isNextLayer = depth == currentDepth + 1;
                var isFirstLayer = depth == 0;
                var isConnectedToCurrent = (currentDepth == -1 && isFirstLayer) ||
                                           (isNextLayer && map.ReadOnlyMapConnections.ElementAt(currentDepth).ElementAt(currentIndex).Contains(index));
                var isInteractable = isConnectedToCurrent;
                instance.interactable = isInteractable;

                var isInPath = (RunManager.ReadOnlyRunInfo.Path.Count > depth) && (RunManager.ReadOnlyRunInfo.Path.ElementAt(depth) == index);
                instance.enabled = !isInPath; //disable button to turn white when isInPath

                var locationAdvanceIndex = index;
                instance.onClick.AddListener(() => RunManager.AdvanceLocation(locationAdvanceIndex));
                instance.transform.position = DepthIndexToWorld(depth, index, layerCount, seed);
                visuals.Add(instance.gameObject);
            }
        }
        var edges = map.ReadOnlyMapConnections;
        for (int fromDepth = 0; fromDepth < edges.Count; fromDepth++)
        {
            var layerCount = edges.ElementAt(fromDepth).Count;
            var nextLayerCount = locations.ElementAt(fromDepth + 1).Count;
            for (int fromIndex = 0; fromIndex < layerCount; fromIndex++)
            {
                var toIndices = edges.ElementAt(fromDepth).ElementAt(fromIndex);
                foreach (var toIndex in toIndices)
                {
                    var from = DepthIndexToWorld(fromDepth, fromIndex, layerCount, seed) + Vector2.right * 1.5f;
                    var to = DepthIndexToWorld(fromDepth + 1, toIndex, nextLayerCount, seed) - Vector2.right * 1.5f;
                    var lr = Instantiate(LocationEdgeTemplate, transform);
                    lr.useWorldSpace = false;

                    var isFromInPath = (RunManager.ReadOnlyRunInfo.Path.Count > fromDepth) && (RunManager.ReadOnlyRunInfo.Path.ElementAt(fromDepth) == fromIndex);
                    var isToInPath = (RunManager.ReadOnlyRunInfo.Path.Count > fromDepth + 1) && (RunManager.ReadOnlyRunInfo.Path.ElementAt(fromDepth + 1) == toIndex);
                    var isConnectedToCurrent = fromDepth == currentDepth && fromIndex == currentIndex;

                    lr.startColor = lr.endColor =
                    isFromInPath && isToInPath ?
                        Color.white :
                        isConnectedToCurrent ?
                            new Color(.5f, .5f, .5f, 1) :
                            new Color(.2f, .2f, .2f, 1);
                    lr.SetPositions(new Vector3[] { from, to });
                    visuals.Add(lr.gameObject);
                }
            }
        }
        transform.position = -CalcMapCenter();
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

