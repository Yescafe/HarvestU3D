using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LumbererManager : DCLSingletonBase<LumbererManager>
{
    Lumberer lumberer;
    private KdTree<Lumberer> lumberers = new KdTree<Lumberer>();

    // minDist 的说明见 Lumberer::SetDestination::modDist
    public float minDist = .3f;

    public Tree closestTree;

    void Start()
    {
        lumberer = GetComponent<Lumberer>();
        Debug.Assert(lumberer != null);
        Debug.Assert(Trees.I.GetClosestTree(lumberer.transform.position) != null, $"{Trees.I.trees.Count}");
        closestTree = Trees.I.GetClosestTree(lumberer.transform.position);
        lumberer.SetDestination(closestTree.transform.position, minDist);
    }


#if USER_CONTROL
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
#endif

}
