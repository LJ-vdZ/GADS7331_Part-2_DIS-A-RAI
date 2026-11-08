using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    private System.Action currentOnContinue;
    private Coroutine typeRoutine;
    private string fullDialogue = "";
    private bool isTyping;

    public bool IsDialogueOpen => dialoguePanel != null && dialoguePanel.activeSelf;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (dialoguePanel == null || !dialoguePanel.activeSelf)
            return;

        bool confirm = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
        if (!confirm)
            return;

        if (isTyping)
            SkipTyping();
        else
            CloseDialogue();
    }

    public void ShowDialogue(DialogueData data, System.Action onContinue = null)
    {
        if (data == null) return;

        dialoguePanel.SetActive(true);

        if (iconImage != null && data.icon != null)
            iconImage.sprite = data.icon;

        if (titleText != null)
            titleText.text = data.alertTitle;

        if (dialogueText != null)
        {
            fullDialogue = data.dialogueText ?? "";
            if (typeRoutine != null)
                StopCoroutine(typeRoutine);
            typeRoutine = StartCoroutine(TypeDialogue(fullDialogue));
        }

        currentOnContinue = onContinue;

        continueButton.onClick.RemoveAllListeners();
        //continueButton.onClick.AddListener(() => {dialoguePanel.SetActive(false); onContinue?.Invoke();});
        continueButton.onClick.AddListener(() =>
        {
            GameAudio.PlayUiClick();
            CloseDialogue();
        });
    }

    private void CloseDialogue()
    {
        if (typeRoutine != null)
        {
            StopCoroutine(typeRoutine);
            typeRoutine = null;
        }

        isTyping = false;
        GameAudio.EndDialogueTyping();
        if (dialogueText != null)
            dialogueText.maxVisibleCharacters = 99999;

        dialoguePanel.SetActive(false);
        currentOnContinue?.Invoke();
        currentOnContinue = null;
    }

    private IEnumerator TypeDialogue(string text)
    {
        isTyping = true;
        dialogueText.text = text;
        dialogueText.maxVisibleCharacters = 0;
        yield return null;
        Canvas.ForceUpdateCanvases();
        dialogueText.ForceMeshUpdate();

        int count = dialogueText.textInfo.characterCount;
        if (count > 0)
            GameAudio.BeginDialogueTyping();

        for (int i = 1; i <= count; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.028f);
        }

        CompleteTyping();
    }

    private void SkipTyping()
    {
        if (typeRoutine != null)
        {
            StopCoroutine(typeRoutine);
            typeRoutine = null;
        }

        if (dialogueText != null)
        {
            dialogueText.text = fullDialogue;
            dialogueText.maxVisibleCharacters = 99999;
        }

        CompleteTyping();
    }

    private void CompleteTyping()
    {
        isTyping = false;
        typeRoutine = null;
        if (dialogueText != null)
            dialogueText.maxVisibleCharacters = 99999;
        GameAudio.EndDialogueTyping();
    }
}
