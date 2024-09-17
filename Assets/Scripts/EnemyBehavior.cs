using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float reachedPointThreshold;

    // Vector used by the smoothdamp function. Needs to be initialized to zero 
    // then is handled by the smoothdamp method 
    private NavMeshAgent agent;
    private Transform currentPatrolTarget;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();

        if(patrolPoints.Length != 0)
        {
            SetNewPatrolPoint();
        }
    }

    private void FixedUpdate()
    {
        // if(patrolPoints.Length = 0)
        {
            anim.SetBool("IsWalking", false);
            return;
        }

        if(Vector3.Distance(transform.position, currentPatrolTarget.position) < reachedPointThreshold)
        {
            SetNewPatrolPoint();
        }

        Move();
    }

    private void Move()
    {   
        anim.SetBool("IsWalking", true);

        agent.SetDestination(currentPatrolTarget.position);
    }  

    private void SetNewPatrolPoint()
    {
        Transform point = patrolPoints[Random.Range(0, patrolPoints.Length)];

        currentPatrolTarget = point;
    }

}
