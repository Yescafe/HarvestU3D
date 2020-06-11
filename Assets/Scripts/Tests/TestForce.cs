using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForce : MonoBehaviour
{
    public Vector3 force = new Vector3(0,0, 1);
    public ForceMode forceMode;
    
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(force, forceMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
