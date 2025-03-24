using UnityEngine;
using System.Collections;
using TMPro;
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

        if (!GameStateManager.isPaused || GameStateManager.timeRemaining <= 0f)
        {
            GameStateManager.timeRemaining = gameDuration;
        }

        StartCoroutine(GameTimerWithUI());
        SpawnNewTarget();
    }

    private IEnumerator GameTimerWithUI()
    {
        while (GameStateManager.timeRemaining > 0f)
        {
            if (!PauseManager.isPaused && Time.timeScale != 0f)
            {
                GameStateManager.timeRemaining -= Time.deltaTime;
                UpdateCountdownUI();
            }

            yield return null;
        }

        EndGame();
    }

    private void UpdateCountdownUI()
    {
        if (PauseManager.isPaused || Time.timeScale == 0f)
            return;

        if (countdownText != null)
        {
            float remaining = GameStateManager.timeRemaining;
            int minutes = Mathf.FloorToInt(remaining / 60);
            int seconds = Mathf.FloorToInt(remaining % 60);
            int milliseconds = Mathf.FloorToInt((remaining * 100) % 100);
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

        GameStateManager.timeRemaining = 0f;
        GameManager.Instance.EndGame();
    }

    public void SpawnNewTarget()
    {
        if (!gameActive || PauseManager.isPaused || Time.timeScale == 0f || mrukRoom == null) return;

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

        // Shrink room bounds to avoid walls
        float margin = 0.3f;
        bounds.Expand(new Vector3(-margin * 2f, 0, -margin * 2f));

        Vector3 playerPosition = Camera.main.transform.position;

        for (int i = 0; i < 30; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            float y = bounds.max.y + 1f; // Start raycast from above

            Vector3 rayOrigin = new Vector3(x, y, z);

            // Raycast downward to find a floor
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo, 5f))
            {
                float spawnHeight = Mathf.Clamp(hitInfo.point.y + Random.Range(minHeight, maxHeight), bounds.min.y + margin, bounds.max.y - margin);
                Vector3 candidate = new Vector3(x, spawnHeight, z);
                float distance = Vector3.Distance(playerPosition, candidate);

                if (distance >= minSpawnDistance && distance <= maxSpawnDistance)
                {
                    return candidate;
                }
            }
        }

        Debug.LogWarning("Could not find valid spawn. Using fallback.");
        return playerPosition + Vector3.forward * Mathf.Clamp(maxSpawnDistance, 1f, 5f);
    }




    private IEnumerator AutoRespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);

        if (gameActive && !PauseManager.isPaused && Time.timeScale != 0f)
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
        if (!gameActive || PauseManager.isPaused) return;
        SpawnNewTarget();
    }
}
