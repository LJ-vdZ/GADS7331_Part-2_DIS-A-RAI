using UnityEngine;

public class HubManager : MonoBehaviour
{
    [Header("Terminal Panels")]
    public GameObject mainTerminalPanel;
    public GameObject shipLevelsPanel;
    public GameObject criticalSystemsPanel;
    public GameObject codeInputPanel;

    [Header("Main TAB Menu")]
    public GameObject mainTabMenuPanel;

    [Header("References")]
    public PlayerController playerController;
    public Terminal terminal;

    private bool isMenuOpen = false;
    private bool tabUnlocked = false;

    private void Update()
    {
        if (!tabUnlocked) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMainTabMenu();
        }
    }

    private void ToggleMainTabMenu()
    {
        isMenuOpen = !isMenuOpen;
        if (isMenuOpen)
            OpenMainTabMenu();
        else
            CloseAllMenus();
    }

    public void OpenMainTabMenu()
    {
        CloseAllMenus();
        if (mainTabMenuPanel != null)
        {
            mainTabMenuPanel.SetActive(true);
            Debug.Log("Main TAB Menu Opened");
        }
        isMenuOpen = true;
        LockCamera(true);
        LockPlayerMovement(true);        // Added
    }

    public void OpenMainTerminalPanel()
    {
        CloseAllMenus();
        if (mainTerminalPanel != null) mainTerminalPanel.SetActive(true);
        LockCamera(true);
        LockPlayerMovement(true);        // Added
    }

    public void OnShipMapButtonPressed()
    {
        if (terminal != null)
            terminal.ShowMapCorruptedDialogue();
    }

    public void OnShipAIButtonPressed()
    {
        if (terminal != null)
            terminal.StartLinkDeviceProcess();
    }

    public void CloseAllMenus()
    {
        if (mainTerminalPanel != null) mainTerminalPanel.SetActive(false);
        if (shipLevelsPanel != null) shipLevelsPanel.SetActive(false);
        if (criticalSystemsPanel != null) criticalSystemsPanel.SetActive(false);
        if (codeInputPanel != null) codeInputPanel.SetActive(false);
        if (mainTabMenuPanel != null) mainTabMenuPanel.SetActive(false);

        isMenuOpen = false;
        LockCamera(false);
        LockPlayerMovement(false);       // Added
    }

    public void LockCamera(bool lockState)
    {
        if (playerController != null)
            playerController.SetCameraLocked(lockState);

        Cursor.visible = lockState;
        Cursor.lockState = lockState ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // ====================== NEW METHOD (Minimal Addition) ======================
    private void LockPlayerMovement(bool lockState)
    {
        if (playerController != null)
        {
            playerController.SetMovementLocked(lockState);
        }
    }

    public void UnlockEverything()
    {
        tabUnlocked = true;
        CloseAllMenus();
        Debug.Log("TAB menu is now fully unlocked!");
    }
}
