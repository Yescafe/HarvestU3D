using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject grassBlockPrefeb;
    public Transform ground;
    public float rate;
    public float sideWidth = 15f;

    private List<GameObject> grassBlocks;

    void Start()
    {
        // Construct
        grassBlocks = new List<GameObject>();

        // Test part
        // InvokeRepeating("ClockBehavior", rate, rate);  // do ClockBehavior() per 3 sec.

        // Set ground
        for (float x = -sideWidth / 2; x <= sideWidth / 2; x += 1f)
            for (float y = -sideWidth / 2; y <= sideWidth / 2; y += 1f)
                grassBlocks.Add(Instantiate<GameObject>(grassBlockPrefeb, new Vector3(x, 0f, y), Quaternion.identity, ground));
    }

    void ClockBehavior()
    {
        Debug.Log("clock up.");
        var ran = new System.Random();
        int idx = ran.Next(0, grassBlocks.Count - 1);
        Destroy(grassBlocks[idx]);
        grassBlocks.RemoveAt(idx);
    }
}
