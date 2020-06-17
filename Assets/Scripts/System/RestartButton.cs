using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void GameRestart()
    {
        System.Diagnostics.Process.Start(Application.dataPath + "/../Harvest.exe"); 
        Application.Quit();
    }
}
