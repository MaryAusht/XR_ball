using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartControllerTrigger : MonoBehaviour
{
    public string restartSceneName = "Restart"; // Ensure this matches the exact name of your scene
    public GameObject mainRigRoot;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (mainRigRoot != null)
            mainRigRoot.SetActive(true);
        else
            Debug.LogWarning("Main Rig not assigned!");
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (mainRigRoot != null)
        {
            mainRigRoot.SetActive(false);
            Debug.Log("Main Rig Deactivated");
        }

        SceneManager.LoadSceneAsync(restartSceneName, LoadSceneMode.Additive);
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (mainRigRoot != null)
        {
            mainRigRoot.SetActive(true);
            Debug.Log("Main Rig Reactivated");
        }

        SceneManager.UnloadSceneAsync(restartSceneName);
    }
}
