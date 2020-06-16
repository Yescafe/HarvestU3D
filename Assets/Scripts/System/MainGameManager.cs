using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : DCLSingletonBase<MainGameManager>
{
    public Text natureInfo;
    public int maxNaturePower = 100;
    

    private int naturePower = 0;
    [NonSerialized] public int treeCnt = 0;
    private int maxTreeCnt;

    void Start()
    {
        maxTreeCnt = Trees.I.trees.Count;
        UpdateNaturePower();
    }

    private void UpdateNaturePower()
    {
        var maxRedness = .6f;
        natureInfo.text = $"{naturePower} / {maxNaturePower}\n{treeCnt} / {maxTreeCnt}";
        natureInfo.color = new Color(1f, 1f - .01f * maxRedness * naturePower, 1f - .01f * maxRedness * naturePower, 1f);
        CameraPostProcessing.I.SetSaturation(naturePower);    // 调整色调
        BGMPlay.I.SetVolume(70f - naturePower);               // BGM 音量随着 nature power 的升高而降低, 70 终止
        HeartBeatPlay.I.SetVolume(naturePower - 20f);         // 调整心跳音量，nature power 20 起步
        Debug.Log(natureInfo.color);
    }

    public void IncNaturePower(int point)
    {
        naturePower += point;
        if (naturePower < 0) naturePower = 0;
        if (naturePower > maxNaturePower) naturePower = maxNaturePower;
        UpdateNaturePower();
        if (naturePower >= maxNaturePower)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
