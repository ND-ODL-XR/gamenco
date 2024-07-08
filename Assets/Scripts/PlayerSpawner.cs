using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour
{
    public List<Transform> spawnPoints;
    public NetworkVariable<int> nextSpawnPointIndex = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        Debug.Log("On network spawn called");
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += MovePlayerToSpawn;
        }
    }

    private void MovePlayerToSpawn(ulong clientId)
    {
        Debug.Log("Moving client to right spawn point");

        Vector3 spawnPosition = GetNextSpawnPoint();
        Debug.Log("spawning at "+ spawnPosition);

        SetPlayerSpawnPointClientRpc(spawnPosition, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } } });
    }

    [ClientRpc]
    private void SetPlayerSpawnPointClientRpc(Vector3 spawnPosition, ClientRpcParams clientRpcParams = default)
    {
        // This will run on the client that requested the spawn point
        MoveLocalCameraRig(spawnPosition);
    }

    private void MoveLocalCameraRig(Vector3 spawnPosition)
    {
       
        var rig = FindObjectOfType<OVRCameraRig>(); 
        if (rig != null)
        {
            rig.transform.position = spawnPosition;
        }
        else
        {
            Debug.LogError("Rig not found!");
        }
    }

    private Vector3 GetNextSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points set!");
            return Vector3.zero;
        }

        Vector3 spawnPoint = spawnPoints[nextSpawnPointIndex.Value].position;
        nextSpawnPointIndex.Value = (nextSpawnPointIndex.Value + 1) % spawnPoints.Count;
        return spawnPoint;
    }
}