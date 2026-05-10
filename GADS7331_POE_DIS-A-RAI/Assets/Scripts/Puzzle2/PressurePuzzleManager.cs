using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PressurePuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public PressurePlate[] pressurePlates = new PressurePlate[4];
    public Door puzzle2Door;                    // Assign your Door script here

    [Header("Deactivation Settings")]
    public float minDeactivateInterval = 8f;
    public float maxDeactivateInterval = 14f;

    private bool puzzleSolved = false;
    private System.Collections.IEnumerator deactivationRoutine;

    private void Start()
    {
        if (puzzle2Door == null)
            Debug.LogError("Main Door is not assigned in PressurePuzzleManager!");

        // Initialize plates
        foreach (var plate in pressurePlates)
        {
            if (plate != null) plate.Deactivate();
        }

        StartDeactivationRoutine();
    }

    private void Update()
    {
        if (puzzleSolved) return;

        if (AllPlatesActive())
        {
            puzzleSolved = true;
            SolvePuzzle();
        }
    }

    private bool AllPlatesActive()
    {
        foreach (var plate in pressurePlates)
        {
            if (plate == null || !plate.IsActive())
                return false;
        }
        return true;
    }

    private void SolvePuzzle()
    {
        Debug.Log("PRESSURE PLATE PUZZLE SOLVED!");
        StopDeactivationRoutine();

        if (puzzle2Door != null)
            puzzle2Door.OpenDoors();
    }

    // ==================== RANDOM DEACTIVATION ====================
    private void StartDeactivationRoutine()
    {
        if (deactivationRoutine != null)
            StopCoroutine(deactivationRoutine);

        deactivationRoutine = DeactivationCoroutine();
        StartCoroutine(deactivationRoutine);
    }

    private void StopDeactivationRoutine()
    {
        if (deactivationRoutine != null)
            StopCoroutine(deactivationRoutine);
    }

    private System.Collections.IEnumerator DeactivationCoroutine()
    {
        while (!puzzleSolved)
        {
            float waitTime = Random.Range(minDeactivateInterval, maxDeactivateInterval);
            yield return new WaitForSeconds(waitTime);

            // Get currently active plates that are NOT occupied
            var activePlates = new System.Collections.Generic.List<PressurePlate>();

            foreach (var plate in pressurePlates)
            {
                if (plate.IsActive() && !plate.IsOccupied())
                    activePlates.Add(plate);
            }

            if (activePlates.Count > 0)
            {
                PressurePlate plateToDeactivate = activePlates[Random.Range(0, activePlates.Count)];
                plateToDeactivate.Deactivate();
                Debug.Log($"Randomly deactivated plate: {plateToDeactivate.name}");
            }
        }
    }

    // For testing
    public void ForceSolve()
    {
        puzzleSolved = true;
        if (puzzle2Door != null) puzzle2Door.OpenDoors();
    }
}
