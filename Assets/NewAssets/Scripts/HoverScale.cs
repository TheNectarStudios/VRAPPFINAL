using UnityEngine;

public class HoverScale : MonoBehaviour
{
    // Variables for original scale and hover scale
    private Vector3 originalScale;
    public float hoverScaleFactor = 1.5f;

    // Reference to the parent GameObject containing the camera
    public GameObject parentObject;

    // Reference to the target GameObject to move to
    public GameObject targetObject;

    void Start()
    {
        // Store the original scale
        originalScale = transform.localScale;
    }

    void OnMouseEnter()
    {
        // When the mouse enters the sphere, scale it up
        transform.localScale = originalScale * hoverScaleFactor;
    }

    void OnMouseExit()
    {
        // When the mouse exits the sphere, reset its scale
        transform.localScale = originalScale;
    }

    void OnMouseDown()
    {
        // When the sphere is clicked, teleport the parent object
        if (parentObject != null && targetObject != null)
        {
            // Move the parent object to the position of the target object
            parentObject.transform.position = targetObject.transform.position;
        }
    }
}
