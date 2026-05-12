using UnityEngine;

public class BlackBox : MonoBehaviour
{
    public Puzzle4Manager puzzleManager;

    private bool canBePickedUp = false;

    private void Update()
    {
        if (canBePickedUp && Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    public void EnablePickup()
    {
        canBePickedUp = true;
        Debug.Log("Black Box can now be retrieved!");
        // Optional: Add glow / particles
    }

    private void TryPickup()
    {
        if (puzzleManager != null && puzzleManager.AllBotsDefeated())
        {
            Debug.Log("Black Box Retrieved! Puzzle Complete!");
            puzzleManager.CompletePuzzle();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Bots are still active!");
        }
    }
}
