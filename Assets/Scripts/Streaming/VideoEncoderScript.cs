using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections;

public class VideoCaptureScript : MonoBehaviour
{
    private bool isCapturing = false;
    private float captureDuration = 5f; // Duration of video capture in seconds
    private string outputFilePath = "CapturedVideo.mp4"; // Output file path for the captured video
    private WebCamTexture webCamTexture;

    private void Start()
    {
        StartCapture();
    }

    private void StartCapture()
    {
        if (isCapturing)
            return;

        XRSettings.showDeviceView = false; // Hide device view during capture

        // Start video capture
        StartCoroutine(StartCaptureCoroutine());
    }

    private IEnumerator StartCaptureCoroutine()
    {
        isCapturing = true;

        // Start webcam texture
        webCamTexture = new WebCamTexture();
        webCamTexture.Play();

        // Wait for the specified duration
        yield return new WaitForSeconds(captureDuration);

        // Stop webcam texture
        webCamTexture.Stop();

        // Encode frames and save video
        EncodeAndSaveVideo();
        
        isCapturing = false;
    }

    private void EncodeAndSaveVideo()
    {
        // Implement video encoding and saving logic here
        Debug.Log("Video captured and saved.");
    }
}
