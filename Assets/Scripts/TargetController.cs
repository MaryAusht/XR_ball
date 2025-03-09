using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour, ITargetInterface
{
    public Animator animator;  // Reference to Animator
    public AudioSource audioSource;  // Reference to AudioSource
    private bool isHit = false; // Prevent multiple triggers
    public float destroyDelay = 2f; // Time before destroying the object

    public void TargetShot()
    {
        if (isHit) return; // Prevent multiple hits
        isHit = true;

        GameManager.Instance.AddScore(5); // Add 5 points when hit

        PlayAnimation();
        PlayAudio();

        StartCoroutine(DestroyAfterDelay());
    }

    public void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void PlayAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit"); // Make sure "Hit" trigger exists in Animator
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay); // Wait exactly 5 seconds

        Destroy(gameObject); // Destroy the target after 5 seconds
    }
}
