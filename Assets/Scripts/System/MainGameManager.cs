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
    

    public GameObject ground;

    private int naturePower = 0;
    [NonSerialized] public int treeCnt = 0;
    private int maxTreeCnt;

    void Start()
    {
        UpdateNaturePower();

        // Test material
        List<Material> m = new List<Material>();
        ground.GetComponent<Renderer>().GetMaterials(m);
        Debug.Log($"grass color = {m[0].color}");
        m[0].color = new Color(
            m[0].color.r,
            m[0].color.g / 2,
            m[0].color.b,
            m[0].color.a / 2
        );

        maxTreeCnt = Trees.I.trees.Count;
    }

    private void UpdateNaturePower()
    {
        var maxRedness = .6f;
        natureInfo.text = $"{naturePower} / {maxNaturePower}\n{treeCnt} / {maxTreeCnt}";
        natureInfo.color = new Color(1f, 1f - .01f * maxRedness * naturePower, 1f - .01f * maxRedness * naturePower, 1f);
        // Debug.Log(natureInfo.color);
    }

    public void IncNaturePower(int point)
    {
        naturePower += point;
        if (naturePower < 0) naturePower = 0;
        if (naturePower > maxNaturePower) naturePower = maxNaturePower;
        UpdateNaturePower();
        if (naturePower >= maxNaturePower)
        {
            isLumbererWon = true;
            isNatureWon = false;
            dispInfo.text = "Lumberers Won!";
        }
    }
}
