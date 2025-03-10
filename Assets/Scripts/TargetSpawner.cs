using UnityEngine;
using System.Collections;
using TMPro;
using System.Diagnostics;
using System;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Vector3 spawnAreaMin = new Vector3(-2f, 1f, 1f);
    public Vector3 spawnAreaMax = new Vector3(2f, 2f, 4f);
    public float respawnTime = 10f;
    public float gameDuration = 60f; // Total time for the experience
    public TMP_Text countdownText; // Assign in Inspector
    private float remainingTime;

    private GameObject currentTarget;
    private Coroutine respawnCoroutine;
    private bool gameActive = true;

    private void Start()
    {
        StartCoroutine(GameTimerWithUI()); // Start 60s countdown
        SpawnNewTarget();
    }

    private IEnumerator GameTimerWithUI()
    {
        remainingTime = gameDuration;

        while (remainingTime > 0f)
        {
            if (countdownText != null)
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                int milliseconds = Mathf.FloorToInt((remainingTime * 100) % 100);

                countdownText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            }

            yield return null; // Update every frame for milliseconds accuracy
            remainingTime -= Time.deltaTime;
        }

        gameActive = false;

        if (countdownText != null)
        {
            countdownText.text = "00:00:00";
        }

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        //Debug.Log("Game Over!");
    }

    public void SpawnNewTarget()
    {
        if (!gameActive) return;

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            UnityEngine.Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        currentTarget = Instantiate(targetPrefab, randomPosition, Quaternion.identity);

        // Restart the 10s timer for next spawn (if target not hit)
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }
        respawnCoroutine = StartCoroutine(AutoRespawnTimer());
    }

    private IEnumerator AutoRespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);

        if (currentTarget != null && gameActive)
        {
            SpawnNewTarget();
        }
    }
}
