using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour, ITargetInterface
{
    public Animator animator;
    public AudioSource audioSource;
    private bool isHit = false;
    public float destroyDelay = 2f;
    private TargetSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<TargetSpawner>(); // Find the spawner
    }

    public void TargetShot()
    {
        if (isHit) return;
        isHit = true;

        GameManager.Instance.AddScore(5);

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
            animator.SetTrigger("Hit");
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        if (spawner != null)
        {
            spawner.SpawnNewTarget(); // Spawn a new target immediately
        }

        Destroy(gameObject);
    }
}
