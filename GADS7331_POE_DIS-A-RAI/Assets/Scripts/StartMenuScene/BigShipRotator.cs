using UnityEngine;

public class BigShipRotator : MonoBehaviour
{
    public float rotationSpeed = 3f;
    void Update() => transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
}
