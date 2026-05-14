using UnityEngine;
using TMPro;

public class CodeInput : MonoBehaviour
{
    [Header("UI References")]
    public GameObject codePanel;
    public TMP_InputField codeInputField;
    public TextMeshProUGUI instructionText;

    private ZeroGravityZone currentZone;
    private bool isActive = false;

    private void Awake()
    {
        Debug.Log("CodeInput Script AWAKE");
    }

    private void Update()
    {
        if (!isActive)
            return;

        Debug.Log("CodeInput Update is running...");   // This should spam every frame when panel is open

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("ENTER key detected!");
            SubmitCode();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed - closing");
            CancelInput();
        }
    }

    public void StartCodeInput(ZeroGravityZone zone)
    {
        Debug.Log("StartCodeInput called with zone: " + (zone != null));
        currentZone = zone;
        isActive = true;

        if (codePanel != null)
            codePanel.SetActive(true);

        if (codeInputField != null)
        {
            codeInputField.text = "";
            codeInputField.ActivateInputField();
            Debug.Log("InputField activated");
        }
        else
        {
            Debug.LogError("CodeInputField reference is MISSING on CodeInput script!");
        }

        if (instructionText != null)
            instructionText.text = "Enter Code to Restore Gravity";
    }

    private void SubmitCode()
    {
        if (currentZone == null)
        {
            Debug.LogError("currentZone is NULL!");
            return;
        }

        string enteredCode = codeInputField.text;
        string correctCode = currentZone.GetCode();

        Debug.Log($"Raw Entered: '{enteredCode}' (Length: {enteredCode.Length})");
        Debug.Log($"Raw Correct: '{correctCode}' (Length: {correctCode.Length})");

        // Clean both sides
        string cleanEntered = enteredCode.Trim().ToUpper();
        string cleanCorrect = correctCode.Trim().ToUpper();

        Debug.Log($"Clean Entered: '{cleanEntered}'");
        Debug.Log($"Clean Correct: '{cleanCorrect}'");

        if (cleanEntered == cleanCorrect && cleanEntered.Length > 0)
        {
            Debug.Log("CODE MATCH SUCCESS!");
            if (instructionText != null)
                instructionText.text = "<color=green>Correct! Restoring Gravity...</color>";

            currentZone.RestoreGravity();
            Invoke(nameof(ClosePanel), 1f);
        }
        else
        {
            Debug.Log("CODE DOES NOT MATCH");
            if (instructionText != null)
                instructionText.text = "<color=red>Wrong Code! Try Again.</color>";

            codeInputField.text = "";
            codeInputField.ActivateInputField();
        }
    }

    private void ClosePanel()
    {
        isActive = false;
        if (codePanel != null) codePanel.SetActive(false);
        currentZone = null;
        Debug.Log("Code panel closed");
    }

    private void CancelInput()
    {
        ClosePanel();
    }

    private void OnEnable()
    {
        Debug.Log("CodeInputPanel OnEnable - Ready");
        if (codeInputField != null)
        {
            codeInputField.text = "";
            codeInputField.ActivateInputField();
        }
    }
}