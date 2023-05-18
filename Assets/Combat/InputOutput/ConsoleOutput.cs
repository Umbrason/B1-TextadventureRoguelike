using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleOutput : SingletonBehaviour<ConsoleOutput>
{
    private static readonly List<string> history = new();
    [SerializeField] private TMP_Text outputText;    

    public static void Println(string text)
    {
        history.Add(text);
        Instance?.UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if(outputText == null) return;
        outputText.text = string.Join('\n', history);
    }
}
