using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject playerCameraRig;
    private int nextSpawnPointIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (!IsOwner) return;

        Vector3 spawnPosition = GetNextSpawnPoint();
        playerCameraRig.transform.position = spawnPosition;
    }

    private Vector3 GetNextSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points set!");
            return Vector3.zero;
        }

        Vector3 spawnPoint = spawnPoints[nextSpawnPointIndex].position;
        nextSpawnPointIndex = (nextSpawnPointIndex + 1) % spawnPoints.Count;
        return spawnPoint;
    }
}