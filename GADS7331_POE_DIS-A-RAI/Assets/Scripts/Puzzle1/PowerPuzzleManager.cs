using UnityEngine;

public class PowerPuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public PowerCellSlot[] slots;
    public Door mainDoor;                    //Assign your new Door script here

    private bool puzzleSolved = false;

    private void Update()
    {
        if (!puzzleSolved && CheckAllSlotsCorrect())
        {
            puzzleSolved = true;
            SolvePuzzle();
        }
    }

    private bool CheckAllSlotsCorrect()
    {
        foreach (PowerCellSlot slot in slots)
        {
            if (!slot.IsCorrectlyFilled())
                return false;
        }
        return true;
    }

    private void SolvePuzzle()
    {
        Debug.Log("POWER PUZZLE SOLVED!");
        if (mainDoor != null)
            mainDoor.OpenDoors();
        else
            Debug.LogError("Main Door is not assigned in PowerPuzzleManager!");
    }

    public void ForceSolve()
    {
        puzzleSolved = true;
        SolvePuzzle();
    }
}
