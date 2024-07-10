using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField PlayerName_InputName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ConnectToPhotonServer()
    {
        if (PlayerName_InputName != null)
        {
            PhotonNetwork.NickName = PlayerName_InputName.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void ConnectAnonymously()
    {
        PhotonNetwork.ConnectUsingSettings();  
    }
    public override void OnConnected()
    {
        Debug.Log("On Connected is called. The server is available");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server with Player name: " + PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("HomeScene");
    }
}
