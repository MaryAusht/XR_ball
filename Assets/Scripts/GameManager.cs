using System.Diagnostics;
using TMPro; // Import TMP namespace
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton for GameManager

    public int score = 0;
    public TMP_Text scoreText; // UI Text for score display
    public GameObject gameOverUI; // UI Panel for Game Over
    public TMP_Text finalScoreText; // UI Text to show final score
    public TargetSpawner targetSpawner; // Reference to TargetSpawner (Assign in Inspector)
    public GameObject welcomeUI; // Reference to the Welcome UI Panel

    public float gameDuration = 60f; // Game duration in seconds
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverUI.SetActive(false); // Hide Game Over UI at the start
        finalScoreText.gameObject.SetActive(false);

        if (welcomeUI != null)
        {
            welcomeUI.SetActive(true); // Show welcome UI at the start
        }
        else
        {
            UnityEngine.Debug.LogError("Your error message here"); 
        }
    }

    public void StartGame()
    {
        if (targetSpawner == null)
        {
            UnityEngine.Debug.LogError("TargetSpawner is NOT assigned in GameManager!");
            return;
        }

        if (welcomeUI != null)
        {
            welcomeUI.SetActive(false); // Hide welcome UI when game starts
        }

        gameOverUI.SetActive(false); // Hide Game Over UI
        finalScoreText.gameObject.SetActive(false);

        score = 0; // Reset score
        UpdateScoreUI();

        isGameOver = false; // Reset game over state

        targetSpawner.StartSpawningTargets(); // Start spawning targets
        Invoke(nameof(EndGame), gameDuration); // Start game timer
    }

    public void AddScore(int points)
    {
        if (isGameOver) return; // Stop updating score after game over
        score += points;
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
            UnityEngine.Debug.LogError("ScoreText UI is NOT assigned in GameManager!");
        }
    }

    public void EndGame()
    {
        isGameOver = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.text = "Final Score: " + score;
        }
    }
}
