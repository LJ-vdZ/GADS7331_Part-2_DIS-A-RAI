using UnityEngine;

public class DoorOpenTrigger : MonoBehaviour
{
    [Header("Door Settings")]
    public string doorTag = "Door";
    public float openHeight = 4f;
    public float openSpeed = 3f;

    private Transform door;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        hasTriggered = true;

        // Find the door by tag when triggered
        door = GameObject.FindGameObjectWithTag(doorTag)?.transform;

        if (door == null)
        {
            Debug.LogError("Door with tag 'Door' not found!");
            return;
        }

        closedPosition = door.position;
        openPosition = closedPosition + Vector3.up * openHeight;
        isOpening = true;

        Debug.Log("Door opening started!");

        // Disable trigger
        GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if (isOpening && door != null)
        {
            door.position = Vector3.MoveTowards(door.position, openPosition, openSpeed * Time.deltaTime);

            if (Vector3.Distance(door.position, openPosition) < 0.1f)
            {
                isOpening = false;
                Destroy(gameObject, 1f);   // Destroy trigger after door opens
            }
        }
    }
}
