
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class ConsoleTextInput : MonoBehaviour
{
    private TMP_InputField cached_inputField;
    private TMP_InputField InputField => cached_inputField ??= GetComponent<TMP_InputField>();
    public event Action<String> Submit = Debug.Log;

    void Start()
    {
        InputField.ActivateInputField();
        InputField.onDeselect.AddListener(_ => InputField.ActivateInputField());
        InputField.onSubmit.AddListener(val =>
        {
            Submit?.Invoke(val);
            InputField.SetTextWithoutNotify("");
            InputField.ActivateInputField();
        });
    }
}
