using UnityEngine;

public class Puzzle4Manager : MonoBehaviour
{
    [Header("Puzzle References")]
    public SecurityBot[] securityBots;
    public BlackBox blackBox;
    //public GameObject exitDoor;           // Or use your Door script

    private bool puzzleCompleted = false;

    public bool AllBotsDefeated()
    {
        foreach (var bot in securityBots)
        {
            if (bot != null && !bot.IsDeactivated())
                return false;
        }
        return true;
    }

    public void CompletePuzzle()
    {
        if (puzzleCompleted) return;

        puzzleCompleted = true;
        Debug.Log("FINAL PUZZLE COMPLETED!");

        //// Open exit
        //if (exitDoor != null)
        //{
        //    Door door = exitDoor.GetComponent<Door>();
        //    if (door != null) door.OpenDoors();
        //}
    }
}
