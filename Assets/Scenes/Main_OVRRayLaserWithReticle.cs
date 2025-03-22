using UnityEngine;
using UnityEngine.UI;

public class Main_OVRRayLaserWithReticle : MonoBehaviour
{
    public Transform rayOrigin;
    public LineRenderer lineRenderer;
    public Transform reticle;
    public float maxDistance = 10f;
    public LayerMask rayLayer;

    private GameObject lastHitObject;

    void Start()
    {
        if (rayLayer == 0)
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

            // Store the object hit
            lastHitObject = hit.collider.gameObject;

            // Check for trigger press to click
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Button button = lastHitObject.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                }
            }
        }
        else
        {
            endPosition = rayOrigin.position + rayOrigin.forward * maxDistance;
            reticle.position = endPosition;
            reticle.gameObject.SetActive(false);
            lastHitObject = null;
        }

        lineRenderer.SetPosition(0, rayOrigin.position);
        lineRenderer.SetPosition(1, endPosition);
    }
}
