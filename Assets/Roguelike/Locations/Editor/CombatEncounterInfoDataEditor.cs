using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombatEncounterInfoData))]
public class CombatEncounterInfoDataEditor : Editor
{
    private CombatEncounterInfoData encounterInfoData;

    private void OnEnable()
    {
        encounterInfoData = (CombatEncounterInfoData)target;
    }

    int size = 10;
    bool editOn = false;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        // Check if roomLayoutFile is assigned

        encounterInfoData.roomLayoutFile = (TextAsset)EditorGUILayout.ObjectField(encounterInfoData.roomLayoutFile, typeof(TextAsset), allowSceneObjects: false);
        if (GUI.changed) EditorUtility.SetDirty(encounterInfoData);
        if (encounterInfoData.roomLayoutFile != null)
        {
            GUI.color = editOn ? Color.gray : Color.white;
            if (GUILayout.Button("Edit"))
                editOn = !editOn;
            GUI.color = Color.white;
            DisplayRoomLayout();
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a TextAsset to roomLayoutFile.", MessageType.Warning);
        }
        if (encounterInfoData.AllyStartPositions.Length < 6)
            EditorGUILayout.HelpBox("At least 6 allied spawn positions should be placed", MessageType.Warning);
        Array.Resize(ref encounterInfoData.EnemyTypes, encounterInfoData.EnemyStartPositions.Length);
        for (int i = 0; i < encounterInfoData.EnemyTypes.Length; i++)
            encounterInfoData.EnemyTypes[i] = DrawEnemyDropdown($"Enemy {i}", encounterInfoData.EnemyTypes[i]);
    }

    private string DrawEnemyDropdown(string label, string type)
    {
        var options = IBinarySerializableFactory<ICombatActor>.Types.Where(type => type.Value != typeof(PlayableCombatActor)).Select(type => type.Value.Name).ToArray();
        var index = Mathf.Max(0, System.Array.IndexOf(options, type));
        return options[Mathf.Clamp(EditorGUILayout.Popup(label, index, options), 0, options.Length - 1)];
    }

    private GUIStyle GUIStyle
    {
        get
        {
            var style = new GUIStyle(GUIStyle.none);
            style.margin = new(0, 0, 0, 0);
            style.normal.background = Texture2D.whiteTexture;
            style.fontSize = size;
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }
    }

    private Vector2Int minCorner;
    private void DisplayRoomLayout()
    {
        EditorGUILayout.LabelField("Room Layout:");

        size = EditorGUILayout.IntSlider("zoom:", size, 4, 20);
        string[] lines = encounterInfoData.roomLayoutFile.text.Split("#")[0].Split('\n');

        minCorner = new Vector2Int(int.MaxValue, int.MaxValue);
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            if (line.All(c => c == ' ')) continue;
            minCorner = new(minCorner.x, Mathf.Min(y, minCorner.y));
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == ' ') continue;
                minCorner = new(Mathf.Min(x, minCorner.x), minCorner.y);
            }
        }

        // Display the room layout
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            GUILayout.BeginHorizontal();
            for (int j = 0; j < line.Length; j++)
            {
                char tile = line[j];
                switch (tile)
                {
                    case ' ':
                        GUILayout.Space(size);
                        break;
                    case 'F':
                        DrawTileButton(j, lines.Length - i - 1);
                        break;
                    case 'W':
                        GUI.backgroundColor = Color.black;
                        GUILayout.Box("", style: GUIStyle, GUILayout.Width(size), GUILayout.Height(size));
                        GUI.backgroundColor = Color.white;
                        break;
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '0':
                        GUILayout.Label($"{tile}", style: GUIStyle, GUILayout.Width(size), GUILayout.Height(size));
                        break;
                }
            }
            GUILayout.EndHorizontal();
        }
    }

    private void DrawTileButton(int x, int y)
    {
        Vector2Int tilePosition = new Vector2Int(x, y) - minCorner;
        bool isAlly = IsPositionAssigned(encounterInfoData.AllyStartPositions ??= new Vector2Int[0], tilePosition, out var allyIndex);
        bool isEnemy = IsPositionAssigned(encounterInfoData.EnemyStartPositions ??= new Vector2Int[0], tilePosition, out var enemyIndex);

        GUI.backgroundColor = isAlly ? Color.green : isEnemy ? Color.red : Color.white;
        string text = isAlly ? $"{allyIndex}" : isEnemy ? $"{enemyIndex}" : "";
        if (GUILayout.Button(text, style: GUIStyle, GUILayout.Width(size), GUILayout.Height(size)) && editOn)
        {
            if (isAlly)
            {
                encounterInfoData.AllyStartPositions = encounterInfoData.AllyStartPositions.Where(p => p.x != tilePosition.x || p.y != tilePosition.y).ToArray();
                encounterInfoData.EnemyStartPositions = encounterInfoData.EnemyStartPositions.Append(tilePosition).ToArray();
            }
            else if (isEnemy) encounterInfoData.EnemyStartPositions = encounterInfoData.EnemyStartPositions.Where(p => p.x != tilePosition.x || p.y != tilePosition.y).ToArray();
            else encounterInfoData.AllyStartPositions = encounterInfoData.AllyStartPositions.Append(tilePosition).ToArray();
            EditorUtility.SetDirty(encounterInfoData);
        }

        GUI.backgroundColor = Color.white;
    }

    private bool IsPositionAssigned(Vector2Int[] positions, Vector2Int position, out int index)
    {
        index = System.Array.IndexOf(positions, position);
        return index != -1;
    }
}