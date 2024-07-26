using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CastanetAnimator : MonoBehaviour
{
    [SerializeField] private ulong allowedPlayerId;

    public OVRHand rightHand;
    public GameObject topCastanet;
    public GameObject bottomCastanet;
    public float maxSeparation = 0.1f; // Maximum separation of castanets in meters


    private void Update()
    {
        if (NetworkManager.Singleton.LocalClientId == allowedPlayerId)
        {
            MoveTopClientRpc();
        }
    }

    [ClientRpc]
    public void MoveTopClientRpc()
    {
        if (rightHand.IsTracked)
        {
            // Calculate the distance between index and thumb
            float fingerDistance = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

            // Map the finger distance to castanet separation
            float castanetSeparation = Mathf.Clamp(fingerDistance, 0f, maxSeparation);

            // Update castanet positions
            Vector3 centerPos = (topCastanet.transform.position + bottomCastanet.transform.position) / 2f;
            topCastanet.transform.position = centerPos - new Vector3(castanetSeparation / 2f, 0f, 0f);
            bottomCastanet.transform.position = centerPos + new Vector3(castanetSeparation / 2f, 0f, 0f);
        }
    }
}
