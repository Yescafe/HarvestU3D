using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCircle : MonoBehaviour
{
    private GameObject track;

    void Update()
    {
        var pos = track.transform.position;
        pos.y = BirdManager.I.selectImageHeight;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void setTrack(GameObject go)
    {
        track = go;
    }
}
