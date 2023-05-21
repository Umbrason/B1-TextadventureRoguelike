
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    void Start()
    {
        text.text = (RunManager.ReadOnlyRunInfo.Won ? "YOU WON!" : "GAME OVER") + "\n<size=50%>-Press any key to continue-<size=50%>";
    }

    void Update()
    {

        if (Input.anyKeyDown)
        {
            RunManager.Reset();
            SceneManager.LoadScene(0);
        }
    }
}
