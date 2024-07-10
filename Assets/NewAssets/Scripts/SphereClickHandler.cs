
// using UnityEngine;

// public class SphereClickHandler : MonoBehaviour
// {
//     // Reference to the target GameObject to move to
//     public GameObject targetObject;

//     public void OnSphereClicked()
//     {
//         GameObject playerObject = GameObject.FindWithTag("Player");
//         if (playerObject != null && targetObject != null)
//         {
//             // Move the player object to the position of the target object
//             playerObject.transform.position = targetObject.transform.position;
//             Debug.Log("Sphere clicked! Player teleported.");
//         }
//         else
//         {
//             if (playerObject == null)
//             {
//                 Debug.LogWarning("No object with tag 'Player' found!");
//             }
//             if (targetObject == null)
//             {
//                 Debug.LogWarning("Target object is not assigned!");
//             }
//         }
//     }
// }

using UnityEngine;

public class SphereClickHandler : MonoBehaviour
{
    public int index; // Public field to store the index

    public void OnSphereClicked()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (playerObjects.Length > 0)
        {
            float targetX = index * 50f;
            foreach (GameObject playerObject in playerObjects)
            {
                Vector3 newPosition = new Vector3(targetX, playerObject.transform.position.y, playerObject.transform.position.z);
                playerObject.transform.position = newPosition;
            }
            Debug.Log("Sphere clicked! All players teleported.");
        }
        else
        {
            Debug.LogWarning("No objects with tag 'Player' found!");
        }
    }
}
