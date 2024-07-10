using UnityEngine;
using Photon.Pun;

public class SpectatorManagerScript : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject spectatorPrefab;

    void Start()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            JoinRoom();
        }
    }

    void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        // Check if the local player is a spectator
        if (PhotonNetwork.LocalPlayer.IsMasterClient) // Assume master client is a spectator
        {
            // Instantiate spectator prefab
            GameObject spectator = PhotonNetwork.Instantiate(spectatorPrefab.name, Vector3.zero, Quaternion.identity);
            Debug.Log("Spawned yes");

            // Automatically detect and activate main player camera
            GameObject mainPlayer = GameObject.FindGameObjectWithTag("Player");
            if (mainPlayer != null)
            {
                Camera mainPlayerCamera = mainPlayer.GetComponentInChildren<Camera>();
                if (mainPlayerCamera != null)
                {
                    mainPlayerCamera.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("Main player camera not found in the main player prefab hierarchy!");
                }
            }
            else
            {
                Debug.LogError("Main player not found with the tag 'Player'!");
            }
        }
    }

    // Implement methods for switching camera view, handling player disconnections, etc.
}
