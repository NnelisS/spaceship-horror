using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 1;
    [SerializeField, Range(0.0f, 0.5f)] float smoothTime = 0.03f;

    Vector2 currentDelta = Vector2.zero;
    Vector2 currentDeltaVelocity = Vector2.zero;

    float cameraPitch = 0.0f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateCamera(Vector2 mouseDelta)
    {
        currentDelta = Vector2.SmoothDamp(currentDelta, mouseDelta, ref currentDeltaVelocity, smoothTime);

        cameraPitch -= mouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }
}
