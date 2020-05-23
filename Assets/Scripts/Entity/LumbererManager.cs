using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        lumberer.Walk(new Vector3(h, 0f, v));

        var lookAt = Vector3.forward * v + Vector3.right * h;
        if (lookAt.magnitude != 0)
        {
            lumberer.Face(lookAt);
        }
    }
}
