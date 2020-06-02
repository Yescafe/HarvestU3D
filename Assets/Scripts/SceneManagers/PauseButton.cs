using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private bool isPaused = false;
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
            Time.timeScale = 1f;
            GetComponentInChildren<Text>().text = "| |";
            GetComponentInChildren<Text>().fontSize = 16;
        }
        isPaused = !isPaused;
    }
}
