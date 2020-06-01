using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LumbererEscape : MonoBehaviour
{
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
