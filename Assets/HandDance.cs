using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandDance : MonoBehaviour
{
    [SerializeField] private float cooldown;
    public TMPro.TextMeshProUGUI handPoseText;

    private bool coolingDown = false;
    private float cooldownTimer;

    private int currentPose = 1;

    [SerializeField] private Material[] progressMaterials;
    [SerializeField] private Color deactivatedColor;
    [SerializeField] private Color[] activatedColors;

    public void OnPoseCorrect(int poseIndex) { 
        if (poseIndex == currentPose) {
            progressMaterials[currentPose - 1].color = activatedColors[currentPose - 1];
            currentPose++;
            if (currentPose > 3) {
                handPoseText.text = "Hand Dance Complete";
                this.gameObject.SetActive(false);
            }
            cooldownTimer = cooldown;
            coolingDown = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                currentPose = 1;
                Debug.Log("Hand Dance Reset");
                coolingDown = false;
                for (int i = 0; i < progressMaterials.Length; i++)
                {
                    progressMaterials[i].color = deactivatedColor;
                }
            }
        }
        
    }

    void OnApplicationQuit()
    {
        for (int i = 0; i < progressMaterials.Length; i++)
        {
            progressMaterials[i].color = deactivatedColor;
        }
    }
}
