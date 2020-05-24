using UnityEngine;
using UnityEngine.Events;

public class Environment : MonoBehaviour
{
    public UnityEvent generateEvent;

    [ContextMenu("Genreate")]
    public void Generate()
    {
        Debug.Log("Generating");
        Debug.Log($"{generateEvent.GetPersistentEventCount()}, {generateEvent.GetPersistentMethodName(0)}");
        generateEvent.Invoke();
    }
}