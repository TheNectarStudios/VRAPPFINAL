using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Room360VR : MonoBehaviourPunCallbacks
{
    private bool isSceneLoading = false;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void ChangeSceneTo360VR()
    {
        if (!isSceneLoading)
        {
            isSceneLoading = true;
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("Changing scene to: 360VR");
                PhotonNetwork.LoadLevel("360VR");
            }
            else
            {
                Debug.LogError("Not in a room. Cannot change scene.");
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
        PhotonNetwork.JoinOrCreateRoom("Room360VR", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room.");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room.");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined the room.");
    }

    public void LeaveRoomAndJoinNew()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Leaving room...");
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.LogError("Not in a room. Cannot leave.");
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Successfully left room. Now joining or creating a new room.");
        PhotonNetwork.JoinOrCreateRoom("Room360VR", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
    }
}
