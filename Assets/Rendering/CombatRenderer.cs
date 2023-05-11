
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatRenderer : MonoBehaviour
{
    [SerializeField] private CombatRenderPipeline renderPipeline;
    [SerializeField] private TMP_Text display;

    private readonly Queue<CombatState> stateUpdateQueue = new();

    void Start()
    {
        renderPipeline = new();
        renderPipeline.AddPass(new RoomRenderPass());
        renderPipeline.AddPass(new TileModifierRenderPass());
        renderPipeline.AddPass(new CombatActorRenderPass());
        CombatManager.Instance.OnCombatStateChanged += stateUpdateQueue.Enqueue;
    }

    private const float DRAWRATE = 5f;
    private float lastDraw = 1f / -DRAWRATE;
    void Update()
    {
        if (stateUpdateQueue.Count == 0 || Time.time < lastDraw + 1f / DRAWRATE) return;
        lastDraw = Time.time;
        display.text = renderPipeline.DrawCombat(stateUpdateQueue.Dequeue());
    }    
}
