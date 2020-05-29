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
        // Trees.I.RemoveTree(this);
        //
        // WARN HERE
        //
        // 这段逻辑可能有待优化。为防止 handling reference，需要让 KDTree 取消对即将
        // 删除的对象的引用。这里采用遍历的方法，复杂度有点高。但是如果没办法也无大碍。
        //
        // Find 方法在本操作中并无实际作用。
        KdTree<Tree> trees = GetComponentInParent<Trees>().trees;
        for (int idx = 0; idx != trees.Count; idx++)
        {
            if (trees[idx] == this)
            {
                trees.RemoveAt(idx);
                break;
            }
        }
    
        Destroy(gameObject);
    }
}
