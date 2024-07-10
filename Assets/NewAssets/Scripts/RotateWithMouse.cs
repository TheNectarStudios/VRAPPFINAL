using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    public float rotationSpeed = 1.5f;

    private bool isRotating = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        // Check for right mouse button press
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }

        // Check for right mouse button release
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        // Rotate object if right mouse button is held down
        if (isRotating)
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;

            // Rotate around the Y-axis
            transform.Rotate(Vector3.up, -deltaMousePosition.x * rotationSpeed, Space.World);

            // Optionally, rotate around other axes as well if desired
            // transform.Rotate(Vector3.right, deltaMousePosition.y * rotationSpeed, Space.World);

            lastMousePosition = Input.mousePosition;  
        }
    }
}