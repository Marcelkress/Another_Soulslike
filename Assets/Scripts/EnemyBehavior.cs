using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float reachedPointThreshold;

    [DetailedInfoBox("Cycle or random",
    "Specifies whether the enemy should choose patrol points at random, or cycle through them")]
    [SerializeField] private bool cycle; 
    [SerializeField] private float viewRadius, viewAngle;
    [SerializeField] private int playerLayer;

    enum EnemyState
    {
        patrolling,
        chasing,
        dead
    }

    private EnemyState currenState;
    private NavMeshAgent agent;
    private Transform currentPatrolTarget;
    private Animator anim;
    private int currentPatrolIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();

        currenState = EnemyState.patrolling;

        if(patrolPoints.Length != 0)
        {
            SetNewPatrolPoint();
        }
    }

    private void FixedUpdate()
    {
        if(patrolPoints.Length == 0)
        {
            anim.SetBool("IsWalking", false);
            return;
        }

        if(Vector3.Distance(transform.position, currentPatrolTarget.position) < reachedPointThreshold)
        {
            SetNewPatrolPoint();
        }
    }

    private void Update()
    {
        CheckForAndSetStates();

        switch (currenState)
        {
            case EnemyState.patrolling:
                Patrol();
                break;
            case EnemyState.chasing:
                // Call a chase method, haven't made yet.
                Debug.Log("CHASING MFSSSS");
                break;
            case EnemyState.dead:
                break;
            default:
                Patrol();
            break;
        }
    }

    private void CheckForAndSetStates()
    {
        if(GetComponentInChildren<IHealth>() != null)
        {
            if(GetComponentInChildren<IHealth>().GetCurrentHealth() >= 0)
            {
                currenState = EnemyState.dead;
            }
        }
        if(InView())
        {
            currenState = EnemyState.chasing;
        }
        else
        {
            currenState = EnemyState.patrolling;
        }
    }

    private void Patrol()
    {   
        anim.SetBool("IsWalking", true);

        agent.SetDestination(currentPatrolTarget.position);
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

    private bool InView()
    {
        // Perform a sphere cast to detect target
        if (Physics.SphereCast(transform.position, viewRadius, transform.forward, out RaycastHit hit, viewRadius, playerLayer))
        {   
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check if the target is within the field of view angle
            if (angleToTarget < viewAngle / 2)
            {
                Debug.Log("heahfeaf");
                // Perform a line-of-sight check
                if (!Physics.Linecast(transform.position, hit.transform.position, out RaycastHit lineHit))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        // Method used to draw the view in the scene
        Gizmos.color = Color.red;

        // Draw view radius
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        // Calculate view angle boundaries
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewRadius;

        // Draw view angle lines
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
