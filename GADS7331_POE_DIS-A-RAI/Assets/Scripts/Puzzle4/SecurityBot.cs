using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SecurityBot : MonoBehaviour
{
    [Header("Bot Settings")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 5f;
    public float detectionRange = 12f;

    private NavMeshAgent agent;
    private Transform player;
    private Transform blackBox;
    private bool isChasing = false;
    private bool isDeactivated = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        blackBox = GameObject.FindGameObjectWithTag("BlackBox")?.transform;
    }

    private void Update()
    {
        if (isDeactivated) return;

        if (blackBox != null)
        {
            float distanceToBox = Vector3.Distance(transform.position, blackBox.position);

            if (distanceToBox < detectionRange)
            {
                isChasing = true;
            }
        }

        if (isChasing)
            ChasePlayer();
        else
            Patrol();
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        if (player != null)
            agent.SetDestination(player.position);
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;
        if (!agent.hasPath || agent.remainingDistance < 2f)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * 20f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 25f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    // ====================== POWER CELL HIT DETECTION ======================
    private void OnCollisionEnter(Collision collision)
    {
        if (isDeactivated) return;

        // Check for Wrong Power Cell
        if (collision.gameObject.CompareTag("WrongPowerCell"))
        {
            Deactivate();

            // Optional: Destroy the power cell after impact
            Destroy(collision.gameObject, 0.3f);
        }
    }

    public void Deactivate()
    {
        if (isDeactivated) return;

        isDeactivated = true;
        agent.enabled = false;

        // Visual feedback
        //GetComponent<Renderer>()?.material.color = Color.gray;

        Debug.Log($"{gameObject.name} has been deactivated by Wrong Power Cell!");
    }

    public bool IsDeactivated() => isDeactivated;
}
