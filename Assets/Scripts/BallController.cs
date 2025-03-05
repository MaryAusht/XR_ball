using UnityEngine;

public class BallController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        TargetController target = collision.gameObject.GetComponent<TargetController>();

        if (target != null)
        {
            target.TargetShot(); // Now, animation and sound will play first, then destroy
        }
    }
}
