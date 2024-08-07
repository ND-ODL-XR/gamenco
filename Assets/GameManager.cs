using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> roomsSolved = new NetworkVariable<int>(0);
    public GameObject curtains;
    //public ParticleSystem victorySparkles;
    public float liftTime;
    public float liftHeight;

    public void OnRoomSolved()
    {
        roomsSolved.Value++;
        if (roomsSolved.Value >= 4)
        {
            LiftCurtainsClientRpc();
        }
    }

    [ClientRpc]
    public void LiftCurtainsClientRpc() {
        StartCoroutine(LiftCurtains());
        //victorySparkles.Play();
    }

    public IEnumerator LiftCurtains()
    {
        float t = 0;
        while (Mathf.Abs( transform.localPosition.y - liftHeight) > 0.01f)
        {
            t += Time.deltaTime;
            float lerpedValue = t / liftTime;
            lerpedValue = lerpedValue * lerpedValue * (3f - 2f * lerpedValue);
            curtains.transform.localPosition = Vector3.Lerp(curtains.transform.localPosition, new Vector3(0, liftHeight, 0), lerpedValue);
            yield return null;
        }
    }
    
}
