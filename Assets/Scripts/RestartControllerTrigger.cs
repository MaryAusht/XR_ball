using UnityEngine;

public class RestartControllerTrigger : MonoBehaviour
{
    public GameObject uiPanel; // Assign in Inspector
    public GameObject rayInteractorObject; // Assign Ray Interactor here
    private bool isPaused = false;

    void Start()
    {
        uiPanel.SetActive(false);
        rayInteractorObject.SetActive(false); // Hide ray at start
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            ToggleUIPanel();
        }
    }

    void ToggleUIPanel()
    {
        isPaused = !isPaused;
        uiPanel.SetActive(isPaused);
        rayInteractorObject.SetActive(isPaused); // Show/hide ray

        if (isPaused)
        {
            GameManager.Instance.targetSpawner.SetPaused(true);
        }
        else
        {
            GameManager.Instance.targetSpawner.SetPaused(false);
        }
    }
}
