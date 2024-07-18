using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Castanets : TimedInstrument
{

    [SerializeField] private Material castanetMaterial;

    [ClientRpc]
    public void ActivateClientRpc()
    {
        castanetMaterial.color = Color.green;
    }

    [ClientRpc]
    public void DeactivateClientRpc()
    {
        castanetMaterial.color = Color.red;
    }

    public void Start() { 
        castanetMaterial.color = Color.red;
    }

}
