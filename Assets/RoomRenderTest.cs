
using UnityEngine;

public class RoomRenderTest : MonoBehaviour
{
    [SerializeField] private ASCIIRoomRenderer roomRenderer;
    [SerializeField] private TextAsset roomLayout;
    void Start()
    {
        var room = new Room(roomLayout.text);
        roomRenderer.Render(room);
    }
}
