using UnityEngine;

public class OVRRayLaserWithReticle : MonoBehaviour
{
    public Transform rayOrigin;
    public LineRenderer lineRenderer;
    public Transform reticle;
    public float maxDistance = 10f;
    public LayerMask rayLayer;

    void Start()
    {
        // Safe place to assign UI layer mask
        if (rayLayer == 0) // Only assign if not set in Inspector
        {
            rayLayer = LayerMask.GetMask("UI");
        }
    }

    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        Vector3 endPosition;

        if (Physics.Raycast(ray, out hit, maxDistance, rayLayer))
        {
            endPosition = hit.point;
            reticle.position = hit.point;
            reticle.gameObject.SetActive(true);
        }
        else
        {
            endPosition = rayOrigin.position + rayOrigin.forward * maxDistance;
            reticle.position = endPosition;
            reticle.gameObject.SetActive(false);
        }

        lineRenderer.SetPosition(0, rayOrigin.position);
        lineRenderer.SetPosition(1, endPosition);
    }
}
