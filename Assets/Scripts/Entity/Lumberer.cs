using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberer : MonoBehaviour
{
    public float speed;                 // walking speed 
    public float turnSpeed;             // turning speed
    public float attackSpeed;           // time spent during attacking
    private float dieSpeed;             // time spent during dying (can't modify in the editor)

    private CharacterController cc;
    private Animator animator;

    private bool isAlive = true;
    private bool isAttacking = false;

    public float health = 10f;

    public void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

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
        Invoke("finishAttack", attackSpeed);
    }

    private void finishAttack()
    {
        this.isAttacking = false;
    }

    public void Death()
    {
        this.isAlive = false;
        DeathAnimation();
        Invoke("TrueDie", dieSpeed);
    }

    private void DeathAnimation()
    {

    }

    private void TrueDie()
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
}
