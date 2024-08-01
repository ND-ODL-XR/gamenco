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
    public float maxSeparation = 0.4f; // Maximum separation of castanets in meters
    public float minSeparation = 0.1f; // Minimum separation of castanets in meters
    public float displacement = 1f; // Displacement of the castanets in meters


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
            float castanetSeparation = Mathf.Clamp(fingerDistance, minSeparation, maxSeparation);

            // Update castanet positions
            topCastanet.transform.position = bottomCastanet.transform.position - new Vector3(0f, castanetSeparation - displacement, 0f);
            topCastanet.transform.localRotation = Quaternion.Euler(-60 - castanetSeparation * 100, topCastanet.transform.localEulerAngles.y, topCastanet.transform.localEulerAngles.z);
        }
    }
}
