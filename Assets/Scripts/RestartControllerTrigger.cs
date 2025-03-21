using UnityEngine;
public class RestartControllerTrigger : MonoBehaviour
{
    public GameObject uiPanel; // Assign the UI panel in Inspector
    private bool isPaused = false; // Track game state
    void Start()
    {
        uiPanel.SetActive(false); // Hide UI panel at start
        Time.timeScale = 1f; // Ensure the game starts unpaused
    }
    void Update()
    {
        // Check if the Secondary Hand Trigger (Grip button) is pressed
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            ToggleUIPanel(); // Show/hide UI and pause/resume game
        }
    }
    void ToggleUIPanel()
    {
        isPaused = !isPaused; // Toggle pause state
        uiPanel.SetActive(isPaused); // Show or hide UI panel
        if (isPaused)
        {
            Time.timeScale = 0f; // Pause game
        }
        else
        {
            Time.timeScale = 1f; // Resume game
        }
    }
}