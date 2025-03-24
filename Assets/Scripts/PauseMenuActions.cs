using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuActions : MonoBehaviour
{
    // Call this from Restart button
    public string countdownSceneName = "CountdownTimer";

    public void RestartGame()
    {
        GameStateManager.score = 0;
        GameStateManager.timeRemaining = 60f;
        GameStateManager.isPaused = false;

        SceneManager.LoadScene(countdownSceneName, LoadSceneMode.Single);
    }

    // Call this from Quit button
    public void QuitGame()
    {
        Debug.Log("Quit game"); // This will show in editor
        Application.Quit(); // This works in builds
    }
}
