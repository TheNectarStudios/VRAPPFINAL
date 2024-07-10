// using UnityEngine;
// using Photon.Pun;

// public class SpawnManager360VR : MonoBehaviourPunCallbacks
// {
//     [SerializeField] GameObject mainPlayerPrefab; // Main player prefab
//     [SerializeField] GameObject cameraPrefab; // Camera prefab
//     public Vector3 spawnPosition;

//     void Start()
//     {
//         // Check if the player is connected and ready to join a room
//         if (PhotonNetwork.IsConnectedAndReady)
//         {
//             // Spawn the player or camera
//             SpawnPlayer();
//         }
//     }

//     public override void OnJoinedRoom()
//     {
//         // Spawn the player or camera when the player joins the room
//         SpawnPlayer();
//     }

//     void SpawnPlayer()
//     {
//         // Check if the current player is the master client (first player in the room)
//         if (PhotonNetwork.IsMasterClient)
//         {
//             if (mainPlayerPrefab != null)
//             {
//                 // Instantiate the main player prefab at the specified spawn position
//                 PhotonNetwork.Instantiate(mainPlayerPrefab.name, spawnPosition, Quaternion.identity);
//             }
//             else
//             {
//                 Debug.LogError("Main player prefab is not assigned!");
//             }
//         }
//         else
//         {
//             if (cameraPrefab != null)
//             {
//                 // Instantiate the camera prefab for non-master clients
//                 PhotonNetwork.Instantiate(cameraPrefab.name, Vector3.zero, Quaternion.identity);
//             }
//             else
//             {
//                 Debug.LogError("Camera prefab is not assigned!");
//             }
//         }
//     }
// }


using UnityEngine;
using Photon.Pun;

public class SpawnManager360VR : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject mainPlayerPrefab; // Main player prefab
    [SerializeField] GameObject cameraPrefab; // Camera prefab
    public Vector3 spawnPosition;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Check if the current player is the master client (first player in the room)
        if (PhotonNetwork.IsMasterClient)
        {
            if (mainPlayerPrefab != null)
            {
                // Instantiate the main player prefab
                PhotonNetwork.Instantiate(mainPlayerPrefab.name, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Main player prefab is not assigned!");
            }
        }
        else
        {
            if (cameraPrefab != null)
            {
                // Instantiate the camera prefab for non-master clients
                PhotonNetwork.Instantiate(cameraPrefab.name, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Camera prefab is not assigned!");
            }
        }
    }
}
