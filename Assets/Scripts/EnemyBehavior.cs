using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Patrol settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float reachedPointThreshold;
    [SerializeField] private bool cycle; 
    [SerializeField] private float walkSpeed;
    private NavMeshAgent agent;
    private Transform currentPatrolTarget;
    private int currentPatrolIndex = 0;

    private enum EnemyState
    {
        idle,
        patrolling,
        chasing,
        attacking,
        dead
    }

    [Header("Attack and view settings")]
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject modelParent;
    [SerializeField] private EnemyState currentState;
    [SerializeField] private float attackRange;
    public UnityEvent AttackEvent;
    private Animator anim;
    private bool playerSeen;
    private Transform playerTransform;
    
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
            
            case EnemyState.attacking:
                Attack();
                break;

            case EnemyState.dead:
                break;
        }
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.isStopped = false;

        anim.SetBool("IsChasing", true);
        agent.SetDestination(playerTransform.position);
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
        else if (SeenAndInRange() || InView())
        {
            //If the enemy is close enough to the player, attack
            if(Vector3.Distance(transform.position, playerTransform.position) < attackRange)
            {
                currentState = EnemyState.attacking;
            }
            else
            {
                currentState = EnemyState.chasing;
            }
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

    private bool SeenAndInRange()
    {
        if(InView() == true)
        {
            playerSeen = true;
        }

        if(playerSeen && Vector3.Distance(transform.position, playerTransform.position) < viewRadius)
        {
            return true;
        } 
        else
        {
            playerSeen = false;
            return false;
        }
    }

    private void Attack()
    {
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("Take hit"))
            return;
        anim.SetBool("IsChasing", false);
        
        anim.SetTrigger("Attack");
        AttackEvent?.Invoke();
        
        agent.isStopped = true;
    }

    private void Idle()
    {
        anim.SetBool("IsChasing", false);
        anim.SetBool("IsWalking", false);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    private void Patrol()
    {   
        anim.SetBool("IsChasing", false);
        anim.SetBool("IsWalking", true);

        agent.isStopped = false;

        agent.speed = walkSpeed;
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

    private bool InView()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);
        
        foreach(Collider obj in targets)
        {
            Vector3 directionToTarget = (obj.transform.position - transform.position).normalized;
            Vector3 forward = modelParent.transform.forward;
            
            // Calculating angle formula
            float dotProduct = Vector3.Dot(directionToTarget, forward);

            float angleInRadians = Mathf.Acos(dotProduct);

            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
            
            if(Mathf.Abs(angleInDegrees) <= viewAngle)    
            {
                if(Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, viewRadius))
                {
                    if(hit.collider == obj)
                    {
                        playerTransform = hit.collider.gameObject.transform;
                        return true;
                    }
                }
            }

        }
        return false;
    }
        
    private void OnDrawGizmos()
    {
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
