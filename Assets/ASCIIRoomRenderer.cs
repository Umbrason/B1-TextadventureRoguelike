using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ASCIIRoomRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private readonly Dictionary<Room.Tile, char> tileChars = new() {
        { Room.Tile.FLOOR, '·' },
        { Room.Tile.WALL, '#' },
        { Room.Tile.VOID, ' ' },
    };

    #region wall chars
    //_0_
    //1X1 ═
    //_0_

    //_1_
    //0X0 ║
    //_1_

    //_1_
    //1X1 ╬
    //_1_

    //_0_
    //0X1 ╔
    //_1_

    //_0_
    //1X0 ╗
    //_1_

    //_1_
    //1X0 ╝
    //_0_

    //_1_
    //0X1 ╚
    //_0_

    //_1_
    //0X1 ╠
    //_1_

    //_1_
    //1X0 ╣
    //_1_    

    //_0_
    //1X1 ╦
    //_1_

    //_1_
    //1X1 ╩
    //_0_

    //_0_
    //0X1 ╞
    //_0_

    //_1_
    //0X0 ╥
    //_0_

    //_0_
    //1X0 ╞
    //_0_

    //_0_
    //0X0 ╡
    //_1_

    //_0_
    //1X1 ═
    //_0_

    //_1_
    //0X0 ║
    //_1_

    //_1_
    //1X1 ╬
    //_1_

    //_0_
    //0X1 ╔
    //_1_

    //_0_
    //1X0 ╗
    //_1_

    //_1_
    //1X0 ╝
    //_0_

    //_1_
    //0X1 ╚
    //_0_

    //_1_
    //0X1 ╠
    //_1_

    //_1_
    //1X0 ╣
    //_1_    

    //_0_
    //1X1 ╦
    //_1_

    //_1_
    //1X1 ╩
    //_0_

    //_0_
    //0X1 ╞
    //_0_

    //_1_
    //0X0 ╥
    //_0_

    //_0_
    //1X0 ╞
    //_0_

    //_0_
    //0X0 ╡
    //_1_
    #endregion

    private Dictionary<int, char> WallLookup = new()
    {
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

    string GenerateString(Room room)
    {
        var roomChars = new char[room.Width, room.Height];
        foreach (var tile in room.Tiles)
        {
            var tilePos = tile.Item1;
            var tileType = tile.Item2;
            var stringPos = tilePos - room.MinCorner;
            if (tileType == Room.Tile.WALL)
            {
                var neighbourMask = (room[tilePos + new Vector2Int(0, 1)] == Room.Tile.WALL ? 0b1000 : 0) +
                                    (room[tilePos - new Vector2Int(1, 0)] == Room.Tile.WALL ? 0b0100 : 0) +
                                    (room[tilePos + new Vector2Int(1, 0)] == Room.Tile.WALL ? 0b0010 : 0) +
                                    (room[tilePos - new Vector2Int(0, 1)] == Room.Tile.WALL ? 0b0001 : 0);
                Debug.Log(neighbourMask.ToBinaryString());
                roomChars[stringPos.x, stringPos.y] = WallLookup[neighbourMask];
            }
            else roomChars[stringPos.x, stringPos.y] = tileChars[tileType];
        }

        var roomText = "";
        for (int j = room.Height - 1; j >= 0; j--)
        {
            for (int i = 0; i < room.Width; i++)
            {
                roomText += roomChars[i, j] != default ? roomChars[i, j] : " ";
            }
            roomText += "\n";
        }
        return roomText;
    }

    public void Render(Room room)
    {
        text.text = GenerateString(room);
    }
}
