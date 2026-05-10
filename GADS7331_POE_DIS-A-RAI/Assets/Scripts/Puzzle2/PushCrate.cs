using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushCrate : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushForce = 12f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 25f;
    }

    public void Push(Vector3 direction)
    {
        rb.AddForce(direction * pushForce, ForceMode.Impulse);
    }
}