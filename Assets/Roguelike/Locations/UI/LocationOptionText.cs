using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LocationOptionText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button cached_Button;
    private Button Button => cached_Button ??= GetComponent<Button>();

    private TMP_Text cached_Text;
    private TMP_Text Text => cached_Text ??= GetComponent<TMP_Text>();

    private string optionString;
    public string OptionString
    {
        set
        {
            optionString = value;
            Text.text = $">{value}";
        }
    }

    void Start()
    {
        Button.onClick.AddListener(() => ConsoleTextInput.Instance.SubmitLine(this.optionString));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ConsoleTextInput.Instance.ForceText(optionString);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ConsoleTextInput.Instance.StopForcingText();
    }
}
