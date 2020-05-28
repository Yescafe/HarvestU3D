using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public Lumberer lumberer;

    private Animator animator;
    private float power;

    void Start()
    {
        power = lumberer.power;
        animator = lumberer.animator;
    }

    // <summary> 
    // 触碰树之后完成触发器触发。
    // 注意：因为动画穿模等不可避免的问题，一次攻击动画可能会和树碰撞多次，造成二次伤害。
    //      于是增加当前攻击的命中判定，如果已经命中过，则将 `isHit` 设置为 `true`，
    //      防止二次伤害。`isHit` 会在 AttackAnimation.cs 中被复位。
    // </summary>
    public void OnTriggerExit(Collider other)
    {
        if (!lumberer.isHit && other.gameObject.CompareTag("Tree"))
        {
            var otherDefense = other.gameObject.GetComponent<Tree>().defense;
            other.gameObject.GetComponent<Tree>().TakeDamage(power - otherDefense, gameObject);
            lumberer.isHit = true;
            Debug.Log($"TakeDamage({power - otherDefense}), this attack has been hit.");
        }
    }
    
}
