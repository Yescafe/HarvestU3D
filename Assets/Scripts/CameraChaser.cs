using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChaser : MonoBehaviour
{
    public Transform trackee;

    public float height;
    public float distance;
    public float deltaScale;

    void Update()
    {
        transform.position = new Vector3(trackee.position.x, 0 + height * deltaScale, trackee.position.z - distance * deltaScale);
    }
}
