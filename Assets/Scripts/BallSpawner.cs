using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallSpawner : MonoBehaviour
{
    public GameObject prefab; // Assign the ball prefab in Inspector
    public float spawnSpeed = 5f; // Adjust as needed
    public AudioClip shootSound; // Assign in Inspector
    private AudioSource audioSource;
    void Start()
    {
        // Ensure the GameObject has an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
        audioSource.volume = 0.6f;
    }
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
            // Play the shooting sound
            PlayShootSound();
        }
    }
    void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}






