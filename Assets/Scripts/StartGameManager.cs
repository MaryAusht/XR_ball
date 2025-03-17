using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour
{
    public string gameSceneName = "CountdownTimer"; // Change to your game scene name

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
