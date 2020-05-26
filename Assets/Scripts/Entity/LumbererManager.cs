using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LumbererManager : DCLSingletonBase<LumbererManager>
{
    Lumberer lumberer;

    void Start()
    {
        lumberer = GetComponent<Lumberer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Attack 1 trig");
            lumberer.Attack1();
        }

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (!GetComponent<Lumberer>().isAttacking)
        {
            lumberer.Walk(new Vector3(h, 0f, v));

            var lookAt = Vector3.forward * v + Vector3.right * h;
            if (lookAt.magnitude != 0)
            {
                lumberer.Face(lookAt);
            }
        }
        else
            lumberer.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        if (lumberer.isAttacking && other.gameObject.CompareTag("Tree"))
        {
            Debug.Log("OnTriggerStay Ran");
            var damage = lumberer.power - other.gameObject.GetComponent<Tree>().defense;
            damage = damage < 0f ? 0f : damage;
            Debug.Log("Get Damage: " + damage);
            other.gameObject.GetComponent<Tree>().GetDamage(damage);
        }
    }
}
