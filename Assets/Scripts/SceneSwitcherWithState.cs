using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherWithState : MonoBehaviour
{
    public string mainScene = "Main";
    public string restartScene = "Restart";

    private bool isInRestartScene;

    private void Start()
    {
        isInRestartScene = SceneManager.GetActiveScene().name == restartScene;
        GameStateManager.isPaused = isInRestartScene;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            if (isInRestartScene)
            {
                SceneManager.LoadScene(mainScene, LoadSceneMode.Single);
            }
            else
            {
                // Save current time before pausing
                if (GameManager.Instance != null)
                {
                    GameStateManager.timeRemaining = GameManager.Instance.GetRemainingTime();
                }

                GameStateManager.isPaused = true;
                SceneManager.LoadScene(restartScene, LoadSceneMode.Single);
            }
        }
    }
}
