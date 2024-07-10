using UnityEngine;

public class CameraFollowSpectating : MonoBehaviour
{
    private Transform mainPlayerTransform; // Reference to the main player's transform

    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, -3f); // Offset from the target object
    [SerializeField] private float smoothSpeed = 10f; // Smoothing speed for camera movement
    [SerializeField] private float mouseSensitivity = 100f; // Sensitivity of mouse movement

    private float xRotation = 0f; // Store the vertical rotation
    private bool isRightMouseButtonPressed = false; // Track right mouse button state

    private void Start()
    {
        // Find the player with the tag "Player" and set it as the main player's transform
        GameObject mainPlayer = GameObject.FindGameObjectWithTag("Player");
        if (mainPlayer != null)
        {
            mainPlayerTransform = mainPlayer.transform;
        }
        else
        {
            Debug.LogWarning("No object with tag 'Player' found!");
        }

        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    private void Update()
    {
        // Check if the right mouse button is pressed or released
        if (Input.GetMouseButtonDown(1))
        {
            isRightMouseButtonPressed = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButtonPressed = false;
        }

        // Handle mouse look
        if (isRightMouseButtonPressed)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY; // Adjust the vertical rotation
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the vertical rotation to avoid flipping

            // Apply vertical rotation to the camera
            transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, 0f);
            // Apply horizontal rotation to the camera
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    private void LateUpdate()
    {
        if (mainPlayerTransform != null)
        {
            // Calculate the desired position of the camera
            Vector3 desiredPosition = mainPlayerTransform.position + offset;
            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            if (!isRightMouseButtonPressed)
            {
                // Smoothly rotate the camera to look at the main player when the mouse is not pressed
                Quaternion desiredRotation = Quaternion.LookRotation(mainPlayerTransform.position - transform.position);
                Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed * Time.deltaTime);
                transform.rotation = smoothedRotation;
            }
        }
    }
}
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.XR.Interaction.Toolkit;

// public class CameraFollowSpectating : MonoBehaviour
// {
//     public InputActionReference rotateAction; // Reference to the rotate action from the Input Actions asset
//     public float rotationSpeed = 45f; // Speed of rotation

//     private void OnEnable()
//     {
//         rotateAction.action.Enable();
//     }

//     private void OnDisable()
//     {
//         rotateAction.action.Disable();
//     }

//     void Update()
//     {
//         // Get the rotation input from the VR controller
//         Vector2 rotationInput = rotateAction.action.ReadValue<Vector2>();

//         // Apply rotation based on the input
//         RotateCamera(rotationInput);
//     }

//     void RotateCamera(Vector2 rotationInput)
//     {
//         // Calculate rotation amount
//         float yaw = rotationInput.x * rotationSpeed * Time.deltaTime;
//         float pitch = rotationInput.y * rotationSpeed * Time.deltaTime;

//         // Apply rotation to the camera
//         transform.Rotate(Vector3.up, yaw, Space.World);
//         transform.Rotate(Vector3.right, -pitch, Space.Self);
//     }
// }
