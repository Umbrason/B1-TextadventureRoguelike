
using UnityEngine;

public class IWinButton : MonoBehaviour
{
    public void Win()
    {
        RunManager.EndRun(true);
    }
}
