using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castanets : TimedInstrument
{
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public override void PlayNote()
    {
        audioSource.Play();
        playedCurrentBeat = true;
    }
}
