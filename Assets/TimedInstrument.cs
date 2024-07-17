using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


public abstract class TimedInstrument : NetworkBehaviour
{
    public TMPro.TextMeshProUGUI resultText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ulong allowedPlayerId;

    public sbyte[] beats;
    public bool playedCurrentBeat = false;
    public bool successfulSequence = true;

    public virtual void PlayNote() {
        PlayNoteServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void PlayNoteServerRpc(ServerRpcParams serverRpcParams = default) {
        if (serverRpcParams.Receive.SenderClientId == allowedPlayerId)
        {
            playedCurrentBeat = true;
            PlayAudioClientRpc();
        }
    }

    [ClientRpc]
    public virtual void PlayAudioClientRpc() { 
        audioSource.Play();
    }

    // Beat is 1-indexed
    public virtual void OnBeatEnd(int beat) {
        if ((beats[beat - 1] == 1 && !playedCurrentBeat) || (beats[beat - 1] == 0 && playedCurrentBeat))
        {
            changeTextClientRpc(Color.red, "Incorrect");
            successfulSequence = false;
        }
        else
        {
            if (beat == 1) {                 
                successfulSequence = true;
            }
            changeTextClientRpc(Color.yellow, "Correct");
            if (beat >= beats.Length && successfulSequence) {
                OnFinishSequence();
            }
        }
        playedCurrentBeat = false;
    }

    public virtual void OnFinishSequence() {
        changeTextClientRpc(Color.green, "Sequence Complete");
        disableObjectClientRpc();
    }

    [ClientRpc]
    public void changeTextClientRpc(Color color, string text) {
        resultText.color = color;
        resultText.text = text;
    }

    [ClientRpc]
    public void disableObjectClientRpc() {
        this.gameObject.SetActive(false);
    }
}
