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

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            CloseDialogue();
        }
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
            dialogueText.text = data.dialogueText;

        currentOnContinue = onContinue;

        continueButton.onClick.RemoveAllListeners();
        //continueButton.onClick.AddListener(() => {dialoguePanel.SetActive(false); onContinue?.Invoke();});
        continueButton.onClick.AddListener(() => { CloseDialogue(); });
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        currentOnContinue?.Invoke();
        currentOnContinue = null;
    }
}
