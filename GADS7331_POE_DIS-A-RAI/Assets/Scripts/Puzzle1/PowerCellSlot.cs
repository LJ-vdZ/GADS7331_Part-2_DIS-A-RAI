using UnityEngine;

public class PowerCellSlot : MonoBehaviour
{
    [Header("Slot Settings")]
    public Transform insertPoint;           // Empty child where cell should snap
    public PowerCell currentCell = null;

    private bool isOccupied = false;

    public bool InsertCell(PowerCell cell)
    {
        if (isOccupied || cell == null) return false;

        currentCell = cell;
        isOccupied = true;

        cell.transform.SetParent(null);
        cell.transform.position = insertPoint.position;
        cell.transform.rotation = insertPoint.rotation;

        cell.OnPlaced();

        Debug.Log($"Cell ({cell.cellType}) inserted into slot");
        return true;
    }

    public void RemoveCell()
    {
        if (currentCell != null)
        {
            currentCell.OnRemoved();
            currentCell = null;
        }
        isOccupied = false;
    }

    public bool IsCorrectlyFilled()
    {
        return currentCell != null && currentCell.cellType == PowerCellType.Correct;
    }

    public bool IsOccupied() => isOccupied;
}
