using UnityEngine;

public class HangarDoorController : MonoBehaviour
{
    [Header("Door References")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Animation Settings")]
    public float openDistance = 2.5f;
    public float openSpeed = 2f;
    public float delayBeforeOpen = 1f;

    [Header("Movement Settings")]
    public bool moveAlongZ = true;

    [Header("Trigger")]
    public BoxCollider arrivalTrigger;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private bool doorsOpen = false;
    private bool isOpening = false;

    void Start()
    {
        if (leftDoor == null || rightDoor == null)
        {
            Debug.LogError("Left and Right door transforms must be assigned!");
            return;
        }

        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;

        CalculateOpenPositions();

        if (arrivalTrigger == null)
        {
            Debug.LogWarning("Arrival trigger not assigned.");
        }
    }

    private void CalculateOpenPositions()
    {
        if (moveAlongZ)
        {
            leftOpenPos = leftClosedPos + new Vector3(0, 0, openDistance);
            rightOpenPos = rightClosedPos + new Vector3(0, 0, -openDistance);
        }
        else
        {
            // Fallback X-axis
            leftOpenPos = leftClosedPos + new Vector3(openDistance, 0, 0);
            rightOpenPos = rightClosedPos + new Vector3(-openDistance, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ElevatorPlatform") && !doorsOpen && !isOpening)
        {
            Invoke(nameof(OpenDoors), delayBeforeOpen);
        }
    }

    public void PlatformArrived()
    {
        if (!doorsOpen && !isOpening)
            Invoke(nameof(OpenDoors), delayBeforeOpen);
    }

    private void OpenDoors()
    {
        if (isOpening) return;
        StartCoroutine(OpenDoorsCoroutine());
    }

    private System.Collections.IEnumerator OpenDoorsCoroutine()
    {
        isOpening = true;

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
        isOpening = false;
    }

    public void CloseDoors()
    {
        if (doorsOpen)
        {
            StartCoroutine(CloseDoorsCoroutine());
        }
    }

    private System.Collections.IEnumerator CloseDoorsCoroutine()
    {
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
    }
}
