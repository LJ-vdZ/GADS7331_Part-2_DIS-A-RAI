using UnityEngine;

public class CrateFloat : MonoBehaviour
{
    [Header("Zero-G Floating Settings")]
    public float initialUpwardForce = 6f;
    public float driftForce = 4f;
    public float rotationSpeed = 40f;

    private Rigidbody rb;
    private bool zeroGActive = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0.6f;
        rb.angularDamping = 1.2f;
    }

    public void ActivateZeroG()
    {
        //zeroGActive = true;
        //// Gentle initial upward push
        //rb.AddForce(Vector3.up * initialUpwardForce, ForceMode.Impulse);

        zeroGActive = true;
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.mass = 1f;                    // explicit
        rb.linearDamping = 0.8f;
        rb.angularDamping = 1.5f;

        rb.AddForce(Vector3.up * initialUpwardForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (!zeroGActive) return;

        // Very gentle random drift
        Vector3 drift = new Vector3(Mathf.Sin(Time.time * 0.3f) * driftForce, 0.5f, Mathf.Cos(Time.time * 0.4f) * driftForce);

        rb.AddForce(drift * Time.deltaTime);

        // Slow rotation
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right * (rotationSpeed * 0.6f) * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!zeroGActive) return;

        // Softer, more realistic bounce
        Vector3 bounce = collision.contacts[0].normal * 1f;     // Reduced from 8f
        rb.linearVelocity = bounce + Random.insideUnitSphere * 1f;    // Reduced randomness
    }

    public void DeactivateZeroG()
    {
        zeroGActive = false;
        rb.useGravity = true;
        rb.linearDamping = 0.1f;     // Normal damping
        rb.angularDamping = 0.1f;

        // Optional: Give a small downward push so they fall nicely
        rb.AddForce(Vector3.down * 3f, ForceMode.Impulse);

        Debug.Log("Crate gravity restored: " + gameObject.name);
    }
}
