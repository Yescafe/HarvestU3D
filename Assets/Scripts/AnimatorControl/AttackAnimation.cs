using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : StateMachineBehaviour
{
    private void Start() {
        
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var lumberer = animator.GetComponent<Lumberer>();
        var lumbererManager = LumbererManager.I;
        bool hasSet = false;
        if (lumberer.closestTree == null && lumberer.isHit)
        {
            lumbererManager.SetTargetTree4Lumberer(lumberer);
            hasSet = true;
        }

        if (lumberer.isHit == true)        // 如果有攻击动画，则定需要攻击命中
            lumberer.isHit = false;        // 关闭命中，等待下一次攻击
        else if (!hasSet)                  // 有攻击动画没有击中的情况，修复空挥
            LumbererManager.I.SetTargetTree4Lumberer(lumberer);

        lumberer.isAttackTriggering = false;     // 取消攻击触发置位
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
