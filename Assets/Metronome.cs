using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    // Interval between HALF beats
    [SerializeField] private float beatInterval;
    [SerializeField] private float errorMargin;
    [SerializeField] private TMPro.TextMeshProUGUI beatText;
    [SerializeField] private Guitar guitar;

    private AudioSource audioSource;
    private float beatTimer;
    public int beatCount;
  
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
            if (beatCount == 0)
            {
                audioSource.pitch = 3f;
            }
            else
            {
                audioSource.pitch = 1f;
            }
            StartCoroutine(PlayNextBeat());
            beatCount = (beatCount + 1) % 4;
            beatTimer = beatInterval;
        }
    }

    // Coroutine to play the next beat after errorMargin/2 seconds
    public IEnumerator PlayNextBeat()
    {
        int beat = beatCount + 1;
        yield return new WaitForSeconds(errorMargin / 2);
        beatText.text = beat.ToString();
        audioSource.Play();
        yield return new WaitForSeconds(errorMargin / 2);
        if (guitar.beats[beat - 1] == 1 && !guitar.playedCurrentBeat || guitar.beats[beat - 1] == 0 && guitar.playedCurrentBeat)
        {
            guitar.Fail();
        } else
        {
            guitar.playedCurrentBeat = false;
            guitar.Succeed();
        }
    }
}
