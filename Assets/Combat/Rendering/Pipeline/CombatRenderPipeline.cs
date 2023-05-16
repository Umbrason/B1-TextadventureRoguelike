using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatRenderPipeline
{
    public class CharBuffer
    {
        public int Width => chars.GetLength(0);
        public int Height => chars.GetLength(1);
        public readonly char[,] chars;
        public readonly Color[,] colors;
        public CharBuffer(int w, int h)
        {
            this.chars = new char[w, h];
            this.colors = new Color[w, h];
        }

        public void Clear(char clearValue = ' ', Color clearColor = default)
        {
            var tw = Width;
            var th = Height;
            for (int w = 0; w < tw; w++)
                for (int h = 0; h < th; h++)
                {
                    chars[w, h] = clearValue;
                    colors[w, h] = clearColor;
                }
        }


        public static void Blit(CharBuffer src, CharBuffer dest) => Blit(src, dest, 0, 0);
        public static void Blit(CharBuffer src, CharBuffer dest, int destOffsetX, int destOffsetY) => Blit(src, 0, 0, src.Width, src.Height, dest, 0, 0);
        public static void Blit(CharBuffer src, int srcOffsetX, int srcOffsetY, int srcWidth, int srcHeight, CharBuffer dest, int destOffsetX, int destOffsetY)
        {
            if (src.Width < srcOffsetX + srcWidth ||
                src.Height < srcOffsetY + srcHeight ||
                dest.Width < srcWidth + destOffsetX ||
                dest.Height < srcHeight + destOffsetY) throw new Exception("Could not Blit: buffer size mismatch!");
            for (int x = 0; x < srcWidth; x++)
                for (int y = 0; y < srcHeight; y++)
                {
                    var srcChar = src.chars[x + srcOffsetX, y + srcOffsetY];
                    var srcCol = src.colors[x + srcOffsetX, y + srcOffsetY];
                    if (srcCol.a > 0.5f) dest.chars[x + destOffsetX, y + destOffsetY] = srcChar;
                    var destColor = dest.colors[x + destOffsetX, y + destOffsetY];
                    dest.colors[x + destOffsetX, y + destOffsetY] = Color.Lerp(destColor, srcCol, srcCol.a);
                }
        }

        //huge unoptimized bottleneck still runs in 4ms
        public override string ToString()
        {
            var screenText = "";
            for (int j = Height - 1; j >= 0; j--)
            {
                for (int i = 0; i < Width; i++)
                    screenText += $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(colors[i, j])}>{(chars[i, j] != default ? chars[i, j] : " ")}</color>";
                screenText += "\n";
            }
            return screenText;
        }
    }

    private readonly SortedList<int, IRenderPass> passes = new();

    public void AddPass(IRenderPass pass)
    {
        passes.Add((int)pass.PassPriority, pass);
    }

    public string DrawCombat(IReadOnlyCombatState state, out int bufferWidth, out int bufferHeight)
    {
        bufferWidth = state.Room.Width;
        bufferHeight = state.Room.Height;
        var ScreenBuffer = new CharBuffer(bufferWidth, bufferHeight);
        foreach (var pass in passes.Values)
        {
            var renderData = new IRenderPass.RenderingData(state, ScreenBuffer);
            pass.Execute(renderData);
        }
        return ScreenBuffer.ToString();
    }

    public interface IRenderPass
    {
        public struct RenderingData
        {
            public IReadOnlyCombatState CombatState { get; }
            public CharBuffer ScreenBuffer { get; }
            public RenderingData(IReadOnlyCombatState state, CharBuffer screenBuffer)
            {
                this.ScreenBuffer = screenBuffer;
                this.CombatState = state;
            }
        }
        public enum Priority
        {
            FLOORPLAN = 0, TILEMODIFIER = 1000, COMBATACTORS = 2000
        }
        public Priority PassPriority { get; }
        public void Execute(RenderingData data);
    }
}
