using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics; // Required for spline evaluation

public class SplineFollower : MonoBehaviour
{
    [Header("Spline Settings")]
    public SplineContainer splineContainer;
    public float speed = 0.08f;           // Speed along the spline (adjust to taste)

    [Header("Movement")]
    public bool loop = true;
    public float smoothness = 8f;

    [Header("Rotation")]
    public bool alignToPath = true;
    public float tiltAmount = 15f;

    private float currentT = 0f;

    void Start()
    {
        // Optional: Start at random position on the spline
        RandomizeStartPosition();
    }

    void Update()
    {
        if (splineContainer == null) return;

        currentT += speed * Time.deltaTime;
        if (currentT > 1f)
        {
            currentT = loop ? 0f : 1f;
        }

        // Evaluate spline
        float3 position;
        float3 tangent;
        splineContainer.Spline.Evaluate(currentT, out position, out tangent, out _);

        Vector3 worldPos = splineContainer.transform.TransformPoint(position);
        Vector3 worldTangent = splineContainer.transform.TransformDirection(tangent);

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, worldPos, smoothness * Time.deltaTime);

        // Rotation + banking
        if (alignToPath && worldTangent.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(worldTangent);

            // Banking (tilt)
            float bank = -Vector3.Dot(worldTangent.normalized, Vector3.right) * tiltAmount;
            targetRot *= Quaternion.Euler(0, 0, bank);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 8f * Time.deltaTime);
        }
    }

    public void RandomizeStartPosition()
    {
        currentT = UnityEngine.Random.value;   // Fixed: explicit UnityEngine.Random
    }
}
