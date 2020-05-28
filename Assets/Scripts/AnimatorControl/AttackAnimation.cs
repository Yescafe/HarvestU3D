using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : StateMachineBehaviour
{
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
        var lmgr = lumberer.GetComponent<LumbererManager>();
        lumberer.isAttacking = false;
        // 最近的树已经销毁 && 伐木人未在攻击（目前停用）
        if (!lmgr.closestTree && !lumberer.isAttacking)
        {
            lmgr.closestTree = Trees.I.GetClosestTree(lumberer.transform.position);
            // 这里的 modDist 参数由 LumbererManager 控制。
            lumberer.SetDestination(lmgr.closestTree.transform.position, lmgr.minDist);
        }
        lumberer.isAttacking = false;
        lumberer.isHit = false;        // 关闭命中，等待下一次攻击
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
