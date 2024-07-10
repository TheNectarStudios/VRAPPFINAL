using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab; // Player prefab
    public Vector3 spawnPosition; // Spawn position for the player

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
        if (playerPrefab != null)
        {
            // Instantiate the player prefab
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Player prefab is not assigned!");
        }
    }
}
