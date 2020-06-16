using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTestMaiinManager : MonoBehaviour
{
    void Start()
    {   
        for (int i = 0; i < 20; i++) {
            var pos = new Vector3(
                UnityEngine.Random.Range(-10f, 10f),
                0f,
                UnityEngine.Random.Range(-10f, 10f)
            );
            LumbererManager.I.CreateEntity(pos).TakeDamage(100f, null);
        }
    }
}
