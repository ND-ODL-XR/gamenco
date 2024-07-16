using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    [SerializeField] private float beatInterval;
    [SerializeField] private float errorMargin;

    [SerializeField] private TMPro.TextMeshProUGUI[] beatTexts;
    [SerializeField] private TimedInstrument[] timedInstruments;

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
        // Save the current beat number due to the delay (1-indexed)
        int beat = beatCount + 1;
        yield return new WaitForSeconds(errorMargin / 2);

        //beatText.text = beat.ToString();
        for (int i = 0; i < beatTexts.Length; i++)
        {
            beatTexts[i].text = beat.ToString();
        }

        audioSource.Play();
        yield return new WaitForSeconds(errorMargin / 2);
        for (int i = 0; i < timedInstruments.Length; i++)
        {
            if (timedInstruments[i].gameObject.activeSelf)
            {
                timedInstruments[i].OnBeatEnd(beat);
            }
        }
        
    }
}
