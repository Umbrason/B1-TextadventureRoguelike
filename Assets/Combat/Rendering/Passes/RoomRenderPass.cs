using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomRenderPass : CombatRenderPipeline.IRenderPass
{
    private readonly Dictionary<RoomInfo.Tile, char> tileChars = new() {
        { RoomInfo.Tile.FLOOR, '·' },
        { RoomInfo.Tile.WALL, '#' },
        { RoomInfo.Tile.VOID, ' ' },
    };

    private readonly Dictionary<RoomInfo.Tile, Color> tileColors = new() {
        { RoomInfo.Tile.FLOOR, new(.3f,.3f,.3f, 1f) },
        { RoomInfo.Tile.WALL, new Color(120, 136, 138, 255) / 255f},
        { RoomInfo.Tile.VOID, Color.black },
    };

    private Dictionary<int, char> WallLookup = new()
    {
        {0b0000, '█'},
        {0b1111, '╬'},

        {0b1110, '╩'},
        {0b1101, '╣'},
        {0b1011, '╠'},
        {0b0111, '╦'},

        {0b1100, '╝'},
        {0b1010, '╚'},
        {0b1001, '║'},
        {0b0110, '═'},
        {0b0101, '╗'},
        {0b0011, '╔'},

        {0b1000, '╨'},
        {0b0100, '╡'},
        {0b0010, '╞'},
        {0b0001, '╥'},
    };

    public CombatRenderPipeline.IRenderPass.Priority PassPriority => CombatRenderPipeline.IRenderPass.Priority.FLOORPLAN;

    public void Execute(CombatRenderPipeline.IRenderPass.RenderingData data)
    {
        var room = data.CombatState.Room;
        var roomBuffer = new CombatRenderPipeline.CharBuffer(room.Width, room.Height);
        foreach (var tile in room.Tiles)
        {
            var tilePos = tile.Item1;
            var tileType = tile.Item2;
            var charPos = tilePos - room.MinCorner;
            if (tileType == RoomInfo.Tile.WALL)
            {
                var neighbourMask = (room[tilePos + new Vector2Int(0, 1)] == RoomInfo.Tile.WALL ? 0b1000 : 0) +
                                    (room[tilePos - new Vector2Int(1, 0)] == RoomInfo.Tile.WALL ? 0b0100 : 0) +
                                    (room[tilePos + new Vector2Int(1, 0)] == RoomInfo.Tile.WALL ? 0b0010 : 0) +
                                    (room[tilePos - new Vector2Int(0, 1)] == RoomInfo.Tile.WALL ? 0b0001 : 0);
                roomBuffer.chars[charPos.x, charPos.y] = WallLookup[neighbourMask];
            }
            else roomBuffer.chars[charPos.x, charPos.y] = tileChars[tileType];
            roomBuffer.colors[charPos.x, charPos.y] = tileColors[tileType];
        }
        CombatRenderPipeline.CharBuffer.Blit(roomBuffer, data.ScreenBuffer);
    }
}
