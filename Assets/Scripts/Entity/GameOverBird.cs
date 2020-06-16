using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBird : DCLSingletonBase<GameOverBird>
{
    void start()
    {
        GetComponent<Animator>().SetBool("flying", true);
    }
}
