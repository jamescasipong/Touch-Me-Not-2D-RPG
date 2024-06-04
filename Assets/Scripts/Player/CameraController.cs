using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{



    public Transform target;
    public LayerMask obstacleMask;
    public float smoothSpeed = 5f;
    public float cameraZOffset = -10f;
    public float collisionRadius = 0.2f;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, cameraZOffset);
        Vector3 adjustedPosition = HandleCollisions(targetPosition);

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, adjustedPosition, smoothSpeed * Time.deltaTime);
    }

    Vector3 HandleCollisions(Vector3 targetPosition)
    {
        RaycastHit2D hit = Physics2D.CircleCast(targetPosition, collisionRadius, Vector2.zero, 0f, obstacleMask);

        if (hit.collider != null)
        {
            Vector3 hitPoint = hit.point;
            hitPoint.z = cameraZOffset;
            return hitPoint;
        }

        return targetPosition;
    }
}
