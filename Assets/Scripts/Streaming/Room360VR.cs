// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using Photon.Realtime;
// using TMPro;
// public class RoomManager : MonoBehaviourPunCallbacks
// {
//     private string mapType;

//     //public TextMeshProUGUI OccupancyRateText_ForSchool;
//     //public TextMeshProUGUI OccupancyRateText_ForOutdoor;
//     public TextMeshProUGUI OccupancyRateText_ForVRroom;


//     // Start is called before the first frame update
//     void Start()
//     {
//         PhotonNetwork.AutomaticallySyncScene = true;

//         if (!PhotonNetwork.IsConnectedAndReady)
//         {
//             PhotonNetwork.ConnectUsingSettings();
//         }
//         else
//         {
//             PhotonNetwork.JoinLobby();
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }

//     #region UI Callback Methods
//     public void JoinRandomRoom()
//     {
//         PhotonNetwork.JoinRandomRoom();
//     }

//     public void OnEnterButtonClicked_VRroom()
//     {
//         mapType = MultiplayerConstants.MAP_TYPE_KEY_360;
//         ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerConstants.MAP_TYPE_KEY, mapType } };
//         PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
//     }

//   ndregion

//     #region Photon Callback Methods
//     public override void OnJoinRandomFailed(short returnCode, string message)
//     {
//         Debug.Log(message);
//         CreateAndJoinRoom();
//     }

//     public override void OnConnectedToMaster()
//     {
//         Debug.Log("Connected to servers again.");
//         PhotonNetwork.JoinLobby();
//     }

//     public override void OnCreatedRoom()
//     {
//         Debug.Log("A room is created with the name: " + PhotonNetwork.CurrentRoom.Name);
//     }

//     public override void OnJoinedRoom()
//     {
//         Debug.Log("The Local player: " + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);

//         if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerConstants.MAP_TYPE_KEY))
//         {
//             object mapType;
//             if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerConstants.MAP_TYPE_KEY,out mapType))
//             {
//                 Debug.Log("Joined room with the map: " + (string)mapType);
//                 if ((string)mapType == MultiplayerConstants.MAP_TYPE_KEY_360)
//                 {
//                     //Load the school scene
//                     PhotonNetwork.LoadLevel("360VR");

//                 }
//             }
//         }


//     }


//     public override void OnPlayerEnteredRoom(Player newPlayer)
//     {
//         Debug.Log(newPlayer.NickName + " joined to: " + "Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
//     }


//     public override void OnRoomListUpdate(List<RoomInfo> roomList)
//     {
//         if (roomList.Count == 0)
//         {
//             //There is no room at all
//             // OccupancyRateText_ForSchool.text = 0 + " / " + 20;
//             // OccupancyRateText_ForOutdoor.text = 0 + " / " + 20;

//         }

//         foreach (RoomInfo room in roomList)
//         {
//             Debug.Log(room.Name);
//             if (room.Name.Contains(MultiplayerConstants.MAP_TYPE_KEY_360))
//             {
//                 //Update the Outdoor room occupancy field
//                 Debug.Log("Room is a Outdoor map. Player count is: " + room.PlayerCount);

//                 OccupancyRateText_ForVRroom.text = room.PlayerCount + " / " + 20;

//             }
//         }


//     }

//     public override void OnJoinedLobby()
//     {
//         Debug.Log("Joined the Lobby.");
//     }
//     #endregion

//     private void CreateAndJoinRoom()
//     {
//         string randomRoomName = "Room_" +mapType + Random.Range(0, 10000);
//         RoomOptions roomOptions = new RoomOptions();
//         roomOptions.MaxPlayers = 20;


//         string[] roomPropsInLobby = { MultiplayerConstants.MAP_TYPE_KEY };
//         //We have 2 different maps
//         //1. Outdoor = "outdoor"
//         //2. School = "school"

//         ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerConstants.MAP_TYPE_KEY, mapType } };

//         roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
//         roomOptions.CustomRoomProperties = customRoomProperties;

//         PhotonNetwork.CreateRoom(randomRoomName, roomOptions);

