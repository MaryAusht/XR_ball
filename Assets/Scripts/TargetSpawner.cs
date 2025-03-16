using UnityEngine;
using System.Collections;
using TMPro;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Vector3 spawnAreaMin = new Vector3(-2f, 1f, 1f);
    public Vector3 spawnAreaMax = new Vector3(2f, 2f, 4f);
    public float respawnTime = 10f;
    public float gameDuration = 60f;
    public TMP_Text countdownText;
    private float remainingTime;

    private GameObject currentTarget;
    private Coroutine respawnCoroutine;
    private bool gameActive = true;

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
            if (countdownText != null)
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                int milliseconds = Mathf.FloorToInt((remainingTime * 100) % 100);
                countdownText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            }

            yield return null;
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

        GameManager.Instance.EndGame();
    }

    public void StartSpawningTargets()
    {
        if (!gameActive)
        {
            gameActive = true;
            StartCoroutine(GameTimerWithUI());
            SpawnNewTarget();
        }
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
            Destroy(currentTarget);
            SpawnNewTarget();
        }
    }

    public void TargetDestroyed()
    {
        if (!gameActive) return;

        SpawnNewTarget();
    }
}
