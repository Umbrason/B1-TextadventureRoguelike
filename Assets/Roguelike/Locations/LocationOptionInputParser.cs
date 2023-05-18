
using System;
using UnityEngine;

public class LocationOptionInputParser : MonoBehaviour
{
    public void Start()
    {
        ConsoleTextInput.OnSubmitLine += Parse;
    }

    public void OnDestroy()
    {
        ConsoleTextInput.OnSubmitLine -= Parse;
    }
    void Parse(string line)
    {
        var options = RunManager.ReadOnlyRunInfo.CurrentLocation.optionTexts;
        for (int i = 0; i < options.Length; i++)
        {
            if (string.Compare(options[i], line, StringComparison.InvariantCultureIgnoreCase) != 0) continue;
            RunManager.ChooseLocationOption(i);
            break;
        }
    }
}
