using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour
{
    public List<Transform> spawnPoints;
    public NetworkObject playerPrefab;
    private int nextSpawnPointIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (!IsServer) return;

        Vector3 spawnPosition = GetNextSpawnPoint();

        NetworkObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        playerInstance.SpawnAsPlayerObject(clientId);
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