using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton
    public int score = 0;
    public TMP_Text scoreText; // UI Text for score display
    public GameObject gameOverUI; // UI Panel for Game Over
    public TMP_Text finalScoreText; // UI Text for final score
    public TargetSpawner targetSpawner; // Reference to TargetSpawner (Assign in Inspector)
    public GameObject welcomeUI; // Welcome UI Panel
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
            return;
        }
    }
    private void Start()
    {
        ResetUI();
        Time.timeScale = 1f; // Ensure the game starts unpaused
    }
    private void ResetUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (finalScoreText != null) finalScoreText.gameObject.SetActive(false);
        if (welcomeUI != null) welcomeUI.SetActive(true);
    }
    public void StartGame()
    {
        if (targetSpawner == null)
        {
            Debug.Log("TargetSpawner is NOT assigned in GameManager!");
            return;
        }
        ResetUI(); // Hide game over UI and reset score
        if (welcomeUI != null) welcomeUI.SetActive(false);
        score = 0;
        UpdateScoreUI();
        isGameOver = false;
        Time.timeScale = 1f; // Resume game when it starts
        targetSpawner.SpawnNewTarget(); // Start target spawning
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
            Debug.Log("ScoreText UI is NOT assigned in GameManager!");
        }
    }
    public void EndGame()
    {
        if (Time.timeScale == 0f) return; // Prevent ending the game if paused
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
        // Hide score UI
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
    }
}