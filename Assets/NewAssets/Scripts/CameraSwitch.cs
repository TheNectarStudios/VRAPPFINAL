using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public MonoBehaviour firstPersonScript;
    public MonoBehaviour thirdPersonScript;
    public Animator fadingAnimator;
    private float transitionDelay = 0.5f;

    void Start()
    {
        // Ensure one camera is enabled and the other is disabled at the start
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        
        // Ensure the corresponding scripts are enabled/disabled accordingly
        firstPersonScript.enabled = true;
        thirdPersonScript.enabled = false;
    }

    void Update()
    {
        // Switch cameras when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SwitchCameraWithDelay());
        }
    }

    IEnumerator SwitchCameraWithDelay()
    {
        // Play fading animation
        if (fadingAnimator != null)
        {
            fadingAnimator.SetTrigger("Fade");
        }

        // Wait for the fading animation to finish
        yield return new WaitForSeconds(0.5f);

        // Toggle camera and corresponding script
        firstPersonCamera.enabled = !firstPersonCamera.enabled;
        thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
        firstPersonScript.enabled = !firstPersonScript.enabled;
        thirdPersonScript.enabled = !thirdPersonScript.enabled;
    }
}
