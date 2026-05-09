using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door References")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Movement Settings")]
    public float openDistance = 2.5f;
    public float openSpeed = 2f;
    public float delayBeforeOpen = 0f;

    [Header("Movement Axis")]
    public bool moveAlongZ = true;

    private Vector3 leftClosedPos; 
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private bool doorsOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        if (leftDoor == null || rightDoor == null)
        {
            Debug.LogError("LeftDoor and RightDoor must be assigned on " + gameObject.name);
            return;
        }

        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;

        CalculateOpenPositions();
    }

    private void CalculateOpenPositions()
    {
        if (moveAlongZ)
        {
            // Left door  > Positive Z
            // Right door > Negative Z
            leftOpenPos = leftClosedPos + new Vector3(0, 0, openDistance);
            rightOpenPos = rightClosedPos + new Vector3(0, 0, -openDistance);
        }
        else
        {
            leftOpenPos = leftClosedPos + new Vector3(openDistance, 0, 0);
            rightOpenPos = rightClosedPos + new Vector3(-openDistance, 0, 0);
        }
    }

    public void OpenDoors()
    {
        if (doorsOpen || isMoving) return;

        if (delayBeforeOpen > 0f)
            Invoke(nameof(StartOpening), delayBeforeOpen);
        else
            StartOpening();
    }

    private void StartOpening()
    {
        StartCoroutine(OpenDoorsCoroutine());
    }

    private System.Collections.IEnumerator OpenDoorsCoroutine()
    {
        isMoving = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            float eased = Mathf.SmoothStep(0f, 1f, t);

            leftDoor.position = Vector3.Lerp(leftClosedPos, leftOpenPos, eased);
            rightDoor.position = Vector3.Lerp(rightClosedPos, rightOpenPos, eased);

            yield return null;
        }

        leftDoor.position = leftOpenPos;
        rightDoor.position = rightOpenPos;
        doorsOpen = true;
        isMoving = false;

        Debug.Log("Main Puzzle Door Opened!");
    }

    public void CloseDoors()
    {
        if (!doorsOpen || isMoving) return;

        StartCoroutine(CloseDoorsCoroutine());
    }

    private System.Collections.IEnumerator CloseDoorsCoroutine()
    {
        isMoving = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            float eased = Mathf.SmoothStep(0f, 1f, t);

            leftDoor.position = Vector3.Lerp(leftOpenPos, leftClosedPos, eased);
            rightDoor.position = Vector3.Lerp(rightOpenPos, rightClosedPos, eased);

            yield return null;
        }

        leftDoor.position = leftClosedPos;
        rightDoor.position = rightClosedPos;
        doorsOpen = false;
        isMoving = false;
    }

    // Optional: Toggle
    public void ToggleDoor()
    {
        if (doorsOpen)
            CloseDoors();
        else
            OpenDoors();
    }
}
