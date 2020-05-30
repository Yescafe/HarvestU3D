using UnityEngine;
using System.Text;

public class ScreenLogger : DCLSingletonBase<ScreenLogger> {
    // private string loginfo;
    private StringBuilder loginfo = new StringBuilder();

    public bool refreshUpdate = false;

    public void Add(string info) => loginfo.Append(info);
    public void AddLine(string info) => loginfo.Append(info + '\n');
    public void Clear() => loginfo.Clear();

    private void LateUpdate()
    {
        // refresh 的时机存在问题，放在 GUI.Label 以后也会造成完全清除的结果
        if (refreshUpdate)
        {
            loginfo.Clear();
        }
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 400, 200), loginfo.ToString());
    }
}