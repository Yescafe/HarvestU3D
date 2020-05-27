using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IEntity
{
    public float health = 10f;
    public float defense = 0f;

    public void TakeDamage(float damage, GameObject attaker)
    {
        health -= damage;
        if (health <= 0f)
        {
            Death();
        }
    }
    
    void Death()
    {
        DeathAnimation();
        TrueDie();
    }

    void DeathAnimation()
    {

    }
    
    public void TrueDie()
    {
        Destroy(gameObject);
    }
}
