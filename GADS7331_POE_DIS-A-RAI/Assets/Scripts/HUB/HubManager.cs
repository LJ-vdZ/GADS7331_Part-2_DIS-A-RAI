using UnityEngine;

public class HubManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainTerminalPanel;        // Main Terminal UI
    public GameObject shipLevelsPanel;          // Ship Map Panel (optional)
    public GameObject criticalSystemsPanel;
    public GameObject codeInputPanel;

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
            ToggleMainMenu();
        }
    }

    private void ToggleMainMenu()
    {
        isMenuOpen = !isMenuOpen;
        if (isMenuOpen)
            OpenMainMenu();
        else
            CloseAllMenus();
    }

    public void OpenMainMenu()
    {
        CloseAllMenus();
        if (mainTerminalPanel != null) mainTerminalPanel.SetActive(true);
        isMenuOpen = true;
        LockCamera(true);
    }

    // ====================== BUTTON CALLS ======================

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

        isMenuOpen = false;
        LockCamera(false);
    }

    public void LockCamera(bool lockState)
    {
        if (playerController != null)
            playerController.SetCameraLocked(lockState);

        Cursor.visible = lockState;
        Cursor.lockState = lockState ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void UnlockEverything()
    {
        tabUnlocked = true;
        CloseAllMenus();
        LockCamera(false);
        Debug.Log("Tab menu is now unlocked!");
    }


}
