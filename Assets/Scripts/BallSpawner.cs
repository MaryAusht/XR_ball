using UnityEngine;
public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;  // Assign the Ball Prefab in the Inspector
    public Transform playerCamera; // Assign the Player's Camera (Meta Camera Rig)
    public float spawnDistance = 0.7f; // Distance in front of the player
    public Vector3 spawnOffset = new Vector3(0, -0.2f, 0); // Adjust height slightly
    private GameObject spawnedBall; // Keep track of the spawned ball
    void Start()
    {
        if (spawnedBall == null) // Check if a ball already exists
        {
            SpawnBall(); // Spawn the ball only once
        }
    }
    void SpawnBall()
    {
        if (ballPrefab == null || playerCamera == null)
        {
            Debug.LogError("BallPrefab or PlayerCamera is not assigned!");
            return;
        }
        // Calculate spawn position in front of the camera
        Vector3 spawnPosition = playerCamera.position + (playerCamera.forward * spawnDistance) + spawnOffset;
        // Instantiate the ball and store reference
        spawnedBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }
}