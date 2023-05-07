
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GridAlignTextScreen : MonoBehaviour
{
    private TMP_Text cached_text;
    private TMP_Text Text => cached_text ??= GetComponent<TMP_Text>();
    private void Update()
    {
        var room = CombatManager.Instance?.CombatLog?.CurrentState.Room;
        if (room == null) return;
        var oddWidth = room.Width % 2 == 1;
        var oddHeight = room.Height % 2 == 1;
        transform.position = new(oddWidth ? 0 : -.5f, oddHeight ? 0 : -.5f, transform.position.z);
    }
}
