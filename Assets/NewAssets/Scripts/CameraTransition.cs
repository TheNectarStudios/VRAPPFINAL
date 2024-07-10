using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraTransition : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public float fadeDuration = 1.0f;

    private Image fadeImage;
    private bool isFading = false;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.enabled = false;
    }

    void Update()
    {
        // Switch cameras when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space) && !isFading)
        {
            StartCoroutine(TransitionCameras());
        }
    }

    IEnumerator TransitionCameras()
    {
        isFading = true;

        // Fade in
        fadeImage.enabled = true;
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Switch cameras
        firstPersonCamera.enabled = !firstPersonCamera.enabled;
        thirdPersonCamera.enabled = !thirdPersonCamera.enabled;

        // Fade out
        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.enabled = false;
        isFading = false;
    }
}