//     }


    
    
// }
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using Photon.Realtime;
// using TMPro;

// public class Room360VR : MonoBehaviourPunCallbacks
// {
//     private string mapType;
//     public TextMeshProUGUI OccupancyRateText_ForVRroom;
//     private bool shouldJoinRoomAfterReconnect = false;

//     void Start()
//     {
//         PhotonNetwork.AutomaticallySyncScene = true;

//         if (!PhotonNetwork.IsConnectedAndReady)
//         {
//             PhotonNetwork.ConnectUsingSettings();
//         }
//     }

//     public void OnEnterButtonClicked_VRroom()
//     {
//         mapType = MultiplayerConstants.MAP_TYPE_KEY_360;

//         if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.Server == ServerConnection.MasterServer)
//         {
//             JoinRandomRoom();
//         }
//         else if (PhotonNetwork.IsConnected && PhotonNetwork.Server != ServerConnection.MasterServer)
//         {
//             Debug.LogWarning("Client is connected but not to Master Server. Attempting to switch to Master Server...");
//             shouldJoinRoomAfterReconnect = true;
//             PhotonNetwork.Disconnect();  // Disconnect first
//         }
//         else if (!PhotonNetwork.IsConnected)
//         {
//             Debug.LogError("Client is not connected to Master Server. Trying to reconnect...");
//             shouldJoinRoomAfterReconnect = true;
//             PhotonNetwork.ConnectUsingSettings();
//         }
//     }

//     private void JoinRandomRoom()
//     {
//         ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerConstants.MAP_TYPE_KEY, mapType } };
//         PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
//     }

//     #region Photon Callback Methods

//     public override void OnDisconnected(DisconnectCause cause)
//     {
//         Debug.Log("Disconnected from Photon with reason: " + cause.ToString());

//         if (shouldJoinRoomAfterReconnect)
//         {
//             shouldJoinRoomAfterReconnect = false;
//             PhotonNetwork.ReconnectAndRejoin();
//         }
//     }

//     public override void OnConnectedToMaster()
//     {
//         Debug.Log("Connected to Master Server.");
//         if (shouldJoinRoomAfterReconnect)
//         {
//             shouldJoinRoomAfterReconnect = false;
//             JoinRandomRoom();
//         }
//         else
//         {
//             PhotonNetwork.JoinLobby();
//         }
//     }

//     public override void OnJoinRandomFailed(short returnCode, string message)
//     {
//         Debug.LogError($"JoinRandomRoom failed: {message}");
//         CreateAndJoinRoom();
//     }

//     public override void OnCreatedRoom()
//     {
//         Debug.Log("A new room has been created with the name: " + PhotonNetwork.CurrentRoom.Name);
//     }

//     public override void OnJoinedRoom()
//     {
//         Debug.Log("The Local player: " + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);

//         if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerConstants.MAP_TYPE_KEY))
//         {
//             if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerConstants.MAP_TYPE_KEY, out object mapType))
//             {
//                 Debug.Log("Joined room with the map: " + (string)mapType);
//                 if ((string)mapType == MultiplayerConstants.MAP_TYPE_KEY_360)
//                 {
//                     PhotonNetwork.LoadLevel("360VR");
//                 }
//             }
//         }
//     }

//     public override void OnPlayerEnteredRoom(Player newPlayer)
//     {
//         Debug.Log(newPlayer.NickName + " joined to: " + "Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
//     }

//     public override void OnRoomListUpdate(List<RoomInfo> roomList)
//     {
//         if (roomList.Count == 0)
//         {
//             OccupancyRateText_ForVRroom.text = 0 + " / " + 20;
//         }

//         foreach (RoomInfo room in roomList)
//         {
//             Debug.Log(room.Name);
//             if (room.Name.Contains(MultiplayerConstants.MAP_TYPE_KEY_360))
//             {
//                 Debug.Log("Room is a VR room. Player count is: " + room.PlayerCount);
//                 OccupancyRateText_ForVRroom.text = room.PlayerCount + " / " + 20;
//             }
//         }
//     }

//     #endregion

