using UnityEngine;

public class FloatingTarget : MonoBehaviour
{
    public float floatSpeed = 2f;  // Speed of floating motion
    public float floatHeight = 0.5f;  // How high it moves up and down
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position; // Store the spawn position
    }

    private void Update()
    {
        // Make the object move up and down using a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
