using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour, ITargetInterface
{
    public Animator animator; // Reference to Animator
    public AudioSource audioSource; // Reference to AudioSource

    [SerializeField] private Material originalMaterial; // Default target material
    [SerializeField] private Material hologramMaterial; // Hologram shader material

    private Renderer targetRenderer; // Renderer component
    private bool isHit = false; // Prevent multiple triggers

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>(); // Get Renderer
        if (targetRenderer == null)
        {
            UnityEngine.Debug.LogError("Renderer not found on target!");
        }
        else
        {
            originalMaterial = targetRenderer.material; // Store original material at start
        }
    }

    public void TargetShot()
    {
        if (isHit) return; // Prevent multiple hits
        isHit = true;

        ChangeToHologram(); // Switch to hologram shader
        PlayAnimation();
        PlayAudio();

        // Start coroutine to destroy after effects
        StartCoroutine(DestroyAfterEffects());
    }

    private void ChangeToHologram()
    {
        if (targetRenderer != null && hologramMaterial != null)
        {
            targetRenderer.material = hologramMaterial;
        }
        else
        {
            UnityEngine.Debug.LogError("Hologram Material or Renderer missing!");
        }
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
            animator.SetTrigger("Hit"); // Ensure "Hit" is a trigger in Animator
        }
    }

    private IEnumerator DestroyAfterEffects()
    {
        // Wait for animation and sound to finish
        float delay = Mathf.Max(GetAnimationLength(), audioSource.clip.length);
        yield return new WaitForSeconds(delay);

        Destroy(gameObject); // Destroy the target after effects
    }

    private float GetAnimationLength()
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "Hit") // Ensure animation name is correct
                {
                    return clip.length;
                }
            }
        }
        return 1f; // Default fallback time
    }
}
