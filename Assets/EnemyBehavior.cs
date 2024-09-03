using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform modelParent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float reachedPointThreshold;

    private Transform currentPatrolTarget;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        SetNewPatrolPoint();
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, currentPatrolTarget.position) < reachedPointThreshold)
            SetNewPatrolPoint();

        else
        {
            Move();
            Rotate(currentPatrolTarget);
        }
        
    }

    private void Move()
    {
        // Move towards patrol point
        Vector3 desiredMoveDirection = modelParent.transform.forward;

        // Apply the movement
        rb.MovePosition(transform.position + moveSpeed * Time.deltaTime * desiredMoveDirection);
    }  

    private void Rotate(Transform target)
    {
        // Rotate towards target
    }

    private void SetNewPatrolPoint()
    {
        Transform point = patrolPoints[Random.Range(0, patrolPoints.Length - 1)];

        currentPatrolTarget = point;
    }

}
