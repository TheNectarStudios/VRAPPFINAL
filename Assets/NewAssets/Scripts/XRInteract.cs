using UnityEngine;

public class XRInteract : MonoBehaviour
{
    private int clickCounter = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            clickCounter++;
        }

        if (clickCounter > 0)
        {
            HandleClickCounter();
        }
    }

    void HandleClickCounter()
    {
        // Find all objects with the SphereClickHandler and call OnSphereClicked
        SphereClickHandler[] handlers = FindObjectsOfType<SphereClickHandler>();
        foreach (SphereClickHandler handler in handlers)
        {
            handler.OnSphereClicked();
        }

        clickCounter = 0; // Reset click counter after handling
    }
}
