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
    public float maxNaturePower = 100;
    

    private float naturePower = 0;
    [NonSerialized] public int treeCnt = 0;
    private int maxTreeCnt;
    private int spendMinutes = 0;

    private int updateTimeUnit = 60;                    // 60s is default

    int lumbererSpawnAmountPreUTU = 6;               // lumberer spawn amount pre updateTimeUnit 
    int deltaLumbererSpawnAmountPreUTU = 3;          // update delta of lumberer spawn amount pre updateTimeUnit 
    void Start()
    {
        maxTreeCnt = Trees.I.trees.Count;
        UpdateNaturePower();
    }

    private void Update()
    {
        var unit = updateTimeUnit;
        float timeScaleIncDegree = .1f;                           // .1f speed per 1 minute is default
        int nowSpendMinutes = (int) Time.realtimeSinceStartup / unit + 1;    // do behavior pre `unit` second
        if (nowSpendMinutes > spendMinutes) {
            // world time scale
            PauseButton.I.curTimeScale += timeScaleIncDegree;
            spendMinutes = nowSpendMinutes;
            Time.timeScale = PauseButton.I.curTimeScale;  // Update world time scale
            
            // updates of lumberer manager 
            var lm = LumbererManager.I;
            lumbererSpawnAmountPreUTU += deltaLumbererSpawnAmountPreUTU;
            lm.Spawn(lumbererSpawnAmountPreUTU, (float) unit / lumbererSpawnAmountPreUTU);

            // updates of bird manager
            var bm = BirdManager.I;

        }
    }

    private void UpdateNaturePower()
    {
        var maxRedness = .75f;
        natureInfo.text = $"{(int) naturePower} / {(int) maxNaturePower}";
        natureInfo.color = new Color(1f, 1f - .01f * maxRedness * naturePower, 1f - .01f * maxRedness * naturePower, 1f);
        CameraPostProcessing.I.SetSaturation(naturePower);    // 调整色调
        BGMPlay.I.SetVolume(70f - naturePower);               // BGM 音量随着 nature power 的升高而降低, 70 终止
        HeartBeatPlay.I.SetVolume(naturePower - 20f);         // 调整心跳音量，nature power 20 起步
    }

    public void IncNaturePower(float point)
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

    public void IncNaturePower(String category)
    {
        float incPower = 0;
        if (category == "AxeHit") incPower = 1f;
        else if (category == "LumbererEscape")  incPower = -4.5f;
        else if (category == "SpawnBird") incPower = 4f;
        IncNaturePower(incPower);
    }

}
