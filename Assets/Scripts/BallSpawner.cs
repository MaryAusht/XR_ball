using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab; // Assign the ball prefab in Inspector
    public float spawnSpeed = 5f; // Adjust as needed

    void Update()
    {
        // Check if the secondary index trigger is pressed on the controller
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            // Instantiate a new ball at the spawner's position
            GameObject spawnedBall = Instantiate(prefab, transform.position, Quaternion.identity);

            // Get the Rigidbody component of the spawned ball
            Rigidbody spawnedBallRB = spawnedBall.GetComponent<Rigidbody>();

            // Apply forward velocity to shoot the ball
            if (spawnedBallRB != null)
            {
                spawnedBallRB.velocity = transform.forward * spawnSpeed;
            }

            // Add the BallCollisionHandler script to track collisions
            spawnedBall.AddComponent<BallCollisionHandler>();
        }
    }
}
