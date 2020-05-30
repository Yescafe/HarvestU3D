using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LumbererManager : DCLSingletonBase<LumbererManager>
{
    public GameObject lumbererPrefab;

    public float lumbererHeight = 0.45f;
    Lumberer lumberer;
    public int lumbererSpawnCount;

    // minDist 的说明见 Lumberer::SetDestination::modDist
    public float minDist = .3f;
    public float distToOuterTree;
    public int spawnCount = 1;
    public float spawnCD = 1f;

    private KdTree<Lumberer> lumberers = new KdTree<Lumberer>();

    void Start()
    {
        ArrangeChild();
        StartCoroutine(Spawn());
    }

    void ArrangeChild()
    {
        var tmps = transform.GetComponentsInChildren<Lumberer>();
        foreach (var lumb in tmps)
        {
            lumberers.Add(lumb);
        }
    }

    IEnumerator Spawn()
    {
        var spawnRadius = distToOuterTree + Trees.I.radius;
        while (lumberers.Count < spawnCount)
        {
            var lumb = CreateLumberer(Helper.RandomOnCircle(Vector3.up * lumbererHeight, spawnRadius));
            SetTargetTree4Lumberer(lumb);
            yield return new WaitForSeconds(spawnCD);
        }
        yield return null;
    }

    Lumberer CreateLumberer(Vector3 pos)
    {
        var go = Instantiate(lumbererPrefab, pos, Quaternion.identity, transform);
        var lumb = go.GetComponent<Lumberer>();
        lumberers.Add(lumb);
        Debug.Log($"Created Lumberer at {pos}");
        return lumb;
    }

    public void SetTargetTree4Lumberer(Lumberer lumberer)
    {
        Tree closestTree = Trees.I.GetClosestTree(lumberer.transform.position);
        if (closestTree != null)
        {
            lumberer.SetTargetTree(closestTree, minDist);
            Trees.I.RemoveTree(closestTree);      // 将已经被占用的树从 KDTree 队列中删除
        }
    }

    public Lumberer GetClosest(Vector3 pos) => lumberers.FindClosest(pos);
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
