using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> roomsSolved = new NetworkVariable<int>(0);
    public TMPro.TextMeshProUGUI[] gameStatusText;
    
    public void OnRoomSolved()
    {
        roomsSolved.Value++;
        if (roomsSolved.Value >= 4)
        {
            ChangeGameStatusTextsClientRpc();
        }
    }

    [ClientRpc]
    public void ChangeGameStatusTextsClientRpc() {
        for (int i = 0; i < gameStatusText.Length; i++)
        {
            gameStatusText[i].text = "SUCCESS, GAME OVER!";
            gameStatusText[i].color = Color.green;
        }
    }
    
}
