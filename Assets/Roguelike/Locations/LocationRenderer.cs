using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationRenderer : MonoBehaviour
{
    [SerializeField] private LocationOptionText locationOptionTextTemplate;
    [SerializeField] private Transform LocationOptionsContainer;
    [SerializeField] private Image LocationImage;
    [SerializeField] private TMP_Text storyText;


    void Start()
    {
        RunManager.OnLocationUpdate += Refresh;
        Refresh();
    }

    private readonly Queue<GameObject> optionInstances = new();
    private void Refresh()
    {
        while (optionInstances.Count > 0) Destroy(optionInstances.Dequeue());
        var location = RunManager.ReadOnlyRunInfo.CurrentLocation;
        storyText.text = location.storyText;
        LocationImage.sprite = Resources.Load<Sprite>($"LocationImages/{location.LocationImage}");
        foreach (var option in location.optionTexts)
        {
            var instance = Instantiate(locationOptionTextTemplate, LocationOptionsContainer);
            instance.OptionString = option;
            optionInstances.Enqueue(instance.gameObject);
        }
    }

    void OnDestroy()
    {
        RunManager.OnLocationUpdate -= Refresh;
    }
}
