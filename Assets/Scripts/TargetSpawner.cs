using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public float minSpawnDistance = 2f;
    public float maxSpawnDistance = 6f;
    public float minHeight = 1f;
    public float maxHeight = 3f;
    public float respawnTime = 10f;
    public float gameDuration = 60f;

    public TMP_Text countdownText;

    private GameObject currentTarget;
    private Coroutine respawnCoroutine;
    private bool gameActive = true;
    private float remainingTime;

    private void Start()
    {
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

        // Hide countdown text
        if (countdownText != null)
        {
            countdownText.text = "00:00:00";
            countdownText.gameObject.SetActive(false); // Hides the text UI
        }

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        GameManager.Instance.EndGame();
    }

    public void SpawnNewTarget()
    {
        if (!gameActive) return;

        // Destroy existing target before spawning a new one
        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        currentTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);

        // Make the target face the player
        Transform playerTransform = Camera.main.transform;
        currentTarget.transform.LookAt(playerTransform);

        // Apply an extra rotation correction (since your Z-axis is rotated 90°)
        currentTarget.transform.rotation *= Quaternion.Euler(0, 180, 90);

        // Reset the respawn timer
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }
        respawnCoroutine = StartCoroutine(AutoRespawnTimer());
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Transform playerTransform = Camera.main.transform;

        float randomDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
        float randomAngle = UnityEngine.Random.Range(0f, 360f);

        // Convert polar coordinates to world position
        Vector3 spawnPosition = playerTransform.position +
                                Quaternion.Euler(0, randomAngle, 0) * (Vector3.forward * randomDistance);

        // Ensure height stays within min and max range
        spawnPosition.y = Mathf.Clamp(playerTransform.position.y + UnityEngine.Random.Range(minHeight, maxHeight),
                                      playerTransform.position.y + minHeight,
                                      playerTransform.position.y + maxHeight);

        return spawnPosition;
    }



    private IEnumerator AutoRespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);

        if (gameActive) // Only destroy and respawn if the game is still running
        {
            if (currentTarget != null)
            {
                Destroy(currentTarget);
            }
            SpawnNewTarget(); // Spawn a new target
        }
    }

    public void TargetDestroyed()
    {
        if (!gameActive) return;
        SpawnNewTarget();
    }
}
