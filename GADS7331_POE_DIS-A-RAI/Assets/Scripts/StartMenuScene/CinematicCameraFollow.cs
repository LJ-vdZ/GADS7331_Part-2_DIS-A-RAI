using UnityEngine;

public class CinematicCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Side Camera Settings")]
    public Vector3 sideOffset = new Vector3(45, 18, 5);   // Bigger values
    public float followBehindFactor = 0.25f;

    [Header("Smoothness")]
    public float positionSmooth = 2.5f;
    public float rotationSmooth = 3f;

    [Header("Look At")]
    public Vector3 lookAtOffset = new Vector3(0, 7, 0);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 shipForward = target.forward;

        // Strong side direction
        Vector3 sideDirection = Vector3.Cross(shipForward, Vector3.up).normalized;

        // Changed to +1 for the opposite side
        sideDirection *= 1f;

        // Build final offset
        Vector3 offsetDirection = sideDirection * sideOffset.x
                                + Vector3.up * sideOffset.y
                                + shipForward * sideOffset.z * followBehindFactor;

        Vector3 desiredPosition = target.position + offsetDirection;

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmooth * Time.deltaTime);

        // Look at ship
        Vector3 lookTarget = target.position + lookAtOffset;
        Quaternion desiredRot = Quaternion.LookRotation(lookTarget - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotationSmooth * Time.deltaTime);
    }

    [ContextMenu("Snap Camera Now")]
    public void SnapNow()
    {
        LateUpdate();
    }
}
