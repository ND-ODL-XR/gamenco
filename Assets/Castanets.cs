using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Castanets : TimedInstrument
{
    [SerializeField] private CastanetAnimator[] castanetAnimators;

    [ClientRpc]
    public override void disableObjectClientRpc()
    {
        disabled = true;
        for (int i = 0; i < castanetAnimators.Length; i++)
        {
            castanetAnimators[i].enabled = false;
        }
    }
}
