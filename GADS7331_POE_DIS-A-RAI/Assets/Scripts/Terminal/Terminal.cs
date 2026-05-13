using UnityEngine;

public class Terminal : MonoBehaviour
{
    [Header("References")]
    public HubManager hubManager;

    [Header("Dialogues")]
    public DialogueData introDialogue;
    public DialogueData mapCorruptedDialogue;
    public DialogueData linkSuccessDialogue;

    [Header("Objects to Destroy")]
    public GameObject textObjectToDestroy;

    private bool hasLinkedDevice = false;
    private bool playerInTrigger = false;

    private void Start()
    {
        if (introDialogue != null && DialogueSystem.Instance != null)
        {
            // Lock camera for intro dialogue
            if (hubManager != null)
                hubManager.LockCamera(true);

            DialogueSystem.Instance.ShowDialogue(introDialogue, OnIntroDialogueClosed);
        }
    }

    // Called after player clicks Continue on intro dialogue
    private void OnIntroDialogueClosed()
    {
        if (hubManager != null)
        {
            hubManager.LockCamera(false);   // Unlock camera
            Debug.Log("Intro dialogue closed - Camera unlocked");
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
        Debug.Log("Terminal -> Opening Terminal Panel");

        if (hasLinkedDevice)
        {
            Debug.Log("Device already linked.");
            return;
        }

        if (hubManager != null)
        {
            hubManager.OpenMainTerminalPanel();     // This should open the terminal UI, not the TAB menu
        }
        else
        {
            Debug.LogError("HubManager is not assigned on Terminal!");
        }
    }

    public void ShowMapCorruptedDialogue()
    {
        if (mapCorruptedDialogue != null && DialogueSystem.Instance != null)
            DialogueSystem.Instance.ShowDialogue(mapCorruptedDialogue);
    }

    public void StartLinkDeviceProcess()
    {
        if (hasLinkedDevice) return;

        hasLinkedDevice = true;

        if (linkSuccessDialogue != null && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ShowDialogue(linkSuccessDialogue, () =>
            {
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
            Destroy(textObjectToDestroy);
    }
}
