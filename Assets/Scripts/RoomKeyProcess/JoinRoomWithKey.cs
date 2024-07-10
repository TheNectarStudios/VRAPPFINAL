using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class JoinRoomWithKey : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomKeyInputField; // Reference to the TMP InputField
    public TextMeshProUGUI feedbackText; // Reference to the feedback text to show messages

    private string inputKey;

    public void OnJoinRoomButtonClicked()
    {
        inputKey = roomKeyInputField.text;
        PhotonNetwork.JoinLobby(); // Ensure we are in a lobby to get room list
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (room.CustomProperties.ContainsKey("RoomKey"))
            {
                if ((string)room.CustomProperties["RoomKey"] == inputKey)
                {
                    PhotonNetwork.JoinRoom(room.Name);
                    return;
                }
            }
        }

        feedbackText.text = "Invalid room key! Please try again.";
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully.");
        feedbackText.text = "Joined room successfully.";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
        feedbackText.text = "Failed to join room: " + message;
    }
}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using Photon.Realtime;
// using TMPro;

// public class JoinRoomWithKey : MonoBehaviourPunCallbacks
// {
//     public TMP_InputField roomKeyInputField; // Reference to the TMP InputField
//     public TextMeshProUGUI feedbackText; // Reference to the feedback text to show messages

//     private string inputKey;

//     public void OnJoinRoomButtonClicked()
//     {
//         inputKey = roomKeyInputField.text;
//         PhotonNetwork.JoinLobby(); // Ensure we are in a lobby to get room list
//     }

//     public override void OnRoomListUpdate(List<RoomInfo> roomList)
//     {
//         foreach (RoomInfo room in roomList)
//         {
//             if (room.CustomProperties.ContainsKey("RoomKey"))
//             {
//                 if ((string)room.CustomProperties["RoomKey"] == inputKey)
//                 {
//                     PhotonNetwork.JoinRoom(room.Name);
//                     return;
//                 }
//             }
//         }

//         feedbackText.text = "Invalid room key! Please try again.";
//     }

//     public override void OnJoinedRoom()
//     {
//         Debug.Log("Joined room successfully.");
//         feedbackText.text = "Joined room successfully.";
//     }

//     public override void OnJoinRoomFailed(short returnCode, string message)
//     {
//         Debug.LogError("Failed to join room: " + message);
//         feedbackText.text = "Failed to join room: " + message;
//     }
// }
