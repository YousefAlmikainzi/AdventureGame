using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    float pitch = 0.0f;
    [SerializeField] float sensitivity = 200.0f;
    Vector2 cameraInputs;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        cameraMovment();
    }
    void cameraMovment()
    {
        cameraInputs.x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        Vector3 lookUp = new Vector3(0,1,0);
        transform.Rotate(Vector3.up * cameraInputs.x, Space.World);

        cameraInputs.y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        pitch -= cameraInputs.y;
        pitch = Mathf.Clamp(pitch, -50.0f, 35.0f);
        Vector3 currentAngle = transform.localEulerAngles;
        currentAngle.x = pitch;
        currentAngle.z = 0;
        transform.localEulerAngles = currentAngle;
    }
}
