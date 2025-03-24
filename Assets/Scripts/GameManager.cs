using TMPro;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TMP_Text scoreText;
    public TMP_Text countdownText;
    public GameObject gameOverUI;
    public TMP_Text finalScoreText;
    public TargetSpawner targetSpawner;
    public GameObject welcomeUI;
    public float gameDuration = 60f;

    private bool isGameOver = false;
    private Coroutine gameTimerCoroutine;
    private float remainingTime;

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
        UnityEngine.Debug.Log("GameManager started. Initializing game state.");
        ResetUI();

        GameStateManager.isPaused = false;
        Time.timeScale = 1f;

        score = GameStateManager.score;
        remainingTime = GameStateManager.timeRemaining > 0f ? GameStateManager.timeRemaining : gameDuration;

        UpdateScoreUI();
        UpdateCountdownUI();

        StartGame();
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
            UnityEngine.Debug.LogWarning("Game is paused! Resuming game...");
            Time.timeScale = 1f;
        }

        if (targetSpawner == null)
        {
            UnityEngine.Debug.LogError("TargetSpawner is NOT assigned in GameManager!");
            return;
        }

        UnityEngine.Debug.Log("Game started!");

        ResetUI();
        if (welcomeUI != null) welcomeUI.SetActive(false);

        if (GameStateManager.timeRemaining <= 0f) // Only reset score if starting a new game
        {
            score = 0;
            GameStateManager.score = 0;
        }
        else
        {
            score = GameStateManager.score; // Keep previous score after pause
        }

        UpdateScoreUI();
        isGameOver = false;

        targetSpawner.SpawnNewTarget();

        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
        }

        remainingTime = GameStateManager.timeRemaining > 0f ? GameStateManager.timeRemaining : gameDuration;
        gameTimerCoroutine = StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        UnityEngine.Debug.Log("Timer started with time: " + remainingTime);

        while (remainingTime > 0f)
        {
            if (!GameStateManager.isPaused)
            {
                remainingTime -= Time.deltaTime;
                GameStateManager.timeRemaining = remainingTime;
                UpdateCountdownUI();
                UnityEngine.Debug.Log("Time left: " + remainingTime);
            }

            yield return null;
        }

        UnityEngine.Debug.Log("Time is up! Ending game.");
        EndGame();
    }

    public void PauseGame()
    {
        UnityEngine.Debug.Log("Game paused.");
        GameStateManager.isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        UnityEngine.Debug.Log("Game resumed.");
        GameStateManager.isPaused = false;
        Time.timeScale = 1f;

        if (gameTimerCoroutine == null) // Resume only if no active timer
        {
            gameTimerCoroutine = StartCoroutine(GameTimer());
        }
    }

    private void UpdateCountdownUI()
    {
        if (countdownText != null)
        {
            float remaining = GameStateManager.timeRemaining;
            int minutes = Mathf.FloorToInt(remaining / 60);
            int seconds = Mathf.FloorToInt(remaining % 60);
            int milliseconds = Mathf.FloorToInt((remaining * 100) % 100);
            countdownText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        UnityEngine.Debug.Log("UI Updated: " + GameStateManager.timeRemaining);
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
    }

    public void EndGame()
    {
        if (GameStateManager.isPaused) return;

        UnityEngine.Debug.Log("Game Over!");
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

    public float GetRemainingTime()
    {
        return GameStateManager.timeRemaining;
    }
}
