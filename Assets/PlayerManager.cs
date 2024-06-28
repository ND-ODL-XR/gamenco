using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Fusion;
using Fusion.Sockets;

public class PlayerManager : MonoBehaviour
{
    private int numPlayers = 0;
    [SerializeField] private NetworkRunner networkRunner;

    private void Awake()
    {
        numPlayers = 0;
        networkRunner = GetComponentInParent<NetworkRunner>();
    }
    
    public void onPlayerJoin() { 
        Debug.Log("Player joined");
        Debug.Log(networkRunner.LocalPlayer.PlayerId);
    }
}
