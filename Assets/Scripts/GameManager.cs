using TMPro;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TMP_Text scoreText;
    public GameObject gameOverUI;
    public TMP_Text finalScoreText;
    public TargetSpawner targetSpawner;
    public GameObject welcomeUI;
    public float gameDuration = 60f;

    private bool isGameOver = false;
    private Coroutine gameTimerCoroutine;
    private float remainingTime; // Timer

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ResetUI();

        score = GameStateManager.score;

        if (GameStateManager.timeRemaining > 0f && GameStateManager.isPaused)
        {
            remainingTime = GameStateManager.timeRemaining;
        }
        else
        {
            remainingTime = gameDuration;
        }

        UpdateScoreUI();
    }

    private void ResetUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (finalScoreText != null) finalScoreText.gameObject.SetActive(false);
        if (welcomeUI != null) welcomeUI.SetActive(true);
    }

    public void StartGame()
    {
        if (Time.timeScale == 0f)
        {
            Debug.Log("Game is paused. StartGame() aborted.");
            return;
        }

        if (targetSpawner == null)
        {
            Debug.Log("TargetSpawner is NOT assigned in GameManager!");
            return;
        }

        ResetUI();
        if (welcomeUI != null) welcomeUI.SetActive(false);

        score = GameStateManager.score;
        UpdateScoreUI();
        isGameOver = false;

        targetSpawner.SpawnNewTarget();

        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
        }

        gameTimerCoroutine = StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        while (remainingTime > 0f)
        {
            if (!GameStateManager.isPaused)
            {
                remainingTime -= Time.deltaTime;
                GameStateManager.timeRemaining = remainingTime;
            }

            yield return null;
        }

        EndGame();
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
        GameStateManager.score = score;

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.Log("ScoreText UI is NOT assigned in GameManager!");
        }
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public void EndGame()
    {
        if (GameStateManager.isPaused) return;

        isGameOver = true;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.text = "Final Score: " + score;
        }

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }

        GameStateManager.score = 0;
        GameStateManager.timeRemaining = gameDuration;
        GameStateManager.isPaused = false;
    }
}
