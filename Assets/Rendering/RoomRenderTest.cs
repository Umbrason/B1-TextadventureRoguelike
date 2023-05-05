
using TMPro;
using UnityEngine;

public class RoomRenderTest : MonoBehaviour
{
    [SerializeField] private CombatRenderPipeline renderPipeline;
    [SerializeField] private TextAsset roomLayout;
    [SerializeField] private TMP_Text display;
    void Start()
    {
        var combatState = new CombatState(new Room(roomLayout.text), new(), new());
        renderPipeline = new();
        renderPipeline.AddPass(new RoomRenderPass());
        display.text = renderPipeline.DrawCombat(combatState);
    }
}
