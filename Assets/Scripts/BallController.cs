using Oculus.Interaction;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Animator animator;
    private GrabInteractable grabInteractable;

    void Start()
    {
        animator = GetComponent<Animator>();
        grabInteractable = GetComponent<GrabInteractable>();

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
            target.TargetShot();
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.WhenStateChanged -= OnGrabStateChanged;
        }
    }
}
