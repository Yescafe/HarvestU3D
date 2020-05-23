using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform trackee;
    public Transform mainCamera;

    public float height;
    public float distance;
    public float deltaScale;

    void Start()
    {
        
    }

    void Update()
    {
        mainCamera.position = new Vector3(trackee.position.x, trackee.position.y + height * deltaScale, trackee.position.z - distance * deltaScale);
    }
}
