using UnityEngine;

public class LocationSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    void Start()
    {
        RunManager.OnLocationUpdate += Play;
    }

    void OnDestroy()
    {
        RunManager.OnLocationUpdate -= Play;
    }

    private void Play()
    {
        Debug.Log("test");
        var AS = new GameObject("", typeof(AudioSource)).GetComponent<AudioSource>();
        DontDestroyOnLoad(AS.gameObject);
        AS.PlayOneShot(clip, AudioSlider.Volume * .5f);
        Destroy(AS.gameObject, clip.length);
    }
}
