using UnityEngine;

public class HotSpotMovementThroughTeleportation : MonoBehaviour
{
    private Camera sphereCamera; // Reference to the sphere camera

    void Start()
    {
        // Find the camera with the tag "SphereCamera"
        GameObject cameraObject = GameObject.FindGameObjectWithTag("tempCam");
        if (cameraObject != null)
        {
            sphereCamera = cameraObject.GetComponent<Camera>();
            Debug.Log("SphereCamera found: " + sphereCamera.name);
        }
        else
        {
            Debug.LogError("Camera with tag 'SphereCamera' not found.");
        }

        // Ensure the hotspot has a collider
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Debug.Log("BoxCollider added to hotspot.");
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Hotspot clicked: " + gameObject.name);

        HotspotScript hotspotScript = GetComponent<HotspotScript>();
        if (hotspotScript != null)
        {
            Debug.Log("HotspotScript found on hotspot.");
            TeleportCamera(hotspotScript.connectedPanoramaIndex);
        }
        else
        {
            Debug.LogError("HotspotScript not found on the hotspot.");
        }
    }

    void TeleportCamera(int connectedPanoramaIndex)
    {
        if (sphereCamera != null)
        {
            Debug.Log("Teleporting camera to index: " + connectedPanoramaIndex);
            float newXPosition = connectedPanoramaIndex * 50f; // Calculate new position based on index
            Vector3 newPosition = new Vector3(newXPosition, sphereCamera.transform.position.y, sphereCamera.transform.position.z);
            sphereCamera.transform.position = newPosition;
            Debug.Log("Teleported to position: " + newPosition);
        }
        else
        {
            Debug.LogError("Sphere camera not assigned or found.");
        }
    }
}
