using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class JoinGameButton : MonoBehaviour
{
    public void JoinGame()
    {
        SceneManager.LoadScene("Game");
    }
}
