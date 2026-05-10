using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Visuals")]
    public Renderer plateRenderer;
    public Color inactiveColor = Color.red;
    public Color activeColor = Color.green;
    public Color occupiedColor = new Color(0f, 0.8f, 0.3f);

    [Header("Debug")]
    public bool showDebug = true;

    private Material material;
    private bool isOccupied = false;
    private bool isActive = false;

    private void Awake()
    {
        if (plateRenderer != null)
            material = plateRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Crate"))
        {
            isOccupied = true;
            Activate();
            if (showDebug) Debug.Log($"Plate {name} occupied by {other.tag}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Crate"))
        {
            isOccupied = false;
            if (showDebug) Debug.Log($"Plate {name} no longer occupied by {other.tag}");
        }
    }

    public void Activate()
    {
        isActive = true;
        UpdateVisual();
    }

    public void Deactivate()
    {
        isActive = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (material == null) return;

        if (isOccupied && isActive)
            material.color = occupiedColor;
        else
            material.color = isActive ? activeColor : inactiveColor;
    }

    public bool IsActive() => isActive;
    public bool IsOccupied() => isOccupied;
}
