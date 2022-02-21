using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_State : StateMachineBehaviour
{
    public bool Turn_White;
    public bool Turn_Black;
    public bool Rise;

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Turn_White == true)
        {
            animator.SetBool("Turn_White", Turn_White);
        }
        if (Turn_Black == true)
        {
            animator.SetBool("Turn_Black", Turn_Black);
        }

        if (Turn_White == false)
        {
            animator.SetBool("Turn_White", Turn_White);
        }
        if (Turn_Black == false)
        {
            animator.SetBool("Turn_Black", Turn_Black);
        }
    }

    //OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
       
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
