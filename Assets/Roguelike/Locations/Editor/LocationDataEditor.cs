using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(LocationData))]
public class LocationDataEditor : Editor
{
    private ReorderableList optionsList;

    private const float LineHeight = 18f;
    private const float Padding = 5f;

    private GUIStyle textAreaStyle;

    private void OnEnable()
    {
        // Create the reorderable list for StoryTexts
        textAreaStyle = new GUIStyle(EditorStyles.textArea);
        textAreaStyle.wordWrap = true;

        // Create the reorderable list for Options
        optionsList = new ReorderableList(serializedObject, serializedObject.FindProperty("Options"), true, true, true, true);
        optionsList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Options");
        optionsList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            SerializedProperty element = optionsList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element, GUIContent.none);
        };
    }

    private float GetTextAreaHeight(string content)
    {
        float contentHeight = textAreaStyle.CalcHeight(new GUIContent(content), EditorGUIUtility.currentViewWidth - Padding * 2);
        float lines = Mathf.Max(contentHeight / LineHeight, 1);
        float height = lines * LineHeight + Padding * 2;
        return height;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty spriteProperty = serializedObject.FindProperty("Sprite");
        EditorGUILayout.PropertyField(spriteProperty);

        // Draw the reorderable lists
        EditorGUILayout.Space();
        serializedObject.FindProperty("StoryText").stringValue = EditorGUILayout.TextArea(serializedObject.FindProperty("StoryText").stringValue);
        EditorGUILayout.Space();
        optionsList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}