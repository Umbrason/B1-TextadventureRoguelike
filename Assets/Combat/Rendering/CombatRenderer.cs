
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatRenderer : MonoBehaviour
{
    [SerializeField] private CombatRenderPipeline renderPipeline;
    [SerializeField] private TMP_Text display;

    private readonly Queue<IReadOnlyCombatState> stateUpdateQueue = new();

    void Start()
    {
        renderPipeline = new();
        renderPipeline.AddPass(new RoomRenderPass());
        renderPipeline.AddPass(new TileModifierRenderPass());
        renderPipeline.AddPass(new CombatActorRenderPass());
        CombatManager.OnVisualUpdate += stateUpdateQueue.Enqueue;
        if (CombatManager.CombatLog != null) stateUpdateQueue.Enqueue(CombatManager.CombatLog.CurrentReadOnlyCombatState);
    }

    void OnDestroy()
    {
        CombatManager.OnVisualUpdate -= stateUpdateQueue.Enqueue;
    }

    private const float DRAWRATE = 5f;
    private float lastDraw = 1f / -DRAWRATE;
    void Update()
    {
        if (stateUpdateQueue.Count == 0 || Time.time < lastDraw + 1f / DRAWRATE) return;
        lastDraw = Time.time;
        display.text = renderPipeline.DrawCombat(stateUpdateQueue.Dequeue(), out var bufferWidth, out var bufferHeight);
        var xOffset = (1 - bufferWidth % 2) * -.5f;
        var yOffset = (1 - bufferHeight % 2) * -.5f;
        display.transform.position = new(xOffset, yOffset, transform.position.z);
    }
}
