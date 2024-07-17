using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Guitar : TimedInstrument
{
    [SerializeField] private AudioSource[] notes;

    private bool colliding = false;
    public int currentNote = 0;

    // Cooldown for resetting the notes
    [SerializeField] float cooldown;
    private float cooldownTimer;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            cooldownTimer = cooldown;
            currentNote = 0;
        }
        colliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (colliding) return;
        colliding = true;

        if (other.CompareTag("GuitarPick")) {

            PlayNote();
        }
    }

    [ClientRpc]
    public override void PlayAudioClientRpc()
    {
        notes[currentNote].Play();

        currentNote++;
        if (currentNote >= notes.Length)
        {
            currentNote = 0;
        }
    }
}
