using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCircle : MonoBehaviour
{
    private GameObject track;

    void Update()
    {
        if (track && GetComponent<Image>().enabled)
        {
            var pos = track.transform.position;
            pos.y = BirdManager.I.selectImageHeight;
            transform.position = pos;
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }

    public void setTrack(GameObject go)
    {
        track = go;
    }
}
