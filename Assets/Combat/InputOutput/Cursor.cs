
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class Cursor : MonoBehaviour
{
    private Camera cached_mainCamera;
    private Camera MainCamera => cached_mainCamera ??= Camera.main;
    private SpriteRenderer cached_renderer;
    private SpriteRenderer Renderer => cached_renderer ??= GetComponent<SpriteRenderer>();

    [SerializeField] private Sprite[] Images;
    [SerializeField] private float animationFrequency = 2f;
    [SerializeField] private TMP_Text coordinateText;

    private RoomInfo room;
    void Start()
    {
        CombatManager.OnCombatStateChanged += (state) => room = state.Room;
    }

    void OnDestroy()
    {
        CombatManager.OnCombatStateChanged -= (state) => room = state.Room;
    }

    void Update()
    {
        if (!(coordinateText.enabled = Renderer.enabled = (room != null))) return; //disable cursor and return if room is null
        var cursorWorldPosition = Vector2Int.RoundToInt(MainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue()));
        transform.position = (Vector2)cursorWorldPosition;
        var cursorRoomPosition = cursorWorldPosition + room.Size / 2 + room.MinCorner;
        var tileType = room[cursorRoomPosition];
        if (UnityEngine.Cursor.visible = !(coordinateText.enabled = Renderer.enabled = (tileType == RoomInfo.Tile.FLOOR))) return; //disable if not hovering over floor tile
        if (Images.Length > 0) Renderer.sprite = Images[Mathf.FloorToInt(Time.time * animationFrequency) % Images.Length];
        if(coordinateText != null) coordinateText.text = $"<mark=#000000aa>x:{cursorRoomPosition.x} y:{cursorRoomPosition.y}";
    }
}
