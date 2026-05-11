using UnityEngine;

public class ZeroGravityZone : MonoBehaviour
{
    [Header("Settings")]
    public string codeToRestoreGravity = "795ROOT";

    [Header("One-Time Trigger")]
    public bool destroyAfterUse = true;
    private bool hasBeenActivated = false;

    [Header("Spawn New Trigger")]
    public GameObject nextTriggerPrefab;        // Assign the new trigger prefab here
    public Transform nextTriggerSpawnPoint;     // Where to spawn the new trigger

    [Header("References")]
    public HubManager menuManager;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenActivated) return;

        if (other.CompareTag("Player"))
        {
            hasBeenActivated = true;

            Debug.Log("Zero-G Zone Triggered - Spawning next trigger");

            // Activate Zero-G
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) player.EnterZeroGravity();

            foreach (var crate in FindObjectsOfType<CrateFloat>())
                crate.ActivateZeroG();

            // Open menu
            if (menuManager != null)
                menuManager.OpenMainMenu();

            // Spawn the next trigger
            SpawnNextTrigger();

            // Disable this trigger
            if (destroyAfterUse)
            {
                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = false;
            }
        }
    }

    private void SpawnNextTrigger()
    {
        if (nextTriggerPrefab != null && nextTriggerSpawnPoint != null)
        {
            Instantiate(nextTriggerPrefab, nextTriggerSpawnPoint.position, nextTriggerSpawnPoint.rotation);
            Debug.Log("Next trigger spawned successfully!");
        }
        else
        {
            Debug.LogWarning("Next Trigger Prefab or Spawn Point not assigned!");
        }
    }

    public string GetCode() => codeToRestoreGravity;

    public void RestoreGravity()
    {
        // Player & Crates
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.ExitZeroGravity();

        foreach (var crate in FindObjectsOfType<CrateFloat>())
            crate.DeactivateZeroG();

        if (menuManager != null)
            menuManager.UnlockEverything();
    }
}