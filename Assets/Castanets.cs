using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Castanets : TimedInstrument
{

    [SerializeField] private Renderer[] castanetRenderers;

    [ClientRpc]
    public void ActivateClientRpc()
    {
        for (int i = 0; i < castanetRenderers.Length; i++)
        {
            castanetRenderers[i].material.color = Color.green;
        }
    }

    [ClientRpc]
    public void DeactivateClientRpc()
    {
        for (int i = 0; i < castanetRenderers.Length; i++)
        {
            castanetRenderers[i].material.color = Color.red;
        }
    }

}
