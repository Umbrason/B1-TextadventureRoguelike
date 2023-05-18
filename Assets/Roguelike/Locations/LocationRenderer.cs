using TMPro;
using UnityEngine;

public class LocationRenderer : MonoBehaviour
{
    [SerializeField] private LocationOptionText locationOptionTextTemplate;
    [SerializeField] private Transform LocationOptionsContainer;
    [SerializeField] private TMP_Text storyText;
    void Start()
    {
        var location = RunManager.ReadOnlyRunInfo.CurrentLocation;
        storyText.text = location.storyText;
        foreach(var option in location.optionTexts)
        {
            var instance = Instantiate(locationOptionTextTemplate, LocationOptionsContainer);
            instance.OptionString = option;
        }
    }
}
