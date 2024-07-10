using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Voice.PUN;
using Photon.Pun;

using System;
using Photon.Voice.Unity;


public class VoiceDebugUI : MonoBehaviour
{
  
    [SerializeField]
    private TextMeshProUGUI voiceState;
 
    private PunVoiceClient punVoiceNetwork;  
    
    private void Awake()
    {
        this.punVoiceNetwork = PunVoiceClient.Instance;    
    }

    private void OnEnable()
    {
        this.punVoiceNetwork.Client.StateChanged += this.VoiceClientStateChanged;     
    }

    private void OnDisable()
    {
        this.punVoiceNetwork.Client.StateChanged -= this.VoiceClientStateChanged;     
    }

    private void Update()
    {
        if (this.punVoiceNetwork==null)
        {
            this.punVoiceNetwork = PunVoiceClient.Instance;
        }       
    }

    
    private void VoiceClientStateChanged(Photon.Realtime.ClientState fromState, Photon.Realtime.ClientState toState)
    {
        this.UpdateUiBasedOnVoiceState(toState);
    }

    private void UpdateUiBasedOnVoiceState(Photon.Realtime.ClientState voiceClientState)
    {
        this.voiceState.text = string.Format("PhotonVoice: {0}", voiceClientState);
        if (voiceClientState == Photon.Realtime.ClientState.Joined)
        {
            voiceState.gameObject.SetActive(false);
        }       
    }
}

