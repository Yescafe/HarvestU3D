using UnityEngine;
using UnityEngine.Events;

public class Environment : MonoBehaviour
{
    /// <summary>
    /// 注意，如果要在 Editor 环境下调用的时候，
    /// 在 Inspector 窗口的每个 listener，左上角选项选择，Editor and Runtime
    /// </summary>
    public UnityEvent generateEvent;

    [ContextMenu("Genreate")]
    public void Generate()
    {
        generateEvent.Invoke();
    }
}