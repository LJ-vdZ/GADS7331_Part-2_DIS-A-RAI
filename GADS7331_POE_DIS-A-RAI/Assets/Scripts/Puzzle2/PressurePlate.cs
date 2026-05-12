using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Visuals")]
    public Renderer plateRenderer;
    public int materialIndex = 0;

    [Header("Materials")]
    public Material inactiveMaterial;
    public Material occupiedMaterial;

    [Header("Debug")]
    public bool showDebug = true;

    private bool hasBeenActivated = false;      // Once true, stays true until manager deactivates
    private bool isCurrentlyOccupied = false;

    private void Start()
    {
        if (plateRenderer == null)
            Debug.LogWarning("PlateRenderer not assigned on " + gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Crate"))
        {
            isCurrentlyOccupied = true;

            bool wasAlreadyActivated = hasBeenActivated;

            if (!hasBeenActivated)
            {
                hasBeenActivated = true;
                if (showDebug) Debug.Log($"Plate {name} has been permanently activated!");
            }

            UpdateVisual();

            // Tell manager player reactivated a plate
            if (wasAlreadyActivated && other.CompareTag("Player"))
            {
                PressurePuzzleManager manager = FindObjectOfType<PressurePuzzleManager>();
                if (manager != null) manager.OnPlateReactivated(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Crate"))
        {
            isCurrentlyOccupied = false;
            UpdateVisual();
        }
    }

    public void Activate()
    {
        hasBeenActivated = true;
        UpdateVisual();
    }

    public void Deactivate()
    {
        hasBeenActivated = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (plateRenderer == null) return;

        Material[] mats = plateRenderer.materials;

        // FIXED: Show occupiedMaterial (green) if the plate has been activated, even if not currently occupied
        if (hasBeenActivated)
            mats[materialIndex] = occupiedMaterial;     // Green when activated
        else
            mats[materialIndex] = inactiveMaterial;     // Orange when never activated or deactivated

        plateRenderer.materials = mats;
    }

    public bool IsActive() => hasBeenActivated;
    public bool IsOccupied() => isCurrentlyOccupied;
}

