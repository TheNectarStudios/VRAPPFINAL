using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float verticalRotation = 0f;

    void Update()
    {
        // Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // Check if left mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            // Rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

            transform.Rotate(Vector3.up * mouseX);
            transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0f);
        }

        // Move the camera
        Vector3 move = transform.TransformDirection(moveDirection) * movementSpeed * Time.deltaTime;
        transform.position += move;
    }
}
