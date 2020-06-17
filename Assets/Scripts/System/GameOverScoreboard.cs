using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScoreboard : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Text>().text = $"感谢您游玩本游戏！\n您的森林存活了{(int) Time.realtimeSinceStartup / 60}分{(int) Time.realtimeSinceStartup % 60}秒！\n        人类工业化发展渐渐加速，越来越多\n的森林遭到破坏，不仅仅是鸟类，很多野\n生动物渐渐失去了它们的家园。请保护现\n有的环境，保护这些守护着一方净土的小\n精灵们！";
    }
}
