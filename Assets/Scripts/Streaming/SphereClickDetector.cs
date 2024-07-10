using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SphereClickDetector : MonoBehaviour
{
    public XRRayInteractor rayInteractor; // Assign the ray interactor from the VR controller
    public LayerMask interactableLayerMask; // Layer mask to filter only interactable objects

    void Update()
    {
        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Sphere"))
            {
                SphereClickHandler clickHandler = hit.collider.GetComponent<SphereClickHandler>();
                if (clickHandler != null && CheckForClick()) // Implement your method to check for the click input from the VR controller
                {
                    clickHandler.OnSphereClicked();
                }
            }
        }
    }

    bool CheckForClick()
    {
        // Replace with your VR input logic
        return Input.GetButtonDown("Fire1"); // Example for simplicity, adapt to VR input
    }
}

// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR;

// public class SphereClickDetector : MonoBehaviour
// {
//     public XRRayInteractor rightRayInteractor;

//     private void Update()
//     {
//         HandleRayInteraction(rightRayInteractor);
//     }

//     private void HandleRayInteraction(XRRayInteractor rayInteractor)
//     {
//         if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
//         {
//             if (hit.collider != null && hit.collider.CompareTag("Sphere"))
//             {
//                 InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
//                 if (rightHandDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isClicked) && isClicked)
//                 {
//                     SphereClickHandler handler = hit.collider.GetComponent<SphereClickHandler>();
//                     if (handler != null)
//                     {
//                         // Assuming your player is tagged as "Player"
//                         handler.playerObject = gameObject; // The player object
//                         handler.OnSphereClicked();
//                     }
//                 }
//             }
//         }
//     }
// }

// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR;

// public class SphereClickDetector : MonoBehaviour
// {
//     public XRRayInteractor rayInteractor; // Assign the ray interactor from the VR controller
//     public LayerMask interactableLayerMask; // Layer mask to filter only interactable objects

//     void Update()
//     {
//         if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
//         {
//             if (hit.collider.CompareTag("Sphere"))
//             {
//                 SphereClickHandler clickHandler = hit.collider.GetComponent<SphereClickHandler>();
//                 if (clickHandler != null && CheckForClick()) // Implement your method to check for the click input from the VR controller
//                 {
//                     clickHandler.OnSphereClicked();
//                     Debug.Log("Position changed");
//                 }
//             }
//         }
//     }

//     bool CheckForClick()
//     {
//         InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
//         if (rightHandDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isClicked) && isClicked)
//         {
//             return true;
//         }
//         return false;
//     }
// }
