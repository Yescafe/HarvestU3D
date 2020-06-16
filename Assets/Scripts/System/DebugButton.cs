using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : MonoBehaviour
{
    public void OnButtonDown()
    {
        Debug.Log($"Trees.I.trees count = {Trees.I.trees.Count}");
    }
}
