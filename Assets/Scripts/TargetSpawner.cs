using UnityEngine;
using System.Collections;
using TMPro;

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
                countdownText.text = "Time Left: " + Mathf.CeilToInt(remainingTime).ToString() + "s";
            }

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        gameActive = false;

        if (countdownText != null)
        {
            countdownText.text = "Time's Up!";
        }

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        Debug.Log("Game Over!");
    }


    public void SpawnNewTarget()
    {
        if (!gameActive) return;

        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }

        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
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
