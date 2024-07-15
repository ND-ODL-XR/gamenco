using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class TimedInstrument : MonoBehaviour
{
    public TMPro.TextMeshProUGUI resultText;

    public sbyte[] beats;
    public bool playedCurrentBeat = false;
    public bool successfulSequence = true;

    // Mechanics for playing notes will differ between instruments
    // But must turn playedCurrentBeat to true
    public abstract void PlayNote();

    // Beat is 1-indexed
    public virtual void OnBeatEnd(int beat) {
        if ((beats[beat - 1] == 1 && !playedCurrentBeat) || (beats[beat - 1] == 0 && playedCurrentBeat))
        {
            resultText.color = Color.red;
            resultText.text = "Incorrect";
            successfulSequence = false;
        }
        else
        {
            if (beat == 1) {                 
                successfulSequence = true;
            }
            resultText.color = Color.yellow;
            resultText.text = "Correct";
            if (beat >= beats.Length && successfulSequence) {
                OnFinishSequence();
            }
        }
        playedCurrentBeat = false;
    }

    public virtual void OnFinishSequence() {
        resultText.color = Color.green;
        resultText.text = "Sequence Complete";
        this.gameObject.SetActive(false);
    }
}
