using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;           // Assign the ball prefab in Inspector
    public float spawnSpeed = 5f;       // Adjust as needed
    public AudioClip shootSound;        // Assign in Inspector
    public GameObject uiPanel;          // Assign the pause UI panel in Inspector

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
        // Only spawn ball if UI is NOT active
        if (!uiPanel.activeSelf && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            SpawnBall();
        }

        if (!uiPanel.activeSelf && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
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
