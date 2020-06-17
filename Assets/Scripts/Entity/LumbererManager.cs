using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.AI;

public class LumbererManager : EntityManager<Lumberer, LumbererManager>
{
    public float lumbererHeight = 0.45f;
    Lumberer lumberer;

    // minDist 的说明见 Lumberer::SetDestination::modDist
    public float minDist = .3f;
    public float distToOuterTree;
    public int testSpawnCount = 1;
    public float testSpawnCD = 1f;
    public float navMeshAgentSpeed = 1f;
    public GameObject LumbererEscape;
    void Start()
    {
        ArrangeChild();
        // for test
        // Spawn();
    }

    public void Spawn()
    {
        StartCoroutine(Spawn_());
    }

    public void Spawn(int count, float cd)
    {
        StartCoroutine(Spawn_(count, cd));
    }

    IEnumerator Spawn_()
    {
        var spawnRadius = distToOuterTree + Trees.I.radius;
        while (entitys.Count < testSpawnCount)
        {
            var lumb = CreateEntity(Helper.RandomOnCircle(Vector3.up * lumbererHeight, spawnRadius));
            SetTargetTree4Lumberer(lumb);
            yield return new WaitForSeconds(testSpawnCD);
        }
        yield return null;
    }

    IEnumerator Spawn_(int count, float cd)
    {
        var spawnRadius = distToOuterTree + Trees.I.radius;
        for (int k = 0; k < count; k++)
        {
            var lumb = CreateEntity(Helper.RandomOnCircle(Vector3.up * lumbererHeight, spawnRadius));
            SetTargetTree4Lumberer(lumb);
            yield return new WaitForSeconds(cd);
        }
        yield return null;
    }

    public void SetTargetTree4Lumberer(Lumberer lumberer)
    {
        Tree closestTree = Trees.I.GetClosestUnaimedTree(lumberer.transform.position);
        if (closestTree != null)
        {
            lumberer.SetTargetTree(closestTree, minDist);
            // 为版本 0.2-alpha release 版本特别注释，防止游戏无法进行下去
            Trees.I.RemoveTreeFromUnaimmed(closestTree);      // 将已经被占用的树从 KDTree 队列中删除
        }
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

    public bool RemoveLumberer(Lumberer lumberer)
    {
        bool ret = true;
        for (int idx = 0; idx != entitys.Count; idx++)
        {
            if (entitys[idx] == lumberer)
            {
                entitys.RemoveAt(idx);
                ret = false;
                break;
            }
        }
        return ret;
    }

}
