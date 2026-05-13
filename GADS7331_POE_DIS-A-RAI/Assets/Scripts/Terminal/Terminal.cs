using UnityEngine;

public class Terminal : MonoBehaviour
{
    [Header("References")]
    public HubManager hubManager;

    [Header("Dialogues")]
    public DialogueData introDialogue;
    public DialogueData mapCorruptedDialogue;
    public DialogueData linkSuccessDialogue;

    private bool hasLinkedDevice = false;
    private bool playerInTrigger = false;

    [Header("Objects to Destroy")]
    public GameObject textObjectToDestroy;

    private void Start()
    {
        // Show intro dialogue at game start + lock camera
        if (introDialogue != null && DialogueSystem.Instance != null)
        {
            // Lock camera before showing dialogue
            if (hubManager != null)
                hubManager.LockCamera(true);

            DialogueSystem.Instance.ShowDialogue(introDialogue, () =>
            {
                //Unlock camera after player closes intro dialogue
                hubManager.LockCamera(false);
            });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = false;
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && !hasLinkedDevice)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (hubManager != null)
            hubManager.OpenMainMenu();        // Opens main terminal panel
    }

    public void ShowMapCorruptedDialogue()
    {
        if (mapCorruptedDialogue != null && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue(mapCorruptedDialogue);
        }
    }

    public void StartLinkDeviceProcess()
    {
        if (hasLinkedDevice) return;

        hasLinkedDevice = true;

        if (linkSuccessDialogue != null && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue(linkSuccessDialogue, () =>
            {
                // After player clicks Continue on success dialogue
                if (hubManager != null)
                    hubManager.UnlockEverything();

                DestroyTextObject();
                Destroy(gameObject);
            });
        }
    }

    private void DestroyTextObject()
    {
        if (textObjectToDestroy != null)
        {
            Destroy(textObjectToDestroy);
            Debug.Log("3D Text object destroyed along with terminal");
        }
    }
}
