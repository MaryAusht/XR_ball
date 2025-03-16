using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour, ITargetInterface
{
    public Animator animator;
    public AudioSource audioSource;

    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material hologramMaterial;

    private Renderer targetRenderer;
    private bool isHit = false;

    private TargetSpawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<TargetSpawner>();
    }

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            Debug.LogError("Renderer not found on target!");
        }
        else
        {
            originalMaterial = targetRenderer.material;
        }
    }

    public void TargetShot()
    {
        if (isHit) return;
        isHit = true;

        GameManager.Instance.AddScore(5); // <--- This updates the score

        ChangeToHologram();
        PlayAnimation();
        PlayAudio();

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
            Debug.LogError("Hologram Material or Renderer missing!");
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
            animator.SetTrigger("Hit");
        }
    }

    private IEnumerator DestroyAfterEffects()
    {
        float delay = Mathf.Max(GetAnimationLength(), audioSource != null ? audioSource.clip.length : 0f);
        yield return new WaitForSeconds(delay);

        if (spawner != null)
        {
            spawner.TargetDestroyed();
        }

        Destroy(gameObject);
    }

    private float GetAnimationLength()
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "Hit")
                {
                    return clip.length;
                }
            }
        }
        return 1f;
    }
}
