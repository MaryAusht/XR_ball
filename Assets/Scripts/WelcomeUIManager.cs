using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WelcomeUIManager : MonoBehaviour
{
    public GameObject welcomePanel; // The Welcome UI Panel
    public TMP_Text welcomeText;    // The Instruction Text
    public Button startButton;      // The Start Button

    void Start()
    {
        // Ensure the panel is active at the beginning
        welcomePanel.SetActive(true);
        startButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        // Destroy the Welcome UI when the game starts
        Destroy(welcomePanel);

        // Destroy this script component as well to free resources
        Destroy(gameObject);

        // Start the game via GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            UnityEngine.Debug.LogError("GameManager instance is missing!");
        }
    }
}
