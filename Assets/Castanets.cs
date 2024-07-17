using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Castanets : TimedInstrument
{
    [ClientRpc]
    public void ActivateClientRpc()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }

    [ClientRpc]
    public void DeactivateClientRpc()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

}
