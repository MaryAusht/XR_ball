using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRayButtonHandler : MonoBehaviour
{
    // This method MUST be public, non-static, and take NO parameters
    public void RestartGame()
    {
        SceneManager.LoadScene("CountdownTimer");
    }
}
