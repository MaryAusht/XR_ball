using UnityEngine;
using Oculus.Interaction;

public class BallController : MonoBehaviour
{
    private Animator animator;
    private GrabInteractable grabInteractable;
    //private AudioSource audioSource;
    //public AudioClip collisionSound; // Assign in Inspector

    void Start()
    {
        animator = GetComponent<Animator>();
        grabInteractable = GetComponent<GrabInteractable>();
        //audioSource = gameObject.AddComponent<AudioSource>(); // Add audio source if not present

        if (grabInteractable != null)
        {
            grabInteractable.WhenStateChanged += OnGrabStateChanged;
        }
    }

    private void OnGrabStateChanged(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select)
        {
            if (animator != null)
            {
                animator.enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TargetController target = collision.gameObject.GetComponent<TargetController>();
        if (target != null)
        {
            // Let the target handle its own behavior (including audio)
            target.TargetShot();
        }
        else
        {
            // If the ball hits any other object (wall, floor, table, etc.), play the environment collision sound
            // PlaySound(collisionSound);
        }
    }

    /*
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    */

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.WhenStateChanged -= OnGrabStateChanged;
        }
    }
}
