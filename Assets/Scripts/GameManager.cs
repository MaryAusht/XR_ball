using TMPro; // Import TMP namespace
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TMP_Text scoreText; // UI Text for score display
    public GameObject gameOverUI; // UI Panel for Game Over
    public TMP_Text finalScoreText; // UI Text to show final score

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
        Invoke(nameof(EndGame), gameDuration); // Start game timer
        UpdateScoreUI();
        finalScoreText.gameObject.SetActive(false);
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
            Debug.LogError("ScoreText UI is NOT assigned in GameManager!");
        }
    }

    private void EndGame()
    {
        isGameOver = true;
        gameOverUI.SetActive(true); // Show Game Over UI
        finalScoreText.gameObject.SetActive(true);
        finalScoreText.text = "Your Score: " + score;
    }
}
