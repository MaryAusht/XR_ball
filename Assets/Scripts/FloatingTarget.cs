using UnityEngine;

public class FloatingTarget : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatHeight = 0.5f;
    private Vector3 startPos;

    private bool isPaused = false;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (isPaused) return; // Stop floating when paused

        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }
}
