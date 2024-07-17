using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Metronome : NetworkBehaviour
{
    [SerializeField] private float beatInterval;

    [SerializeField] private TMPro.TextMeshProUGUI[] beatTexts;
    [SerializeField] private TimedInstrument[] timedInstruments;

    private AudioSource audioSource;
    private float beatTimer;

    public NetworkVariable<int> beatCount = new NetworkVariable<int>(0);
  
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        beatTimer = beatInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {

            beatTimer -= Time.deltaTime;

            if (beatTimer <= 0)
            {
                float pitch;
                if (beatCount.Value == 0)
                {
                    pitch = 3f;
                }
                else
                {
                    pitch = 1f;
                }
                PlayNextBeatClientRpc(pitch);
                beatCount.Value = (beatCount.Value + 1) % 4;
                beatTimer = beatInterval;
            }
        }
    }

    [ClientRpc]
    public void PlayNextBeatClientRpc(float pitch) {
        StartCoroutine(PlayNextBeat(pitch));
    }

    // Coroutine to play the beat and then end the "beat" halfway to the next beat
    public IEnumerator PlayNextBeat(float pitch)
    {
        int beat = beatCount.Value + 1;
        yield return new WaitForSeconds(beatInterval / 2);

        for (int i = 0; i < beatTexts.Length; i++)
        {
            beatTexts[i].text = beat.ToString();
        }

        audioSource.pitch = pitch;
        audioSource.Play();

        yield return new WaitForSeconds(beatInterval / 2);
        for (int i = 0; i < timedInstruments.Length; i++)
        {
            if (timedInstruments[i].gameObject.activeSelf)
            {
                timedInstruments[i].OnBeatEnd(beat);
            }
        }
    }
}
