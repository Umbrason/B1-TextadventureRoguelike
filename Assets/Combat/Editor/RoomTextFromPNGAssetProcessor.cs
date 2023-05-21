using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RoomTextFromPNGAssetProcessor : AssetPostprocessor
{
    private const string SubAssetSuffix = "_text";

    private void OnPostprocessTexture(Texture2D texture)
    {
        if (!assetPath.EndsWith("room.png", true, CultureInfo.InvariantCulture))
            return;

        string textFilePath = assetPath.Replace(".png", SubAssetSuffix + ".txt");
        string textData = ConvertPNGToText(texture);

        var file = File.CreateText(textFilePath);
        file.Write(textData);
        file.Close();
        AssetDatabase.ImportAsset(textFilePath);
    }

    private string ConvertPNGToText(Texture2D texture)
    {

        var tiles = new Dictionary<Vector2Int, RoomInfo.Tile>();
        for (int x = 0; x < texture.width; x++)
            for (int y = 0; y < texture.height; y++)
            {
                var color = texture.GetPixel(x, y);
                if (color.a <= 0) continue;
                tiles[new(x, y)] = color.r > .5f ? RoomInfo.Tile.FLOOR : RoomInfo.Tile.WALL;
            }
        if (tiles.Count == 0) return "";
        var minCorner = tiles.Keys.Aggregate((a, b) => new(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y)));
        var maxCorner = tiles.Keys.Aggregate((a, b) => new(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y)));
        var size = maxCorner - minCorner + Vector2Int.one;
        char[,] roomChars = new char[size.x, size.y];
        for (int x = 0; x < roomChars.GetLength(0); x++)
            for (int y = 0; y < roomChars.GetLength(1); y++)
                roomChars[x, y] = ' ';
        foreach (var tile in tiles)
        {
            var pos = tile.Key - minCorner;
            var ch = tile.Value switch
            {
                RoomInfo.Tile.FLOOR => 'F',
                RoomInfo.Tile.WALL => 'W',
                _ => ' ',
            };
            roomChars[pos.x, pos.y] = ch;
        }
        var output = "";
        for (int y = roomChars.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < roomChars.GetLength(0); x++)
                output += roomChars[x, y];
            output += '\n';
        }
        return output;
    }
}