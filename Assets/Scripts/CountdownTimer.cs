using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 3f; // Time in seconds
    private float currentTime;

    public TextMeshProUGUI countdownText;

    private bool hasShot = false;

    void Start()
    {
        currentTime = countdownTime;
        UpdateText();
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0); // Clamp to 0
            UpdateText();
        }
        else if (!hasShot)
        {
            countdownText.text = "Shoot!";
            hasShot = true;

            // Delay the scene change by 1 second (optional)
            Invoke("LoadNextScene", 2f);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Main");
    }
    void UpdateText()
    {
        countdownText.text = Mathf.CeilToInt(currentTime).ToString();
    }
}
