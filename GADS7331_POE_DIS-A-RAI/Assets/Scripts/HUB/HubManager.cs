using UnityEngine;

public class HubManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject shipLevelsPanel;
    public GameObject codeInputPanel;

    [Header("References")]
    public PlayerController playerController;

    private bool isMenuOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            CloseAllMenus();
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
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        isMenuOpen = true;
        LockCamera(true);
    }

    public void OpenShipLevelsPanel()
    {
        CloseAllMenus();
        if (shipLevelsPanel != null) shipLevelsPanel.SetActive(true);
        LockCamera(true);
    }

    public void OpenGravityCodePanel()
    {
        CloseAllMenus();
        if (codeInputPanel != null)
        {
            codeInputPanel.SetActive(true);

            CodeInput codeInput = codeInputPanel.GetComponent<CodeInput>();
            if (codeInput != null)
            {
                // We need to know which zone - for now we'll find it
                ZeroGravityZone zone = FindObjectOfType<ZeroGravityZone>();
                if (zone != null)
                    codeInput.StartCodeInput(zone);
                else
                    Debug.LogError("No ZeroGravityZone found in scene!");
            }
        }
        LockCamera(true);
    }

    public void CloseAllMenus()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (shipLevelsPanel != null) shipLevelsPanel.SetActive(false);
        if (codeInputPanel != null) codeInputPanel.SetActive(false);

        isMenuOpen = false;
        LockCamera(false);
    }

    private void LockCamera(bool lockState)
    {
        if (playerController != null)
            playerController.SetCameraLocked(lockState);

        Cursor.visible = lockState;
        Cursor.lockState = lockState ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void UnlockEverything()
    {
        CloseAllMenus();
        LockCamera(false);        // This unlocks the camera
    }
}
