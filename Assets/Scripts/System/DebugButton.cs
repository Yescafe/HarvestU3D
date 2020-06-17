using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : MonoBehaviour
{
    public void OnButtonDown()
    {
        Debug.Log("----- Debug button triggered. -----");
        Debug.Log($"Trees.I.trees count = {Trees.I.trees.Count}");
        foreach (var bird in BirdManager.I.selectedBirds)
        {
            Debug.Log($"----- {bird.name} triggered. -----");
            bird.WatchLumberer();
        }
    }
}
