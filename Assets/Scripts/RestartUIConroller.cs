using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartUIController : MonoBehaviour
{
    public GameObject restartUI; // Assign the UI panel in Inspector
    void Start()
    {
        restartUI.SetActive(false); // Ensure UI is hidden at the start
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("CountdownTimer");
    }
    public void ResumeGame()
    {
        restartUI.SetActive(false); // Hide the UI panel
    }
    public void ExitGame()
    {
        Application.Quit(); // Closes the application
    }
}