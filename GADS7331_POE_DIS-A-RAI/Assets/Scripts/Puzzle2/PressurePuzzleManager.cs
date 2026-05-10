using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class PressurePuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public PressurePlate[] pressurePlates = new PressurePlate[4];
    public Door mainDoor;

    [Header("Deactivation Settings")]
    public float minDeactivateInterval = 10f;
    public float maxDeactivateInterval = 18f;

    private bool allPlatesActivatedOnce = false;
    private bool puzzleSolved = false;
    private Coroutine deactivationRoutine;
    private bool deactivationInProgress = false;   // Strong guard

    private void Start()
    {
        if (mainDoor == null)
            Debug.LogError("Main Door not assigned!");

        foreach (var plate in pressurePlates)
            if (plate != null) plate.Deactivate();
    }

    private void Update()
    {
        if (puzzleSolved) return;

        if (!allPlatesActivatedOnce && AllPlatesActivatedOnce())
        {
            allPlatesActivatedOnce = true;
            Debug.Log("All plates activated once! Starting controlled deactivation.");
            StartDeactivationRoutine();
        }

        if (AllPlatesCurrentlyOccupied())
        {
            puzzleSolved = true;
            SolvePuzzle();
        }
    }

    private bool AllPlatesActivatedOnce()
    {
        foreach (var plate in pressurePlates)
            if (!plate.IsActive()) return false;
        return true;
    }

    private bool AllPlatesCurrentlyOccupied()
    {
        foreach (var plate in pressurePlates)
            if (!plate.IsOccupied()) return false;
        return true;
    }

    private void SolvePuzzle()
    {
        Debug.Log("PRESSURE PLATE PUZZLE SOLVED!");
        StopDeactivationRoutine();
        if (mainDoor != null) mainDoor.OpenDoors();
    }

    private void StartDeactivationRoutine()
    {
        if (deactivationRoutine != null) StopCoroutine(deactivationRoutine);
        deactivationRoutine = StartCoroutine(DeactivationCoroutine());
    }

    private void StopDeactivationRoutine()
    {
        if (deactivationRoutine != null)
            StopCoroutine(deactivationRoutine);
    }

    private IEnumerator DeactivationCoroutine()
    {
        while (!puzzleSolved)
        {
            float delay = Random.Range(minDeactivateInterval, maxDeactivateInterval);
            yield return new WaitForSeconds(delay);

            if (!puzzleSolved)
                TryDeactivateOnePlate();
        }
    }

    private void TryDeactivateOnePlate()
    {
        if (deactivationInProgress) return;
        deactivationInProgress = true;

        List<PressurePlate> candidates = new List<PressurePlate>();

        foreach (var plate in pressurePlates)
        {
            if (plate.IsActive() && !plate.IsOccupied())
                candidates.Add(plate);
        }

        if (candidates.Count > 0)
        {
            PressurePlate chosen = candidates[Random.Range(0, candidates.Count)];
            chosen.Deactivate();
            Debug.Log($"[Single Deactivation] Deactivated: {chosen.name}");
        }

        deactivationInProgress = false;
    }

    // Called from PressurePlate when player steps on a previously deactivated plate
    public void OnPlateReactivated(PressurePlate plate)
    {
        if (allPlatesActivatedOnce && !puzzleSolved)
        {
            Debug.Log($"Player reactivated {plate.name} > Deactivating one other plate");
            TryDeactivateOnePlate();
        }
    }
}
