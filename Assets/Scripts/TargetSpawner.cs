using UnityEngine;
using System.Collections;
using TMPro;
using System;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public float minSpawnDistance = 2f;
    public float maxSpawnDistance = 6f;
    public float minHeight = 1f;
    public float maxHeight = 3f;
    public float respawnTime = 5f;
    public float gameDuration = 60f;

    public TMP_Text countdownText;

    private GameObject currentTarget;
    private Coroutine respawnCoroutine;
    private bool gameActive = true;
    private float remainingTime;

    private MRUKRoom mrukRoom;

    private void Start()
    {
        StartCoroutine(WaitForMRUKAndStartGame());
    }

    private IEnumerator WaitForMRUKAndStartGame()
    {
        while (MRUK.Instance == null || !MRUK.Instance.IsInitialized)
        {
            yield return null;
        }

        mrukRoom = MRUK.Instance.GetCurrentRoom();
        if (mrukRoom == null)
        {
            Debug.LogError("No MRUK Room found.");
            yield break;
        }

        StartCoroutine(GameTimerWithUI());
        SpawnNewTarget();
    }

    private IEnumerator GameTimerWithUI()
    {
        remainingTime = gameDuration;

        while (remainingTime > 0f)
        {
            UpdateCountdownUI();
            yield return null;
            remainingTime -= Time.deltaTime;
        }

        EndGame();
    }

    private void UpdateCountdownUI()
    {
        if (countdownText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            int milliseconds = Mathf.FloorToInt((remainingTime * 100) % 100);
            countdownText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    private void EndGame()
    {
        gameActive = false;

        if (countdownText != null)
        {
            countdownText.text = "00:00:00";
            countdownText.gameObject.SetActive(false);
        }

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        GameManager.Instance.EndGame();
    }

    public void SpawnNewTarget()
    {
        if (!gameActive || mrukRoom == null) return;

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        currentTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);

        Transform playerTransform = Camera.main.transform;
        currentTarget.transform.LookAt(playerTransform);
        currentTarget.transform.rotation *= Quaternion.Euler(0, 180, 90);

        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }
        respawnCoroutine = StartCoroutine(AutoRespawnTimer());
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (mrukRoom == null || mrukRoom.Anchors == null || mrukRoom.Anchors.Count == 0)
        {
            Debug.LogWarning("MRUK Room or anchors not available. Fallback to default.");
            return Camera.main.transform.position + Vector3.forward * 3f;
        }

        // Calculate bounds from all anchor positions
        Bounds bounds = new Bounds();
        bool hasBounds = false;

        foreach (var anchor in mrukRoom.Anchors)
        {
            if (anchor == null) continue;

            Vector3 pos = anchor.transform.position;
            if (!hasBounds)
            {
                bounds = new Bounds(pos, Vector3.zero);
                hasBounds = true;
            }
            else
            {
                bounds.Encapsulate(pos);
            }
        }

        if (!hasBounds)
        {
            return Camera.main.transform.position + Vector3.forward * 3f;
        }

        Vector3 playerPosition = Camera.main.transform.position;

        float margin = 0.3f; // distance from walls

        for (int i = 0; i < 20; i++)
        {
            float x = UnityEngine.Random.Range(bounds.min.x + margin, bounds.max.x - margin);
            float z = UnityEngine.Random.Range(bounds.min.z + margin, bounds.max.z - margin);
            float y = bounds.min.y + UnityEngine.Random.Range(minHeight, maxHeight);

            Vector3 candidate = new Vector3(x, y, z);
            float distance = Vector3.Distance(playerPosition, candidate);

            if (distance >= minSpawnDistance && distance <= maxSpawnDistance)
            {
                return candidate;
            }
        }

        // Fallback if nothing valid is found
        Debug.LogWarning("Fallback spawn position used.");
        return playerPosition + Vector3.forward * Mathf.Clamp(maxSpawnDistance, 1f, 5f);
    }


    private IEnumerator AutoRespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);

        if (gameActive)
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
        if (!gameActive) return;
        SpawnNewTarget();
    }
}
