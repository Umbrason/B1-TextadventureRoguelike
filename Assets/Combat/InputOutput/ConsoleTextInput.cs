
using System;
using TMPro;
using UnityEngine;

public class ConsoleTextInput : SingletonBehaviour<ConsoleTextInput>
{
    [SerializeField] private TMP_InputField InputField;
    public static event Action<String> OnSubmitLine;


    public void ForceText(string text)
    {
        _prevText = InputField.text;
        InputField.SetTextWithoutNotify(text);
        InputField.interactable = false;
    }
    private string _prevText;
    public void StopForcingText()
    {
        InputField.SetTextWithoutNotify(_prevText);
        InputField.interactable = true;
        InputField.ActivateInputField();
    }

    public void SubmitLine(string line) => SubmitLine(line, true);
    public void SubmitLine(string line, bool clearsInput = true)
    {
        line = line.Trim(' ');
        ConsoleOutput.Println($">{line}");
        if (clearsInput)
        {
            InputField.SetTextWithoutNotify("");
            InputField.ActivateInputField();
            StopForcingText();
        }
        OnSubmitLine?.Invoke(line);
    }

    void Start()
    {
        InputField.ActivateInputField();
        InputField.onDeselect.AddListener(_ => InputField.ActivateInputField());
        InputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
        InputField.inputValidator = ScriptableObject.CreateInstance<CustomValidator>();
        InputField.onSubmit.AddListener(SubmitLine);
    }

    private class CustomValidator : TMP_InputValidator
    {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (ch >= 'a' && ch <= 'z' || ch == ' ' || ch >= 'A' && ch <= 'z' || ch >= '0' && ch <= '9')
            {
                text = text.Insert(pos, $"{ch}");
                pos++;
                return ch;
            }
            return (char)0;
        }
    }
}
