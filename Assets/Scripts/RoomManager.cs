using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private string mapType;
    public static string RoomKey; // Store the room key

    public TextMeshProUGUI OccupancyRateText_ForSchool;
    public TextMeshProUGUI OccupancyRateText_ForOutdoor;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    void Update()
    {
    }

    #region UI Callback Methods
    public void OnEnterButtonClicked_Outdoor()
    {
        mapType = MultiplayerConstants.MAP_TYPE_KEY_OUTDOOR;
        TryCreateAndJoinRoom();
    }

    public void OnEnterButtonClicked_School()
    {
        mapType = MultiplayerConstants.MAP_TYPE_KEY_SCHOOL;
        TryCreateAndJoinRoom();
    }
    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        TryCreateAndJoinRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to servers again.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("A room is created with the name: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("The Local player: " + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerConstants.MAP_TYPE_KEY))
        {
            object mapType;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerConstants.MAP_TYPE_KEY, out mapType))
            {
                Debug.Log("Joined room with the map: " + (string)mapType);
                if ((string)mapType == MultiplayerConstants.MAP_TYPE_KEY_SCHOOL)
                {
                    PhotonNetwork.LoadLevel("World_School");
                }
                else if ((string)mapType == MultiplayerConstants.MAP_TYPE_KEY_OUTDOOR)
                {
                    PhotonNetwork.LoadLevel("World_Outdoor");
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to: " + "Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            OccupancyRateText_ForSchool.text = "0 / 20";
            OccupancyRateText_ForOutdoor.text = "0 / 20";
        }

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (room.Name.Contains(MultiplayerConstants.MAP_TYPE_KEY_OUTDOOR))
            {
                Debug.Log("Room is an Outdoor map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForOutdoor.text = room.PlayerCount + " / 20";
            }
            else if (room.Name.Contains(MultiplayerConstants.MAP_TYPE_KEY_SCHOOL))
            {
                Debug.Log("Room is a School map. Player count is: " + room.PlayerCount);
                OccupancyRateText_ForSchool.text = room.PlayerCount + " / 20";
            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined the Lobby.");
    }
    #endregion

    #region Private Methods
    public void TryCreateAndJoinRoom()
    {
        BookingFetcher.BookingData loadedData = BookingFetcher.LoadBookingDataFromJSON();
        if (loadedData != null && loadedData.key == BookingFetcher.FetchedRoomKey)
        {
            RoomKey = loadedData.key;
            CreateAndJoinRoom(RoomKey);
        }
        else
        {
            Debug.LogWarning("No valid room key found or room key does not match.");
        }
    }

    private void CreateAndJoinRoom(string roomKey)
    {
        string randomRoomName = "Room_" + mapType + "_" + DateTime.Now.Ticks;
        RoomKey = roomKey;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        string[] roomPropsInLobby = { MultiplayerConstants.MAP_TYPE_KEY, "RoomKey" };
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { MultiplayerConstants.MAP_TYPE_KEY, mapType },
            { "RoomKey", RoomKey }
        };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);

        Debug.Log("Room created with key: " + RoomKey);
    }
    #endregion
}
