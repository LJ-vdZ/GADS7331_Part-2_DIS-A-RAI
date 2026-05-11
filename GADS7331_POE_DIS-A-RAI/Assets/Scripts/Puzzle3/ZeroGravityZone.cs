using UnityEngine;

public class ZeroGravityZone : MonoBehaviour
{
    [Header("Settings")]
    public string codeToRestoreGravity = "1234";
    public float playerZeroGSpeed = 12f;
    public float playerPushForce = 20f;

    private bool gravityDisabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.EnterZeroGravity();

            // Activate crates
            foreach (var crate in FindObjectsOfType<CrateFloat>())
                crate.ActivateZeroG();
        }
    }

    public void RestoreGravity()
    {
        gravityDisabled = false;
        Debug.Log("Gravity Restored!");

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.ExitZeroGravity();
    }

    public string GetCode() => codeToRestoreGravity;
}
