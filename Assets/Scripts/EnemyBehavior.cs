using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float reachedPointThreshold;

    [SerializeField] private bool cycle; 
    [SerializeField] private float viewRadius, viewAngle;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private GameObject modelParent;
    public GameObject player;

    public enum EnemyState
    {
        idle,
        patrolling,
        chasing,
        dead
    }

    public EnemyState currentState;
    private NavMeshAgent agent;
    private Transform currentPatrolTarget;
    private Animator anim;
    private int currentPatrolIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();

        currentState = EnemyState.patrolling;

        if(patrolPoints.Length != 0)
        {
            SetNewPatrolPoint();
        }
    }

    private void Update()
    {
        CheckForAndSetStates();

        switch (currentState)
        {
            case EnemyState.idle:
                Idle();
                break;

            case EnemyState.patrolling:
                Patrol();
                break;

            case EnemyState.chasing:
                ChasePlayer();
                break;

            case EnemyState.dead:
                break;
        }
    }

    private void ChasePlayer()
    {
        InView(out Transform player);

        agent.SetDestination(player.position);
    }

    private void CheckForAndSetStates()
    {
       // Check if the enemy is dead
        if (GetComponentInChildren<IHealth>() != null && GetComponentInChildren<IHealth>().GetCurrentHealth() <= 0)
        {
            currentState = EnemyState.dead;
            return; // Exit early if dead
        }
        // Check if the player is in view
        if (InView(out Transform player))
        {
            currentState = EnemyState.chasing;
        }
        // Check if there are patrol points and the player is not in view
        else if (patrolPoints.Length != 0)
        {
            currentState = EnemyState.patrolling;
        }
        // Set to idle if none of the above conditions are met
        else
        {
            currentState = EnemyState.idle;
        }
    }

    private void Idle()
    {
        anim.SetBool("IsWalking", false);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    private void Patrol()
    {   
        anim.SetBool("IsWalking", true);

        agent.SetDestination(currentPatrolTarget.position);

        if(Vector3.Distance(transform.position, currentPatrolTarget.position) < reachedPointThreshold)
        {
            SetNewPatrolPoint();
        }
    } 

    private void SetNewPatrolPoint()
    {
        if(cycle)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            currentPatrolTarget = patrolPoints[currentPatrolIndex];
        }
        else
        {
            currentPatrolTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
        }
    }

    private bool InView(out Transform player)
    {
        player = null;
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);
        
        foreach(Collider obj in targets)
        {
            Vector3 directionToTarget = (obj.transform.position - transform.position).normalized;
            Vector3 forward = modelParent.transform.forward;
            
            // Calculating angle formula
            float dotProduct = Vector3.Dot(directionToTarget, forward);

            float angleInRadians = Mathf.Acos(dotProduct);

            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

            Debug.Log("Angle: " + angleInDegrees);
            
            if(Mathf.Abs(angleInDegrees) <= viewAngle)    
            {
                if(Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, viewRadius))
                {
                    if(hit.collider == obj)
                    {
                        player = hit.collider.gameObject.transform;
                        return true;
                    }
                }
            }

        }
        player = null;
        return false;
    }
        
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, player.transform.position);

        // Draw the detection range as a wire sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // Draw the 90-degree field of view
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle, 0) * transform.forward * viewRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle, 0) * transform.forward * viewRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }

    
}
