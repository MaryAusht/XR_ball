using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public float minSpawnDistance = 2f;
    public float maxSpawnDistance = 4.5f;
    public float minHeight = 0.3f;
    public float maxHeight = 1.5f;
    public float respawnTime = 5f;

    [SerializeField] private float wallMargin = 0.5f; // Adjustable in Inspector

    private GameObject currentTarget;
    private Coroutine respawnCoroutine;
    private bool gameActive = true;

    private void Start()
    {
        SpawnNewTarget();
    }

    public void SpawnNewTarget()
    {
        if (!gameActive || GameStateManager.isPaused || Time.timeScale == 0f) return;

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        currentTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);

        // Rotate target to face the player
        Transform playerTransform = Camera.main.transform;
        currentTarget.transform.LookAt(playerTransform);
        currentTarget.transform.rotation *= Quaternion.Euler(0, 180, 90);

        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }

        if (gameActive) // Ensure targets only respawn if the game is active
        {
            respawnCoroutine = StartCoroutine(AutoRespawnTimer());
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Transform playerTransform = Camera.main.transform;

        for (int i = 0; i < 30; i++)  // Try up to 30 times to find a valid spawn point
        {
            float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
            float xOffset = Mathf.Cos(angle) * distance;
            float zOffset = Mathf.Sin(angle) * distance;

            Vector3 candidatePosition = new Vector3(
                playerTransform.position.x + xOffset,
                playerTransform.position.y + UnityEngine.Random.Range(minHeight, maxHeight),
                playerTransform.position.z + zOffset
            );

            // Check if candidate position is too close to a wall
            if (!IsNearWall(candidatePosition))
            {
                return candidatePosition;
            }
        }

        UnityEngine.Debug.LogWarning("Failed to find a valid spawn point, using fallback.");
        return playerTransform.position + Vector3.forward * minSpawnDistance;
    }

    private bool IsNearWall(Vector3 position)
    {
        float checkDistance = wallMargin + 0.1f; // Slight buffer for accuracy
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (var dir in directions)
        {
            if (Physics.Raycast(position, dir, checkDistance))
            {
                return true; // Too close to a wall
            }
        }
        return false;
    }

    private IEnumerator AutoRespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);

        if (gameActive && !GameStateManager.isPaused && Time.timeScale != 0f)
        {
            if (currentTarget != null)
            {
                Destroy(currentTarget);
            }
            SpawnNewTarget();
        }
    }

    public void TargetDestroyed()
    {
        if (!gameActive || GameStateManager.isPaused) return;
        SpawnNewTarget();
    }

    public void StopSpawning()
    {
        gameActive = false;
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }
    }
}
