using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI interactions
public class UIBallController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip buttonClickSound; // Assign a button press sound in Inspector
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Ensure AudioSource exists
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the ball collides with a UI button
        Button button = other.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke(); // Simulate button click
            PlayButtonSound();
        }
    }
    void PlayButtonSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}