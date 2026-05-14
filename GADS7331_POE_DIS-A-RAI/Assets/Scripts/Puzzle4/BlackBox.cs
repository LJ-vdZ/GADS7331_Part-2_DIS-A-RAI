using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackBox : MonoBehaviour
{
    [Header("Settings")]
    public Puzzle4Manager puzzleManager;

    private Rigidbody rb;
    private bool canBePickedUp = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canBePickedUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canBePickedUp = false;
        }
    }

    public void TryPickup(PlayerController player)
    {
        if (!canBePickedUp) return;

        if (puzzleManager != null && puzzleManager.AllBotsDefeated())
        {
            Debug.Log("BLACK BOX COLLECTED - PUZZLE COMPLETE!");

            // Attach to player with specific rotation
            player.PickupBlackBox(this);

            // Hide trigger collider while being carried
            GetComponent<Collider>().enabled = false;

            SceneManager.LoadScene("WinState");


        }
        else
        {
            Debug.Log("Cannot pick up yet - Defeat all bots first!");
        }
    }
}
