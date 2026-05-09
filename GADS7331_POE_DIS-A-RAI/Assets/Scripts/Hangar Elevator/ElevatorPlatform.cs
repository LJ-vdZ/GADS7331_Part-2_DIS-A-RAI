using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    [Header("Positions")]
    public Transform upperPosition;   // Top floor position
    public Transform lowerPosition;   // Bottom floor position (new)

    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float delayBeforeLift = 1.5f;

    [Header("References")]
    public HangarDoorController doorController;
    public BoxCollider platformTrigger;

    private Vector3 startPosition;
    private bool isMoving = false;
    private bool playerOnPlatform = false;

    private enum ElevatorState { Lower, Upper }
    private ElevatorState currentState = ElevatorState.Lower;

    void Start()
    {
        startPosition = transform.position;

        if (platformTrigger == null)
        {
            platformTrigger = GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();
            platformTrigger.isTrigger = true;
        }

        // Set initial state
        if (Mathf.Abs(transform.position.y - upperPosition.position.y) < 0.5f)
            currentState = ElevatorState.Upper;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving && !playerOnPlatform)
        {
            playerOnPlatform = true;
            Invoke(nameof(StartMovement), delayBeforeLift);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    private void StartMovement()
    {
        if (isMoving) return;

        if (currentState == ElevatorState.Lower)
            StartCoroutine(MoveToPosition(upperPosition.position.y));
        else
            StartCoroutine(MoveToPosition(lowerPosition.position.y));
    }

    private System.Collections.IEnumerator MoveToPosition(float targetY)
    {
        isMoving = true;
        Vector3 startPos = transform.position;

        while (Mathf.Abs(transform.position.y - targetY) > 0.001f)
        {
            float newY = Mathf.MoveTowards(transform.position.y, targetY, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(startPos.x, newY, startPos.z);
            yield return null;
        }

        transform.position = new Vector3(startPos.x, targetY, startPos.z);

        currentState = (currentState == ElevatorState.Lower) ? ElevatorState.Upper : ElevatorState.Lower;
        isMoving = false;

        if (currentState == ElevatorState.Upper && doorController != null)
            doorController.PlatformArrived();
    }

    //Public method to call elevator from button
    public void CallElevatorToUpper()
    {
        if (!isMoving && currentState == ElevatorState.Lower)
            StartCoroutine(MoveToPosition(upperPosition.position.y));
    }

    public void CallElevatorToLower()
    {
        if (!isMoving && currentState == ElevatorState.Upper)
            StartCoroutine(MoveToPosition(lowerPosition.position.y));
    }
}
