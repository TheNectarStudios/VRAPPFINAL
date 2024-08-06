using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Room360VR : MonoBehaviourPunCallbacks
{
    private WatchlistItem currentWatchlistItem;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void SetWatchlistItemAndChangeScene(WatchlistItem item)
    {
        currentWatchlistItem = item;
        SaveWatchlistItemToPlayerPrefs(item);
        ChangeSceneTo360VR();
    }

    private void SaveWatchlistItemToPlayerPrefs(WatchlistItem item)
    {
        PlayerPrefs.SetString("CurrentWatchlistItem_organisationName", item.organisationName);
        PlayerPrefs.SetString("CurrentWatchlistItem_parentPropertyName", item.parentPropertyName);
        PlayerPrefs.SetString("CurrentWatchlistItem_propertyName", item.propertyName);
        PlayerPrefs.SetString("CurrentWatchlistItem_date", item.date);
        PlayerPrefs.SetString("CurrentWatchlistItem_time", item.time);
        PlayerPrefs.SetString("CurrentWatchlistItem_imageURL", item.imageURL);
        PlayerPrefs.SetString("CurrentWatchlistItem_username", item.username);
        PlayerPrefs.Save();
    }

    private void ChangeSceneTo360VR()
    {
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

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
        PhotonNetwork.JoinOrCreateRoom("Room360VR", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
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

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon: " + cause.ToString());
    }
}
