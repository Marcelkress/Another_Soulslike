using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;  

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange;
    [SerializeField] private Vector3 rayOffset;
    [SerializeField] private Transform characterParent; // The parent object of all the knight meshes
    [SerializeField] private LayerMask interactLayer;

    private PlayerHealth playerHealth; 
    private PlayerInput playerInput;
    private InputAction interactAction;
    private Vector3 rayOrigin, rayDirection;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();

        playerHealth.deathEvent.AddListener(DisableInteract);

        interactAction = playerInput.actions["Interact"];
        interactAction.started += Interact;
    }

    void Start()
    {

        rayOrigin = transform.position;
    }

    void Update()
    {
        rayDirection = characterParent.forward;
        rayOrigin = transform.position;
    }
 
    private void Interact(InputAction.CallbackContext context)
    {
        CastRay();
    }

    private void CastRay()
    {
        if (Physics.Raycast(rayOrigin + rayOffset, rayDirection, out RaycastHit hit, interactRange, interactLayer))
        {
            hit.collider.gameObject.GetComponent<IInteractable>()?.Interact();
        }

    }

    private void OnDrawGizmos()
    {
        // Update ray origin and direction in case they change in the editor
        rayOrigin = transform.position;
        rayDirection = characterParent.forward;

        // Draw the ray in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayOrigin + rayOffset, rayDirection * interactRange);
    }

    private void DisableInteract()
    {
        this.enabled = false;
    }

}
