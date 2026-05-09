using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Visuals")]
    public Renderer plateRenderer;
    public Color deactivatedColor = Color.red;
    public Color activatedColor = Color.green;
    public Color occupiedColor = new Color(0, 1f, 0.5f); // Bright green when occupied

    [Header("Settings")]
    public bool isActive = false;

    private Material material;
    private Collider plateCollider;

    private void Awake()
    {
        plateCollider = GetComponent<Collider>();
        if (plateRenderer != null)
            material = plateRenderer.material;
    }

    public void Activate()
    {
        isActive = true;
        UpdateVisuals();
    }

    public void Deactivate()
    {
        isActive = false;
        UpdateVisuals();
    }

    public void SetOccupied(bool occupied)
    {
        if (occupied && isActive)
        {
            SetColor(occupiedColor);
        }
        else
        {
            SetColor(isActive ? activatedColor : deactivatedColor);
        }
    }

    private void UpdateVisuals()
    {
        SetColor(isActive ? activatedColor : deactivatedColor);
    }

    private void SetColor(Color color)
    {
        if (material != null)
            material.color = color;
    }

    public bool IsActive() => isActive;
}
