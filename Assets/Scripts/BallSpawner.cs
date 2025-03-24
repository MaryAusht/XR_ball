using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;           // Assign the ball prefab in Inspector
    public float spawnSpeed = 5f;       // Adjust as needed
    public AudioClip shootSound;        // Assign in Inspector

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 0.6f;
    }

    void Update()
    {
        // Don't allow spawning while paused
        if (Time.timeScale == 0f) return;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            SpawnBall();
        }
    }

    void SpawnBall()
    {
        GameObject spawnedBall = Instantiate(prefab, transform.position, Quaternion.identity);

        Rigidbody spawnedBallRB = spawnedBall.GetComponent<Rigidbody>();
        if (spawnedBallRB != null)
        {
            spawnedBallRB.velocity = transform.forward * spawnSpeed;
        }

        spawnedBall.AddComponent<BallCollisionHandler>();

        PlayShootSound();
    }

    void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
