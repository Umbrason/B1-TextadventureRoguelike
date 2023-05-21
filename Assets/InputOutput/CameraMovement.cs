using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour, CameraControls.ICameraControlsMapActions
{
    Vector2 ActualPosition;

    private Camera cached_Camera;
    private Camera Camera => cached_Camera ??= GetComponent<Camera>();

    CameraControls input;
    void Start()
    {
        input = new CameraControls();
        input.CameraControlsMap.SetCallbacks(this);
        input.CameraControlsMap.Enable();
    }

    void OnDestroy()
    {
        input.Dispose();
    }

    Vector2 dragStartWorldPosition;
    Vector2 dragStartScreenPosition;
    bool dragging;
    public void OnDrag(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                dragStartScreenPosition = PointerPosition;
                dragStartWorldPosition = Camera.transform.position;
                dragging = true;
                break;
            case InputActionPhase.Canceled:
                dragging = false;
                break;
        }
    }

    Vector2 PointerPosition;
    public void OnMovePointer(InputAction.CallbackContext context)
    {
        PointerPosition = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (!dragging) return;
        var delta = dragStartScreenPosition - PointerPosition;
        var deltaWorldPosition = RoundPosition(Camera.ScreenToWorldPoint(delta) - Camera.ScreenToWorldPoint(new(0, 0)));
        Camera.transform.position = (Vector3)RoundPosition(dragStartWorldPosition + deltaWorldPosition) + Vector3.forward * -10;
    }

    private Vector2 RoundPosition(Vector2 position)
    {
        return (Vector2)Vector2Int.RoundToInt(position * 9) / 9f;
    }
}
