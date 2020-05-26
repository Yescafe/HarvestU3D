using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trees : DCLSingletonBase<Trees>
{
    public List<GameObject> treeCategory;

    // TODO 如何序列化这玩意，以便在 Inspector 进行修改
    public UnityAction generator;

    public int treeNumberToGen;

    private float sideWidth;

    void Start()
    {
        generator = CircleGenerate;
        GenerateTrees();
    }


    [ContextMenu("Generate Trees")]
    public void GenerateTrees()
    {
        // 如果放在 Start 中，在 Editor 状态下不会去执行，因此放在该函数内部
        // 并且在 ground 进行变化以后依然可以随着扩大，而不是只是在 start 中得到一个起始固定值
        sideWidth = Ground.I.sideWidth;

        for (int i = 0; i < treeNumberToGen; i++)
        {
            // generator?.Invoke();
            GuassianGenerate();
        }
    }

    void CircleGenerate()
    {
        var ranIdx = (int)UnityEngine.Random.Range(0f, treeCategory.Count);
        
        // Generate in a circle
        var ranH = UnityEngine.Random.Range(-sideWidth / 2, sideWidth / 2);
        // var ranV = Random.Range(-sideWidth / 2, sideWidth / 2);
        var ranUnit = UnityEngine.Random.Range(-1f, 1f);
        var ranV = (float)(ranUnit * Math.Sqrt(Math.Pow(sideWidth / 2, 2) - Math.Pow(Math.Abs(ranH), 2)));


        Instantiate(treeCategory[ranIdx], new Vector3(ranH, .45f, ranV), Quaternion.identity, transform);
    }

    void GuassianGenerate()
    {
        var ranIdx = (int)UnityEngine.Random.Range(0f, treeCategory.Count);

        // Generate trees with Gaussian distribution
        var ranH = NextGaussian(0f, 1 / 2f, -1f, 1f) * sideWidth / 2;
        var ranV = NextGaussian(0f, 1 / 2f, -1f, 1f) * sideWidth / 2;

        var ranScale = UnityEngine.Random.Range(.9f, 1.1f);
        Debug.Log("ranIdx = " + ranIdx + " ranH = " + ranH + " ranV = " + ranV + " ranScale = " + ranScale);

        treeCategory[ranIdx].GetComponent<Transform>().localScale *= ranScale;
        Instantiate(treeCategory[ranIdx], new Vector3(ranH, .45f, ranV), Quaternion.identity, transform);
        treeCategory[ranIdx].GetComponent<Transform>().localScale /= ranScale;
    }

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
}
