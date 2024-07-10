using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static VirtualWorldManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    public void LeaveRoomAndLoadHomeScene()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " Joined to: " + "Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }    

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    } 

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("HomeScene");
    }
}
