using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


public abstract class TimedInstrument : NetworkBehaviour
{
    public TMPro.TextMeshProUGUI resultText;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public ulong allowedPlayerId;
    [SerializeField] private Metronome metronome;

    [SerializeField] private GameManager gameManager;

    public sbyte[] beats;
    public bool playedCurrentBeat = false;
    public bool successfulSequence = true;
    public bool disabled = false;

    public virtual void PlayNote() {
        PlayNoteServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void PlayNoteServerRpc(ServerRpcParams serverRpcParams = default) {
        if (serverRpcParams.Receive.SenderClientId == allowedPlayerId && !disabled)
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
        if (disabled) return;

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
        gameManager.OnRoomSolved();
        StartLoopingTrackClientRpc();
        disableObjectClientRpc();
    }


    [ClientRpc]
    public virtual void StartLoopingTrackClientRpc() {
        metronome.OnBeat += PlayLoopingNote;
    }

    public virtual void PlayLoopingNote() {
        // Modded since the beat count is 1 ahead of the actual beat
        // TODO: Fix this
        if (beats[(metronome.beatCount.Value + 1) % 4] == 1)
        {
            PlayAudioClientRpc();
        }
    }

    [ClientRpc]
    public virtual void changeTextClientRpc(Color color, string text) {
        resultText.color = color;
        resultText.text = text;
    }

    [ClientRpc]
    public virtual void disableObjectClientRpc() {
        disabled = true;
    }
}
