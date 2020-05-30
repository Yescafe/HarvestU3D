using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    [NonSerialized] public bool isLumbererWon = false;
    [NonSerialized] public bool isNatureWon = false;

    public GameObject dispInfo;

    public void UpdateStatus()
    {
        if (Trees.I.GetComponent<Transform>().childCount == 0)
        {
            isLumbererWon = true;
            isNatureWon = !isLumbererWon;
        }
    }

    public void Update()
    {
        UpdateStatus();
        if (isLumbererWon)
        {
            dispInfo.GetComponent<Text>().text = "Lumberers Won!";
        }
    }
}
