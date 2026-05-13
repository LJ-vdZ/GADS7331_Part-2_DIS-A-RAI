using UnityEngine;

public class ShipPatrol : MonoBehaviour
{
    [Header("Orbit Target")]
    public Transform target;

    [Header("Orbit Settings")]
    public float orbitRadius = 45f;
    public float orbitSpeed = 8f;           // degrees per second
    public float orbitHeight = 12f;

    [Header("Movement Style")]
    public float smoothFactor = 4f;         // Lower = smoother, less snappy
    public float bobSpeed = 1f;
    public float bobAmount = 1.5f;
    public float tiltAmount = 12f;

    private float angle = 0f;
    private Vector3 startOffset;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("ShipPatrol: Target (Big Ship) is not assigned!");
            enabled = false;
            return;
        }

        // Start from current position (very important!)
        Vector3 flatDirection = (transform.position - target.position);
        flatDirection.y = 0;
        angle = Mathf.Atan2(flatDirection.x, flatDirection.z) * Mathf.Rad2Deg;

        // Force correct initial distance
        transform.position = target.position + flatDirection.normalized * orbitRadius + Vector3.up * 5f;
    }

    void Update()
    {
        angle += orbitSpeed * Time.deltaTime;

        float x = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;
        float z = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
        float y = Mathf.Sin(angle * 0.6f * Mathf.Deg2Rad) * orbitHeight;

        Vector3 desiredPos = target.position + new Vector3(x, y, z);

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothFactor * Time.deltaTime);

        // Gentle bob
        transform.position += Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobAmount * 0.02f;

        // Look at target + banking
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion baseRotation = Quaternion.LookRotation(directionToTarget);

        float bank = Mathf.Sin(angle * Mathf.Deg2Rad) * tiltAmount;
        Quaternion finalRotation = baseRotation * Quaternion.Euler(0, 0, -bank);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, 5f * Time.deltaTime);
    }
}
