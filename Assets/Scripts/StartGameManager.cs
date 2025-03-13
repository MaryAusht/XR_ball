using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour
{
    public string gameSceneName = "Main"; // Change to your game scene name

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