//     #region Private Methods

//     private void CreateAndJoinRoom()
//     {
//         string randomRoomName = "Room_" + mapType + Random.Range(0, 10000);
//         RoomOptions roomOptions = new RoomOptions
//         {
//             MaxPlayers = 20,
//             CustomRoomPropertiesForLobby = new string[] { MultiplayerConstants.MAP_TYPE_KEY }
//         };

//         ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerConstants.MAP_TYPE_KEY, mapType } };
//         roomOptions.CustomRoomProperties = customRoomProperties;

//         PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
//     }

//     #endregion
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using Photon.Realtime;
// using TMPro;

// public class Room360VR : MonoBehaviourPunCallbacks
// {
//     private bool shouldJoinNewRoomAfterLeaving = false;

//     void Start()
//     {
//         PhotonNetwork.AutomaticallySyncScene = true;

//         if (!PhotonNetwork.IsConnectedAndReady)
//         {
//             PhotonNetwork.ConnectUsingSettings();
//         }
//     }

//     public void ChangeRoom()
//     {
//         Debug.Log("Initiating room change to: 360VR");
//         if (PhotonNetwork.InRoom)
//         {
//             Debug.Log("Currently in a room, leaving the current room first...");
//             shouldJoinNewRoomAfterLeaving = true;
//             PhotonNetwork.LeaveRoom();
//         }
//         else
//         {
//             Debug.Log("Not in any room, joining or creating new room: 360VR");
//             JoinOrCreateRoom();
//         }
//     }

//     public override void OnLeftRoom()
//     {
//         Debug.Log("Left the room.");
//         if (shouldJoinNewRoomAfterLeaving)
//         {
//             shouldJoinNewRoomAfterLeaving = false;
//             Debug.Log("Switching back to Master Server...");
//             PhotonNetwork.Disconnect();
//         }
//     }

//     public override void OnDisconnected(DisconnectCause cause)
//     {
//         Debug.Log("Disconnected from Photon, cause: " + cause);
//         if (cause == DisconnectCause.None || cause == DisconnectCause.DisconnectByClientLogic)
//         {
//             Debug.Log("Reconnecting to Master Server...");
//             PhotonNetwork.ConnectUsingSettings();
//         }
//     }

//     public override void OnConnectedToMaster()
//     {
//         Debug.Log("Connected to Master Server.");
//         Debug.Log("Attempting to join or create the new room: 360VR");
//         JoinOrCreateRoom();
//     }

//     public void JoinRandomRoom()
//     {
//         if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.Server == ServerConnection.MasterServer)
//         {
//             PhotonNetwork.JoinRandomRoom();
//         }
//         else
//         {
//             Debug.LogWarning("Cannot join random room. Make sure the client is connected to the Master Server.");
//         }
//     }

//     public override void OnJoinRandomFailed(short returnCode, string message)
//     {
//         Debug.LogError($"JoinRandomRoom failed: {message}");
//         CreateAndJoinRoom();
//     }

//     public override void OnCreatedRoom()
//     {
//         Debug.Log("A room has been created with the name: " + PhotonNetwork.CurrentRoom.Name);
//     }

//     public override void OnJoinedRoom()
//     {
//         Debug.Log("The Local Player: " + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
//     }

//     public override void OnPlayerEnteredRoom(Player newPlayer)
//     {
//         Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
//     }

//     private void CreateAndJoinRoom()
//     {
//         Debug.Log("Creating and joining a new room with name: 360VR");
//         RoomOptions roomOptions = new RoomOptions
//         {
//             MaxPlayers = 20
//         };

//         PhotonNetwork.CreateRoom("360VR", roomOptions);
//     }

//     private void JoinOrCreateRoom()
//     {
//         Debug.Log("Joining or creating a room with name: 360VR");
//         RoomOptions roomOptions = new RoomOptions
//         {
//             MaxPlayers = 20
//         };
//         PhotonNetwork.JoinOrCreateRoom("360VR", roomOptions, null);
//     }
// }
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Room360VR : MonoBehaviourPunCallbacks
{
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
