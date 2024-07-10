using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraCont : MonoBehaviour
{
    public GameObject leftHandController;
    public GameObject rightHandController;
    public Transform head;
    public Transform cameraTransform;
    public float moveSpeed = 1f;
    public float rotateSpeed = 1f;

    void Update()
    {
        // Move camera up and down using left and right hand triggers
        float verticalLeftInput = Input.GetAxis("LeftTrigger") - Input.GetAxis("RightTrigger");
        cameraTransform.Translate(Vector3.up * verticalLeftInput * moveSpeed * Time.deltaTime);

        // Rotate camera based on head movement
        float yaw = head.rotation.eulerAngles.y;
        cameraTransform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Move camera based on right hand joystick
        float horizontalInput = Input.GetAxis("RightJoystickHorizontal");
        float verticalRightInput = Input.GetAxis("RightJoystickVertical");
        Vector2 joystickInput = new Vector2(horizontalInput, verticalRightInput);
        Vector3 moveDirection = new Vector3(joystickInput.x, 0f, joystickInput.y).normalized;
        cameraTransform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
