using UnityEngine;

public class FloatingTarget : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatHeight = 0.5f;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // If game is paused, skip floating
        if (Time.timeScale == 0f) return;

        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
