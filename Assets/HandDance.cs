using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


public class HandDance : NetworkBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private ulong poserId;
    [SerializeField] private GameManager gameManager;
    public TMPro.TextMeshProUGUI handPoseText;

    private bool coolingDown = false;
    private float cooldownTimer;

    public NetworkVariable<int> currentPose = new NetworkVariable<int>(1);

    [SerializeField] private Renderer[] progressRenderers;
    [SerializeField] private Color deactivatedColor;
    [SerializeField] private Color[] activatedColors;

    public void OnPoseCorrect(int poseIndex)
    {
        Debug.Log("Pose Correct: " + poseIndex);
        if (IsClient)
        {
            RequestPoseCorrectServerRpc(poseIndex);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestPoseCorrectServerRpc(int poseIndex, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("Requested from client " + serverRpcParams.Receive.SenderClientId);
        if (serverRpcParams.Receive.SenderClientId == poserId)
        {
            if (poseIndex == currentPose.Value)
            {
                currentPose.Value += 1;

                // Change to client RPC later
                if (currentPose.Value > 3)
                {
                    OnHandDanceCompletionClientRpc();
                    
                }
                cooldownTimer = cooldown;
                coolingDown = true;
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentPose.OnValueChanged += UpdateBoxColor;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            currentPose.OnValueChanged -= UpdateBoxColor;
        }
    }


    private void UpdateBoxColor(int oldPoseNumber, int newPoseNumber)
    {
        Debug.Log("Updating box color");
        UpdateBoxColorClientRpc(newPoseNumber);
    }

    [ClientRpc]
    private void UpdateBoxColorClientRpc(int newPoseNumber)
    {
        Debug.Log("Updating box color client rpc. New pose value is " + newPoseNumber);
        for (int i = 0; i < progressRenderers.Length; i++)
        {
            if (i < newPoseNumber - 1)
            {
                progressRenderers[i].material.color = activatedColors[i];
            }
            else
            {
                progressRenderers[i].material.color = deactivatedColor;
            }
        }
    }

    [ClientRpc]
    private void OnHandDanceCompletionClientRpc()
    {
        Debug.Log("Hand Dance Completed");
        handPoseText.text = "Hand Dance Complete";
        gameManager.OnRoomSolved();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                currentPose.Value = 1;
                Debug.Log("Hand Dance Reset");
                coolingDown = false;
            }
        }
        
    }

    void Start() {
        // Reset box colors in the beginning
        UpdateBoxColor(1, 1);
    }
}
