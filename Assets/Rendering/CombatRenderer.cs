
using TMPro;
using UnityEngine;

public class CombatRenderer : MonoBehaviour
{
    [SerializeField] private CombatRenderPipeline renderPipeline;
    [SerializeField] private TMP_Text display;

    void Start()
    {
        renderPipeline = new();
        renderPipeline.AddPass(new RoomRenderPass());
        renderPipeline.AddPass(new TileModifierRenderPass());
        renderPipeline.AddPass(new CombatActorRenderPass());
        CombatManager.Instance.CombatLog.OnStateChanged += DrawCombatState;
        DrawCombatState(CombatManager.Instance.CombatLog.CurrentState);
    }

    private void DrawCombatState(CombatState state)
    {
        display.text = renderPipeline.DrawCombat(state);
    }
}
