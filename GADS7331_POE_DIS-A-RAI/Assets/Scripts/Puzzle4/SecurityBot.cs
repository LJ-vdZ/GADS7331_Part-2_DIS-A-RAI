using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class SecurityBot : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 5.5f;
    public float playerDetectionRange = 8f;
    public float blackBoxDetectionRange = 15f;
    public float attackRange = 1.8f;

    [Header("Animations")]
    public Animator animator;

    private NavMeshAgent agent;
    private Transform player;
    private Transform blackBox;

    private bool isChasing = false;
    private bool isDeactivated = false;
    private string currentState = "";

    private void Start()        // Changed from Awake to Start
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        blackBox = GameObject.FindGameObjectWithTag("BlackBox")?.transform;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isDeactivated) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToBox = blackBox != null ? Vector3.Distance(transform.position, blackBox.position) : 999f;

        if (distanceToPlayer < playerDetectionRange || distanceToBox < blackBoxDetectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
            ChasePlayer();
        else
            Patrol();

        // Game Over Check
        if (distanceToPlayer <= attackRange)
        {
            GameOver();
        }
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        if (player != null)
            agent.SetDestination(player.position);

        ChangeAnimationState("Run");
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (!agent.hasPath || agent.remainingDistance < 1.5f)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * 18f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 25f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        ChangeAnimationState("Walk");
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.SetTrigger(newState);
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER - Caught by Security Bot!");

        SceneManager.LoadScene("GameOver");

        //Time.timeScale = 0f;   // Pause game

        //SceneManager.LoadScene("GameOver");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDeactivated) return;

        if (collision.gameObject.CompareTag("WrongPowerCell"))
        {
            Deactivate();
            Destroy(collision.gameObject, 0.4f);
        }
    }

    public void Deactivate()
    {
        if (isDeactivated) return;

        isDeactivated = true;
        agent.enabled = false;

        if (animator != null)
            animator.enabled = false;

        Debug.Log($"{gameObject.name} has been deactivated!");
    }

    public bool IsDeactivated() => isDeactivated;
}