using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    private int collisionCount = 0; // Track collisions
    public int maxCollisions = 3; // Set max collisions before destruction

    private void OnCollisionEnter(Collision collision)
    {
        collisionCount++; // Increment on each collision

        if (collisionCount >= maxCollisions)
        {
            Destroy(gameObject); // Destroy the ball after max collisions
        }
    }
}
