using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lumberer : MonoBehaviour
{
    public float speed;                 // walking speed 
    public float turnSpeed;             // turning speed

    public float power = 5f;            // attack power

    private CharacterController cc;
    private Animator animator;
    private NavMeshAgent agent;
    

    private bool isAlive = true;
    public bool isAttacking { get; set; } = false;

    public float health = 10f;

    public void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    #region Navigation

    public void SetDestination(Vector3 target) {
        Debug.Log($"Setted Destination: {target}");
        Debug.Assert(agent.SetDestination(target), "Set Destination Failed");
    }

    #endregion

    #region Move

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

    #region Attack and HP

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

    public void TakeDamage(float damage)
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
