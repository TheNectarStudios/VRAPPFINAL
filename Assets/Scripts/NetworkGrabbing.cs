using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView m_PhotonView;
    Rigidbody rb;
    bool isHeld = false;

    private void Awake() {
        m_PhotonView = GetComponent<PhotonView>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(isHeld)
        {
            rb.isKinematic = true;
            gameObject.layer = 11;
        }
        else
        {
            rb.isKinematic = false;
            gameObject.layer = 9;
        }
    }

    public void TransferOwnership()
    {
        m_PhotonView.RequestOwnership();
    }

    public void OnSelectEntered()
    {
        Debug.Log("Object is Grabbed");
        m_PhotonView.RPC("StartNetworkGrabbing",RpcTarget.AllBuffered);
        if (m_PhotonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("We do not request the ownership. Already mine");
        }
        else
        {

        }

        TransferOwnership();
    }

    public void OnSelectedExited()
    {
        Debug.Log("Object is Released");
        m_PhotonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != m_PhotonView)
        {
            return;
        }

        Debug.Log("Ownership requested for: " + targetView.name + " from " + requestingPlayer.NickName);
        m_PhotonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Ownership transferred to: " + targetView.name + " from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log("Ownership transfer failed for: " + targetView.name + " requested by " + senderOfFailedRequest.NickName);
    }

    [PunRPC]
    public void StartNetworkGrabbing()
    {
        isHeld = true;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        isHeld = false;
    }
}
