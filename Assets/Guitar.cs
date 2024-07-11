using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guitar : MonoBehaviour
{
    [SerializeField] private AudioSource[] notes;
    [SerializeField] private Metronome metronome;
    [SerializeField] private TMPro.TextMeshProUGUI guitarText;

    private bool colliding = false;
    public int currentNote = 0;
    [SerializeField] float cooldown;
    private float cooldownTimer;

    public sbyte[] beats;
    public bool playedCurrentBeat = false;

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
            notes[currentNote].Play();
            currentNote++;

            playedCurrentBeat = true;

            if (currentNote >= notes.Length)
            {
                currentNote = 0;
            }
        }
    }

    public void Fail() { 
        guitarText.color = Color.red;
        guitarText.text = "Incorrect";
    }

    public void Succeed()
    {
        guitarText.color = Color.green;
        guitarText.text = "Correct";
    }   

}
