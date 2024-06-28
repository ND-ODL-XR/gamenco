using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoopDetector : MonoBehaviour
{
    [SerializeField] private TMP_Text P1ScoreText;
    [SerializeField] private TMP_Text P2ScoreText;
    private int P1score = 0;
    private int P2score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the object that enters the trigger is the ball
        if (other.gameObject.tag == "P1Ball")
        {
            P1score++;
            P1ScoreText.SetText("P1 score: " + P1score);
        }
        else if (other.gameObject.tag == "P2Ball")
        {
            P2score++;
            P2ScoreText.SetText("P2 score: " + P2score);
        }
    }

}
