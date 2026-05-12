using UnityEngine;

public class ServerLiftController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform endPoint;              // Empty object at the bottom
    public float liftSpeed = 4f;
    public float delayBeforeMoving = 1.5f;

    private Vector3 startPosition;
    private float endY;
    private bool playerInside = false;
    private bool hasMoved = false;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
        endY = endPoint.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasMoved && !playerInside)
        {
            playerInside = true;
            Debug.Log("Player entered simple lift");
            Invoke(nameof(StartMovingDown), delayBeforeMoving);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void StartMovingDown()
    {
        if (hasMoved) return;

        Debug.Log("Simple Lift starting to move down...");
        isMoving = true;
        hasMoved = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move ONLY along Y-axis
            float newY = Mathf.MoveTowards(transform.position.y, endY, liftSpeed * Time.deltaTime);

            // Lock X and Z position
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            // Stop when reached destination
            if (Mathf.Abs(transform.position.y - endY) < 0.05f)
            {
                transform.position = new Vector3(startPosition.x, endY, startPosition.z);
                isMoving = false;
                Debug.Log("Simple Lift arrived at bottom");
            }
        }
    }
}
