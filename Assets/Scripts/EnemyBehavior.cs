using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform modelParent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float reachedPointThreshold;
    [SerializeField] private float rotationSpeed;

    // Vector used by the smoothdamp function. Needs to be initialized to zero 
    // then is handled by the smoothdamp method
    private Vector3 currentVelocity;    
    private Transform currentPatrolTarget;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentVelocity = Vector3.zero;

        SetNewPatrolPoint();
    }

    private void FixedUpdate()
    {
        if(patrolPoints == null)
            return;

        if(Vector3.Distance(transform.position, currentPatrolTarget.position) < reachedPointThreshold)
        {
            SetNewPatrolPoint();
            Rotate();
        }

        Move();
    }

    private void Move()
    {   
        // MoveDirection
        Vector3 moveDireciton = (currentPatrolTarget.position - transform.position).normalized;

        // Apply the movement
        rb.MovePosition(transform.position + moveSpeed * Time.deltaTime * moveDireciton);
    }  

    private void Rotate()
    {
        Vector3 targetVector = (currentPatrolTarget.position - modelParent.position).normalized;

        StartCoroutine(SmoothRotate(targetVector));
    }

    private IEnumerator SmoothRotate(Vector3 targetVector)
    {
        float elapsedTime = 0f;

        targetVector *= -1;

        Quaternion targetRotation = Quaternion.LookRotation(targetVector);
        Quaternion initialRotation = modelParent.localRotation;

        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        while (elapsedTime < rotationSpeed)
        {
            modelParent.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / rotationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is set
        modelParent.localRotation = targetRotation;
    }

    private void SetNewPatrolPoint()
    {
        Transform point = patrolPoints[Random.Range(0, patrolPoints.Length)];

        currentPatrolTarget = point;
    }

}
