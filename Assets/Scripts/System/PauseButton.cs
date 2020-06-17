using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseButton : DCLSingletonBase<PauseButton>
{
    private bool isPaused = false;
    [NonSerialized] public float curTimeScale = 1f;
    public void OnButtonDown()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            GetComponentInChildren<Text>().text = ">";
            GetComponentInChildren<Text>().fontSize = 18;
        }
        else
        {
            Time.timeScale = curTimeScale;
            GetComponentInChildren<Text>().text = "| |";
            GetComponentInChildren<Text>().fontSize = 16;
        }
        isPaused = !isPaused;
    }
}
