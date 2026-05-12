using UnityEngine;

public class LiftController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform endPoint;                  // Empty object at bottom
    public float liftSpeed = 4f;
    public float delayBeforeClosing = 1.8f;

    [Header("Door Settings")]
    public Transform liftDoor;                  // Single door
    public float doorOpenHeight = 4f;           // How high the door slides up
    public float doorSpeed = 3f;

    private Vector3 startPosition;
    private float endY;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;

    private bool playerInside = false;
    private bool hasMoved = false;
    private bool doorsOpen = true;
    private bool isMoving = false;
    private bool isDoorMoving = false;

    private void Awake()
    {
        startPosition = transform.position;
        endY = endPoint.position.y;

        if (liftDoor != null)
        {
            doorClosedPos = liftDoor.position;
            doorOpenPos = doorClosedPos + Vector3.up * doorOpenHeight;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasMoved && !playerInside)
        {
            playerInside = true;
            Debug.Log("Player entered lift");
            Invoke(nameof(StartLiftSequence), delayBeforeClosing);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hasMoved)
        {
            playerInside = false;
            Debug.Log("Player exited lift");
            CloseDoor();
        }
    }

    private void StartLiftSequence()
    {
        if (!playerInside || hasMoved) return;

        Debug.Log("Closing door and starting lift...");
        CloseDoor();
        Invoke(nameof(StartMovingDown), 1.2f);
    }

    private void StartMovingDown()
    {
        isMoving = true;
        hasMoved = true;
    }

    private void Update()
    {
        // Move lift ONLY along Y-axis
        if (isMoving)
        {
            float newY = Mathf.MoveTowards(transform.position.y, endY, liftSpeed * Time.deltaTime);

            // Lock X and Z, only change Y
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            if (Mathf.Abs(transform.position.y - endY) < 0.05f)
            {
                transform.position = new Vector3(startPosition.x, endY, startPosition.z);
                isMoving = false;
                OpenDoor();
                Debug.Log("Lift arrived at bottom - Door opening");
            }
        }

        // Door animation
        if (isDoorMoving && liftDoor != null)
        {
            Vector3 targetPos = doorsOpen ? doorOpenPos : doorClosedPos;
            liftDoor.position = Vector3.MoveTowards(liftDoor.position, targetPos, doorSpeed * Time.deltaTime);

            if (Vector3.Distance(liftDoor.position, targetPos) < 0.05f)
            {
                liftDoor.position = targetPos;
                isDoorMoving = false;
            }
        }
    }

    public void OpenDoor()
    {
        doorsOpen = true;
        isDoorMoving = true;
    }

    public void CloseDoor()
    {
        doorsOpen = false;
        isDoorMoving = true;
    }
}
