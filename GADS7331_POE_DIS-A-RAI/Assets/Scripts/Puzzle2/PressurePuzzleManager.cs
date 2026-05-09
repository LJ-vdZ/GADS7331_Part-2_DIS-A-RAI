using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PressurePuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public PressurePlate[] pressurePlates = new PressurePlate[4];
    public GameObject doorToOpen;
    public Door doorScript;                    // Use your new Door script

    [Header("Deactivation Settings")]
    public float minDeactivateTime = 8f;
    public float maxDeactivateTime = 15f;

    private HashSet<PressurePlate> activePlates = new HashSet<PressurePlate>();
    private bool puzzleSolved = false;
    private Coroutine deactivationCoroutine;

    private void Start()
    {
        // Initialize all plates as deactivated
        foreach (var plate in pressurePlates)
        {
            if (plate != null) plate.Deactivate();
        }

        StartDeactivationTimer();
    }

    public void OnPlateActivated(PressurePlate plate)
    {
        activePlates.Add(plate);
        CheckPuzzleComplete();
    }

    public void OnPlateDeactivated(PressurePlate plate)
    {
        activePlates.Remove(plate);
    }

    private void CheckPuzzleComplete()
    {
        if (puzzleSolved) return;

        if (activePlates.Count >= pressurePlates.Length)
        {
            puzzleSolved = true;
            SolvePuzzle();
        }
    }

    private void SolvePuzzle()
    {
        Debug.Log("PRESSURE PLATE PUZZLE SOLVED!");
        StopAllCoroutines();

        if (doorScript != null)
            doorScript.OpenDoors();
        else if (doorToOpen != null)
            Debug.LogWarning("Door script not assigned, but door object is present.");
    }

    // Random deactivation system
    private void StartDeactivationTimer()
    {
        if (deactivationCoroutine != null)
            StopCoroutine(deactivationCoroutine);

        float delay = Random.Range(minDeactivateTime, maxDeactivateTime);
        deactivationCoroutine = StartCoroutine(DeactivateRandomPlate(delay));
    }

    private System.Collections.IEnumerator DeactivateRandomPlate(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Find plates that are currently active and not occupied by player or crate
        var candidates = pressurePlates.Where(p => p.IsActive()).ToList();

        if (candidates.Count > 0)
        {
            PressurePlate plateToDeactivate = candidates[Random.Range(0, candidates.Count)];
            plateToDeactivate.Deactivate();
            OnPlateDeactivated(plateToDeactivate);

            Debug.Log($"Plate deactivated: {plateToDeactivate.name}");
        }

        StartDeactivationTimer(); // Restart timer
    }

    // Public method for plates to call
    public void RegisterPlateActivation(PressurePlate plate)
    {
        OnPlateActivated(plate);
    }
}
