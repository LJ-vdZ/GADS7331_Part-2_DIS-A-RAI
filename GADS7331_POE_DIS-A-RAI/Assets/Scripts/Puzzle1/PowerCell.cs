using UnityEngine;

public enum PowerCellType
{
    Dead,
    Correct,
    Wrong
}


public class PowerCell : MonoBehaviour
{
    [Header("Power Cell Settings")]
    public PowerCellType cellType = PowerCellType.Correct;

    [HideInInspector]
    public bool isPlaced = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnPickup()
    {
        isPlaced = false;
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public void OnPlaced()
    {
        isPlaced = true;
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public void OnRemoved()
    {
        isPlaced = false;
    }
}
