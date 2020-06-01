using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trees : DCLSingletonBase<Trees>
{
    public List<GameObject> treeCategory;
    /// <summary>
    /// 包含 碰撞体 和 Tree 脚本的模板
    /// </summary>
    public GameObject treeTemplate;
    public float radius = 10f;
    public int treeNumberToGen;

    // TODO[-] 修改成 private
    // 将“trees 队列”的意义定义为“等待被预约的 tree 队列”
    public KdTree<Tree> trees = new KdTree<Tree>();
    // TODO[x] 被伐木人瞄准了的树木，在这里的所有树，会在 GetClosestTree 的查找内容中忽略
    // 该内容以全权转接给 this.trees 实现，无须重复实现 (LumbererManager.cs:54)
    // private HashSet<Tree> aimedTrees = new HashSet<Tree>();

    // TODO 如何序列化这玩意，以便在 Inspector 进行修改
    public GenerateType generator;

    public GameObject treeStump;

    public enum GenerateType
    {
        Circle,
        GuassianSquare,
    }

    Dictionary<GenerateType, Action> generators;

    void Awake()
    {
        generators = new Dictionary<GenerateType, Action>()
    {
        {GenerateType.Circle, CircleGenerate},
        {GenerateType.GuassianSquare, GuassianGenerate}
    };
        GenerateTrees();
    }

    public Tree GetClosestTree(Vector3 pos) => trees.FindClosest(pos);

    public void RegisterTree(Tree tree)
    {
    }

    public bool RemoveTree(Tree tree)
    {
        bool ret = true;
        for (int idx = 0; idx != trees.Count; idx++)
        {
            if (trees[idx] == tree)
            {
                trees.RemoveAt(idx);
                ret = false;
                break;
            }
        }
        return ret;
    }

    #region Generation

    [ContextMenu("Generate Trees")]
    public void GenerateTrees()
    {
        // 如果放在 Start 中，在 Editor 状态下不会去执行，因此放在该函数内部

        Helper.ClearAllChild(transform);
        trees.Clear();
        for (int i = 0; i < treeNumberToGen; i++)
        {
            generators[generator]?.Invoke();
            // GuassianGenerate();
        }
    }

    /// <summary>
    /// 平均分布，随机取一个圆的面积，然后通过这个面积来获得到半径值，这样的 随机半径值 和 随机角度 的结合，满足平均分布
    /// https://medium.com/@dreamume/leetcode-478-generate-random-point-in-a-circle-efc5590c5065
    /// https://stackoverflow.com/a/50746409/9337675
    /// </summary>
    void CircleGenerate()
    {
        var ranIdx = (int)UnityEngine.Random.Range(0f, treeCategory.Count);

        var ranArea = UnityEngine.Random.Range(0f, radius * radius);
        var ranRadius = Mathf.Sqrt(ranArea);
        var ranAngle = UnityEngine.Random.Range(0f, 360f);
        var ranH = ranRadius * Mathf.Sin(ranAngle);
        var ranV = ranRadius * Mathf.Cos(ranAngle);

        CreateTree(treeCategory[ranIdx], new Vector3(ranH, .45f, ranV));
    }

    void GuassianGenerate()
    {
        var ranIdx = (int)UnityEngine.Random.Range(0f, treeCategory.Count);

        // Generate trees with Gaussian distribution
        var ranH = NextGaussian(0f, 1 / 2f, -1f, 1f) * radius / 2;
        var ranV = NextGaussian(0f, 1 / 2f, -1f, 1f) * radius / 2;

        var ranScale = UnityEngine.Random.Range(.9f, 1.1f);
        Debug.Log("ranIdx = " + ranIdx + " ranH = " + ranH + " ranV = " + ranV + " ranScale = " + ranScale);

        CreateTree(treeCategory[ranIdx], new Vector3(ranH, .45f, ranV), ranScale);
    }

    void CreateTree(GameObject model, Vector3 pos, float scale = 1f)
    {
        var newTreeHolder = Instantiate(treeTemplate, pos, Quaternion.identity, transform);
        var newTree = Instantiate(model, newTreeHolder.transform);
        newTreeHolder.transform.localScale *= scale;
        newTreeHolder.gameObject.name = $"TreeHolder {trees.Count}";
        trees.Add(newTreeHolder.GetComponent<Tree>());
    }

    #endregion

    #region RNG

    float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }

    float NextGaussian(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;
    }

    float NextGaussian(float mean, float standard_deviation, float min, float max)
    {
        float x;
        do
        {
            x = NextGaussian(mean, standard_deviation);
        } while (x < min || x > max);
        return x;
    }

    #endregion
}
