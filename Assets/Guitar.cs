using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Oculus.Interaction.HandGrab;


public class Guitar : TimedInstrument
{
    [SerializeField] private AudioSource[] notes;
    [SerializeField] private DistanceHandGrabInteractable distanceHandGrab;
    [SerializeField] private GameObject restingPosition;

    private bool colliding = false;
    public int currentNote = 0;

    // Cooldown for resetting the notes
    [SerializeField] float cooldown;
    private float cooldownTimer;

    private void Start()
    {
        DisableGuitarClientRpc();
    }

    // Disables the guitar for all players except the allowed player
    [ClientRpc]
    public virtual void DisableGuitarClientRpc()
    {
        if (NetworkManager.Singleton.LocalClientId != allowedPlayerId)
        {
            disableGrabbable();
        }
    }

    private void disableGrabbable()
    {
        distanceHandGrab.enabled = false;
    }


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

    [ClientRpc]
    public override void disableObjectClientRpc()
    {
        disabled = true;
        disableGrabbable();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        transform.position = restingPosition.transform.position;
        transform.rotation = restingPosition.transform.rotation;
    }
}
