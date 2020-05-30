using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lumberer : MonoBehaviour, IEntity
{
    public float speed;                 // walking speed 
    public float turnSpeed;             // turning speed

    public float power = 5f;            // attack power

    private CharacterController cc;
    public Animator animator;
    private NavMeshAgent agent;
    
    private bool isAlive = true;

    public bool isAttacking = false;    // 目前弃用
    public bool isHit = false;          // 命中，提供给 Axe.cs 使用

    public float health = 10f;

    public bool IsDead => !isAlive;

    [NonSerialized] public Tree closestTree;

    public void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        Debug.Assert(agent != null);
    }

    public void Update()
    {
        if (animator)
        {
            // Debug.Log($"agent.velocity = {agent.velocity}");
            animator.SetFloat("velocity", agent.velocity.magnitude);
            //
            // BUG HERE!
            // 

            // 防止打空 && 防止行走的时候还在攻击（会平移）（bug: 仍无法防止多余的一次攻击）
            if (closestTree && agent.velocity.magnitude == 0)
            {
                // 到达目的地时开始攻击
                if (agent.destination == transform.position)
                {
                    animator.SetTrigger("attack");
                }
            }
        }
    }


    #region Navigation

    public void SetDestination(Vector3 target) {
        Debug.Log($"Setted Destination: {target}");
        Debug.Assert(agent != null);
        Debug.Assert(agent.SetDestination(target), "Set Destination Failed");
    }

    /// <param name="modDist">
    ///     最终目的地距离目标的距离，预留一个大于 0 的值可以防止<del>秦王</del>伐木人绕树
    ///     对目前的情况，`.3f` 是个比较合适的值。该值正常情况下由 LumbererManager 控制。
    /// </param>
    public void SetDestination(Vector3 target, float modDist)
    {
        var dirToTarget = (target - transform.position).normalized;
        var modTargetPos = target - dirToTarget * modDist;
        Debug.Log($"Setted Destination: {modTargetPos}");
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();            
        Debug.Assert(agent != null);
        Debug.Assert(agent.SetDestination(modTargetPos), "Set Destination Failed");
    }

    public void SetTargetTree(Tree target, float modDist)
    {
        closestTree = target;
        SetDestination(target.transform.position, modDist);
    }

    #endregion

    #region Move(deprecated)

    public void Walk(Vector3 velocity)
    {
        if (!isAlive)
            return;

        Vector3 movement = velocity * speed;
        cc.SimpleMove(movement);

        if (animator)
        {
            // animator.SetFloat("velocity", (float)Math.Abs(cc.velocity.x + cc.velocity.z));
            animator.SetFloat("velocity", cc.velocity.magnitude);
        }
    }

    public void Stop()
    {
        this.Walk(new Vector3(0f, 0f, 0f));
    }

    public void Face(Vector3 lookAt)
    {
        var targetPosition = transform.position + lookAt;
        var thisPosition = transform.position;

        targetPosition.y = thisPosition.y = 0;

        var faceDirtVec = targetPosition - thisPosition;
        var faceDirtQuat = Quaternion.LookRotation(faceDirtVec);
        var slerp = Quaternion.Slerp(transform.rotation, faceDirtQuat, turnSpeed * Time.deltaTime);

        transform.rotation = slerp;
    }

    #endregion

    #region Attack(deprecated)

    public void Attack1()
    {
        if (!isAlive)
            return;
        if (isAttacking)
            return;

        if (animator)
        {
            animator.SetTrigger("attack");
        }

        this.isAttacking = true;
        // 对 isAttacking 状态的解除已由 Animator 的脚本托管
    }

    #endregion

    #region Life

    public void Death()
    {
        this.isAlive = false;
        DeathAnimation();
        // TODO 修改成 animator ... onExit 之类的
        // Invoke("TrueDie", dieSpeed);
        // 已完成由 Animator 的托管
    }

    private void DeathAnimation()
    {

    }

    public void TrueDie()
    {
        DestroyImmediate(this.GetComponent<GameObject>());
    }

    public void TakeDamage(float damage, GameObject attaker)
    {
        this.health -= damage;
        if (health <= 0f && isAlive)
        {
            Death();
        } 
        else
        {
            TakeDamageAnimation();
        }
    }

    private void TakeDamageAnimation()
    {

    }
    
    #endregion
}
