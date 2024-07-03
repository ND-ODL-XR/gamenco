using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoin : IMatchmakingCallbacks
{
    [SerializeField] private InputField joinRoomName;
    private LoadBalancingClient loadBalancingClient;

    //public void CreateRoom() { 
    //    Debug.Log("Create Room");
    //    PhotonNetwork.CreateRoom("Room1");
    //}

    void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    public void JoinRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        Debug.Log("Join Room");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomName = joinRoomName.text;
        enterRoomParams.RoomOptions = roomOptions;
        loadBalancingClient.OpJoinOrCreateRoom(enterRoomParams);

    }

    // do not forget to register callbacks via loadBalancingClient.AddCallbackTarget
    // also deregister via loadBalancingClient.RemoveCallbackTarget
    #region IMatchmakingCallbacks

    void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
    {
        // log error code and message
        Debug.LogError($"Join Room Failed: {returnCode} - {message}");
    }

    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        // joined a room successfully, OpJoinOrCreateRoom leads here on success
        Debug.Log("Joined Room");
        PhotonNetwork.LoadLevel("Game");
    }

    // [..] Other callbacks implementations are stripped out for brevity, they are empty in this case as not used.

    #endregion

}
