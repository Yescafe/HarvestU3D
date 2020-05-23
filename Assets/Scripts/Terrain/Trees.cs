using System;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;
    public GameObject tree4;
    public int treeNumber;
    public Transform trees;
    public GameObject ground;

    private List<GameObject> treeCategory;

    private float sideWidth;
    void Start()
    {
        treeCategory = new List<GameObject>();
        treeCategory.Add(tree1);
        treeCategory.Add(tree2);
        treeCategory.Add(tree3);
        treeCategory.Add(tree4);

        sideWidth = ground.GetComponent<Ground>().sideWidth;

        for (int i = 0; i < treeNumber; i++)
        {
            var ranIdx = (int)UnityEngine.Random.Range(0f, treeCategory.Count);

            /*
            // Generate in a circle
            var ranH = UnityEngine.Random.Range(-sideWidth / 2, sideWidth / 2);
            // var ranV = Random.Range(-sideWidth / 2, sideWidth / 2);
            var ranUnit = UnityEngine.Random.Range(-1f, 1f);
            var ranV = (float)(ranUnit * Math.Sqrt(Math.Pow(sideWidth / 2, 2) - Math.Pow(Math.Abs(ranH), 2)));
            */

            // Generate trees with Gaussian distribution
            var ranH = NextGaussian(0f, 1 / 2f, -1f, 1f) * sideWidth / 2;
            var ranV = NextGaussian(0f, 1 / 2f, -1f, 1f) * sideWidth / 2;

            var ranScale = UnityEngine.Random.Range(.9f, 1.1f);
            Debug.Log("ranIdx = " + ranIdx + " ranH = " + ranH + " ranV = " + ranV + " ranScale = " + ranScale);

            treeCategory[ranIdx].GetComponent<Transform>().localScale *= ranScale;
            Instantiate(treeCategory[ranIdx], new Vector3(ranH, .45f, ranV), Quaternion.identity, trees);
            treeCategory[ranIdx].GetComponent<Transform>().localScale /= ranScale;
            
        }
    }

    void Update()
    {
        
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
