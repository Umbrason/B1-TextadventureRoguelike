
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbandonButton : MonoBehaviour
{
    public void Abandon()
    {
        RunManager.Reset();
        SceneManager.LoadScene(0);
    }
}
