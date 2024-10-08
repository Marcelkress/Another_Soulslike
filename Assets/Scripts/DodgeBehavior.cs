using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DodgeBehavior : StateMachineBehaviour
{
    private Player_Controller pc;
    private PlayerHealth ph;
    private float previousSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ph = animator.gameObject.GetComponentInParent<PlayerHealth>();
        pc = animator.gameObject.GetComponentInParent<Player_Controller>();
        
        if (ph == null)
        {
            Debug.LogError("PlayerHealth component not found on " + animator.gameObject.name);
            return;
        }
        if (pc == null)
        {
            Debug.LogError("Player_Controller component not found on " + animator.gameObject.name);
            return;
        }


        previousSpeed = pc.currentSpeed;

        pc.currentSpeed = pc.dodgeRollSpeed;
        ph.isInvincible = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
       
    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pc.currentSpeed = previousSpeed;
        ph.isInvincible = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
