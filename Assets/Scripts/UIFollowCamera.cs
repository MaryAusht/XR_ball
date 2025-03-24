using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowCamera : MonoBehaviour
{
    public float distanceFromCamera = 2f;
    public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if (Camera.main == null) return;

        Transform cam = Camera.main.transform;
        Vector3 targetPosition = cam.position + cam.forward * distanceFromCamera + offset;

        transform.position = targetPosition;
        transform.LookAt(cam);
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
    }
}