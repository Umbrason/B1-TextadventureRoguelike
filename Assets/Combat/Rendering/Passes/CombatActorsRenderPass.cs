using System.Collections.Generic;
using UnityEngine;

public class CombatActorRenderPass : CombatRenderPipeline.IRenderPass
{
    public CombatRenderPipeline.IRenderPass.Priority PassPriority => CombatRenderPipeline.IRenderPass.Priority.COMBATACTORS;
    private Dictionary<int, Color> AlignmentColors;

    public void Execute(CombatRenderPipeline.IRenderPass.RenderingData data)
    {
        var actorBuffer = new CombatRenderPipeline.CharBuffer(data.ScreenBuffer.Width, data.ScreenBuffer.Height);
        actorBuffer.Clear(clearColor: new(0, 0, 0, 0));
        var roomMinCorner = data.CombatState.Room.MinCorner;        
        foreach (var actor in data.CombatState.CombatActors.Values)
        {
            var screenPosition = actor.Position - roomMinCorner;
            actorBuffer.chars[screenPosition.x, screenPosition.y] = 'O';
            actorBuffer.colors[screenPosition.x, screenPosition.y] = actor.Alignment == 0 ? Color.green : Color.red;
        }
        CombatRenderPipeline.CharBuffer.Blit(actorBuffer, data.ScreenBuffer);
    }
}