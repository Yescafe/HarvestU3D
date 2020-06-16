using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LumbererEscape : MonoBehaviour
{
    private NavMeshAgent agent;
    private float spendTime;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spendTime = 0f;
    }

    void Update()
    {
        if (this.transform.position == agent.destination)
        {
            Destroy(gameObject);
        }
        else
        {
            this.GetComponent<Animator>().SetFloat("velocity", agent.velocity.magnitude);
        }
    }
}
