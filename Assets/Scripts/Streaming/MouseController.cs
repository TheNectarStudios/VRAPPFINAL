using UnityEngine;

public class MouseController : MonoBehaviour  
{
    public float mouseSensitivity = 100f; // Sensitivity of mouse movement 

    private float xRotation = 0f; // Store the vertical rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // Check if the right mouse button is held down
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY; // Adjust the vertical rotation
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the vertical rotation to avoid flipping

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Apply vertical rotation to the camera
            transform.Rotate(Vector3.up * mouseX); // Apply horizontal rotation to the camera
        }
    }
}
