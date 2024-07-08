using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : MonoBehaviour
{
    [SerializeField] private AudioSource[] notes;
    private bool colliding = false;
    public int currentNote = 0;

    private void Update()
    {
        colliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (colliding) return;
        colliding = true;

        if (other.CompareTag("GuitarPick")) { 
            notes[currentNote].Play();
            currentNote++;
            if (currentNote >= notes.Length)
            {
                currentNote = 0;
            }
        }
    }
}
