public class TileModifierRenderPass : CombatRenderPipeline.IRenderPass
{
    public CombatRenderPipeline.IRenderPass.Priority PassPriority => CombatRenderPipeline.IRenderPass.Priority.TILEMODIFIER;

    public void Execute(CombatRenderPipeline.IRenderPass.RenderingData data)
    {
        var tileModifierBuffer = new CombatRenderPipeline.CharBuffer(data.ScreenBuffer.Width, data.ScreenBuffer.Height);        
        var roomMinCorner = data.CombatState.Room.MinCorner;
        foreach(var tileModifier in data.CombatState.Room.TileModifiers)
        {
            var pos = tileModifier.Key;
            var screenPos = pos - roomMinCorner;
            var modifier = tileModifier.Value;
            tileModifierBuffer.colors[screenPos.x, screenPos.y] = modifier.Color;
            tileModifierBuffer.chars[screenPos.x, screenPos.y] = modifier.Char;
        }
        CombatRenderPipeline.CharBuffer.Blit(tileModifierBuffer, data.ScreenBuffer);
    }
}