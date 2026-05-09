using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushCrate : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushForce = 8f;
    public bool isBeingPushed = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 20f; 
    }

    public void Push(Vector3 direction)
    {
        isBeingPushed = true;
        rb.AddForce(direction * pushForce, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < 0.1f)
            isBeingPushed = false;
    }
}