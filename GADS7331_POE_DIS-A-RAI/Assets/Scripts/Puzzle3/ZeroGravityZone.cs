using UnityEngine;

public class ZeroGravityZone : MonoBehaviour
{
    [Header("Settings")]
    public string codeToRestoreGravity = "795ROOT";

    [Header("One-Time Trigger")]
    public bool destroyAfterUse = true;
    private bool hasBeenActivated = false;

    [Header("References")]
    public HubManager menuManager;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenActivated) return;

        if (other.CompareTag("Player"))
        {
            hasBeenActivated = true;

            Debug.Log("Zero-G Zone Triggered");

            // Activate Zero-G on Player
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.EnterZeroGravity();

            // Activate all crates
            foreach (var crate in FindObjectsOfType<CrateFloat>())
                crate.ActivateZeroG();

            // Open menu
            if (menuManager != null)
                menuManager.OpenMainMenu();
            else
                Debug.LogWarning("MenuManager not assigned on ZeroGravityZone!");

            // Disable trigger after use
            if (destroyAfterUse)
            {
                Collider col = GetComponent<Collider>();
                if (col != null)
                    col.enabled = false;
            }
        }
    }

    public string GetCode()
    {
        return codeToRestoreGravity;
    }

    public void RestoreGravity()
    {
        Debug.Log("RestoreGravity() called");

        // Restore Player
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.ExitZeroGravity();

        // Restore Crates
        foreach (var crate in FindObjectsOfType<CrateFloat>())
            crate.DeactivateZeroG();

        // Unlock camera/menu
        if (menuManager != null)
            menuManager.UnlockEverything();
    }
}