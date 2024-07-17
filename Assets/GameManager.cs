using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> roomsSolved = new NetworkVariable<int>(0);

    public void OnRoomSolved()
    {
        roomsSolved.Value++;
    }


}
