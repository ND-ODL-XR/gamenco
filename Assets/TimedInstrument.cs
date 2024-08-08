using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

// Class that represents an instrument that plays notes on a beat e.g. castanets, guitar
// and judges if it's being played in the right sequence

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

    // On play note, request to play note on server
    public virtual void PlayNote() {
        PlayNoteServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void PlayNoteServerRpc(ServerRpcParams serverRpcParams = default) {
        // If the player is the one allowed to play the instrument, play the note across all clients
        if (serverRpcParams.Receive.SenderClientId == allowedPlayerId && !disabled)
        {
            playedCurrentBeat = true;
            PlayAudioClientRpc();
        }
    }

    // Play the note on each client
    [ClientRpc]
    public virtual void PlayAudioClientRpc() { 
        audioSource.Play();
    }

    // Check if a note has been played on the beat
    // Beat is 1-indexed
    public virtual void OnBeatEnd(int beat) {
        if (disabled) return;

        // If the note was played on the wrong beat, or if the note wasn't played on the right beat, it's incorrect
        if ((beats[beat - 1] == 1 && !playedCurrentBeat) || (beats[beat - 1] == 0 && playedCurrentBeat))
        {
            changeTextClientRpc(Color.red, "Inorrecto.");
            successfulSequence = false;
        }
        else
        {
            if (beat == 1) {                 
                successfulSequence = true;
            }
            changeTextClientRpc(Color.yellow, "¡Correcta!");
            if (beat >= beats.Length && successfulSequence) {
                OnFinishSequence();
            }
        }
        playedCurrentBeat = false;
    }

    public virtual void OnFinishSequence() {
        changeTextClientRpc(Color.green, "¡Secuencia completa!");
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
        // TODO: Fix this; it doesn't play the right sequence
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
