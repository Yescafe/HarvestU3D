using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lumberer : MonoBehaviour, IEntity
{
    public float speed;                 // walking speed 
    public float turnSpeed;             // turning speed

    public float atk = 5f;            // attack power
    public float takeDamageCD = 2f;

    private CharacterController cc;
    [NonSerialized] public Animator animator;
    

    [NonSerialized] public bool isAttacking = false;    // 目前弃用
    [NonSerialized] public bool isAttackTriggering = false;
    [NonSerialized] public bool isHit = false;          // 命中，提供给 Axe.cs 使用

    public float health = 10f;

    public bool IsAlive => isAlive;

    private NavMeshAgent agent;
    private bool isAlive = true;
    private float lastDamagedTime = 0f;

    [NonSerialized] public Tree closestTree;

    public void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.speed = LumbererManager.I.navMeshAgentSpeed;
        agent = GetComponent<NavMeshAgent>();
        Debug.Assert(agent != null);
    }

    public void Update()
    {
        // 攻击动画的优先级高于一切动作（发生碰撞除外，见Lumberer::OnTriggerEnter）
        if (!isAttackTriggering && animator)
        {
            // 攻击移动二选一
            if (closestTree != null && agent.destination == transform.position)
            {
                animator.SetFloat("velocity", 0f);     // 在攻击时强制停止移动动作
                animator.SetTrigger("attack");
                isAttackTriggering = true;        // Trigger 启动标志
            }
            else
            {
                animator.SetFloat("velocity", agent.velocity.magnitude);
            }
        }
    }

    /*
    public void OnTriggerEnter(Collider other)
    {
        // 防止伐木人之间互相卡位，完成随机位移
        // 因为碰撞是相互的，所以只对触发方进行实现，但实际上两个对象都会发生位移
        const float limitRange = .15f;    // [-.15f, .15f]
        if (other.gameObject.CompareTag("Lumberer"))
        {
            var randomVelocity = new Vector3(UnityEngine.Random.Range(-limitRange, limitRange),
                                             UnityEngine.Random.Range(-limitRange, limitRange),
                                             UnityEngine.Random.Range(-limitRange, limitRange));
            // cc.SimpleMove(randomVelocity);
        }
    }
    */

    #region Navigation

    public void SetDestination(Vector3 target) {
        Debug.Log($"{this.name} Setted Destination: {target}");
        Debug.Assert(agent != null);
        var assertInfo = agent.SetDestination(target);
        Debug.Assert(assertInfo, "Set Destination Failed");
    }

    /// <param name="modDist">
    ///     最终目的地距离目标的距离，预留一个大于 0 的值可以防止<del>秦王</del>伐木人绕树
    ///     对目前的情况，`.3f` 是个比较合适的值。该值正常情况下由 LumbererManager 控制。
    /// </param>
    public void SetDestination(Vector3 target, float modDist, String targetName)
    {
        var dirToTarget = (target - transform.position).normalized;
        var modTargetPos = target - dirToTarget * modDist;
        // Debug.Log($"{this.name} Setted Destination {modTargetPos} ({targetName})");
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        Debug.Assert(agent != null);
        var assertInfo = agent.SetDestination(modTargetPos);
        Debug.Assert(assertInfo, "Set Destination Failed");
    }

    public void SetTargetTree(Tree target, float modDist)
    {
        closestTree = target;
        if (target != null)
            SetDestination(target.transform.position, modDist, target.name);
        else
            Debug.Log("There is no more trees in the map. Lumberer won.");
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
        Debug.Log($"{name} dead");
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

    public void TakeDamage(float damage, GameObject attacker)
    {
        if (Time.time - lastDamagedTime < takeDamageCD) 
        {
            return;
        }
        lastDamagedTime = Time.time;
        Debug.Log($"Take {damage} damage from {attacker}");
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
