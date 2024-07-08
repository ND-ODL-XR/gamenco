using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private float beatInterval = 1.5f;
    private AudioSource audioSource;
    private float beatTimer;
  
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        beatTimer = beatInterval;
    }

    // Update is called once per frame
    void Update()
    {
        beatTimer -= Time.deltaTime;
        if (beatTimer <= 0)
        {
            audioSource.Play();
            beatTimer = beatInterval;
        }
    }
}
