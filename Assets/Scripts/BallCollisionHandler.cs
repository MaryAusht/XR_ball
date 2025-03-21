using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    private int collisionCount = 0; // Track collisions
    public int minCollisions = 3;
    public int maxCollisions = 5; // Set max collisions before destruction

    private void OnCollisionEnter(Collision collision)
    {
        collisionCount++;

        if (collisionCount >= minCollisions && collisionCount <= maxCollisions)
        {
            // Do something optional here
        }

        if (collisionCount >= maxCollisions)
        {
            Destroy(gameObject);
        }
    }
}