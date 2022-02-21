using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_State : StateMachineBehaviour
{
    public string Flag1;
    public string Flag2;
    public float Times;
    public bool Bool1;
    public bool Bool2;
    public float StateTime;
    Animator animator;


    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("St  " + "StateTime : " + StateTime + "Times : " + Times);

        this.animator = animator; 
        StateTime = stateInfo.normalizedTime;

        if (Bool1 != Bool2)
        {
            //Debug.Log("a " + "Flag1 : " + Flag1 + "   Flag2 : " + Flag2);
            //Debug.Log("a " + "Bool1 : " + Bool1 + "   Bool2 : " + Bool2);
            animator.SetBool(Flag1, Bool1);
        }


        if (StateTime >= Times)
        {
            Bool1 = false;
            animator.SetBool(Flag1, Bool1);
        }


        Flag2 = Flag1;
        Bool2 = Bool1;
    }

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
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
