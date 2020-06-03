using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class MainGameManager : DCLSingletonBase<MainGameManager>
{
    [NonSerialized] public bool isLumbererWon = false;
    [NonSerialized] public bool isNatureWon = false;

    public Text dispInfo;
    public Text natureInfo;
    public int maxNaturePower = 100;

    private int naturePower = 0;

    void Start()
    {
        UpdateNaturePower();
    }

    private void UpdateNaturePower()
    {
        var maxRedness = .6f;
        natureInfo.text = $"{naturePower} / {maxNaturePower}";
        natureInfo.color = new Color(1f, 1f - .01f * maxRedness * naturePower, 1f - .01f * maxRedness * naturePower, 1f);
        // Debug.Log(natureInfo.color);
    }

    public void IncNaturePower(int point)
    {
        naturePower += point;
        UpdateNaturePower();
        if (naturePower >= maxNaturePower)
        {
            isLumbererWon = true;
            isNatureWon = false;
            dispInfo.text = "Lumberers Won!";
        }
    }
}
