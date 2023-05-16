
using System;
using TMPro;
using UnityEngine;

public class ConsoleTextInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField InputField;
    public event Action<String> OnSubmitLine;

    void Start()
    {
        InputField.ActivateInputField();
        InputField.onDeselect.AddListener(_ => InputField.ActivateInputField());
        InputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
        InputField.inputValidator = ScriptableObject.CreateInstance<CustomValidator>();
        InputField.onSubmit.AddListener(val =>
        {
            val = val.Trim(' ');
            ConsoleOutput.Println($">{val}");
            InputField.SetTextWithoutNotify("");
            InputField.ActivateInputField();
            OnSubmitLine?.Invoke(val);
        });
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
