
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
        InputField.onSubmit.AddListener(val =>
        {
            OnSubmitLine?.Invoke(val);
            InputField.SetTextWithoutNotify("");
            InputField.ActivateInputField();
        });
    }
}
