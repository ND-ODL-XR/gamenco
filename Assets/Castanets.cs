using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Castanets : TimedInstrument
{

    [SerializeField] private Material castanetMaterial;

    [ClientRpc]
    public override void PlayAudioClientRpc()
    {
        audioSource.Play();
        StartCoroutine(ChangeColor());
    }

    // Coroutine to turn the material green for a short time
    private IEnumerator ChangeColor()
    {
        castanetMaterial.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        castanetMaterial.color = Color.red;
    }

    public void Start() {
        castanetMaterial.color = Color.red;
    }
}
