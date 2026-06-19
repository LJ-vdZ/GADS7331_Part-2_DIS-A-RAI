using UnityEngine;

public class ProximityPanel : MonoBehaviour
{
    [Header("Appearance")]
    [Tooltip("Minimum scale when player is very close")]
    public float minSize = 0.5f;

    [Tooltip("Maximum scale when player is far away")]
    public float maxSize = 4f;

    [Header("Scaling Behavior")]
    [Tooltip("Distance at which the plane reaches minimum size")]
    public float closeDistance = 3f;

    [Tooltip("Distance at which the plane reaches maximum size")]
    public float farDistance = 15f;

    [Header("Smoothness")]
    [Tooltip("How quickly the plane scales (higher = faster)")]
    public float scaleSpeed = 6f;

    [Header("Rotation / Billboard")]
    [Tooltip("Should the plane always face the player?")]
    public bool lookAtPlayer = true;

    [Tooltip("How fast the plane rotates to face the player (0 = instant)")]
    public float lookSpeed = 8f;

    private Renderer planeRenderer;
    private Collider triggerCollider;
    private Transform playerTransform;
    private bool playerInside = false;

    private void Awake()
    {
        planeRenderer = GetComponent<Renderer>();
        triggerCollider = GetComponent<Collider>();

        if (triggerCollider != null)
            triggerCollider.isTrigger = true;

        // Start hidden
        if (planeRenderer != null)
            planeRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            playerInside = true;
            if (planeRenderer != null)
                planeRenderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (planeRenderer != null)
                planeRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (!playerInside || playerTransform == null)
            return;

        // === Look At Player (Billboard) ===
        if (lookAtPlayer)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);

            // Optional: Lock Z rotation if you want it to stay upright
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        }

        // === Distance-based Scaling ===
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        float t = Mathf.InverseLerp(closeDistance, farDistance, distance);
        float targetSize = Mathf.Lerp(minSize, maxSize, t);

        Vector3 targetScale = new Vector3(targetSize, targetSize, 1f);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }
}
