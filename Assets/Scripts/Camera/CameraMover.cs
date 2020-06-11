using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个单纯用于 鼠标右键拖拽视角 鼠标滚轮用于放大缩小视角 的相机脚本
/// </summary>
public class CameraMover : MonoBehaviour
{
    [SerializeField] int minSize = 2, maxSize = 10;
    [SerializeField] float mouseMoveSpeed = 0.5f;
    [SerializeField] float mouseMoveAreaPercent = 0.2f;
    [SerializeField] bool enableMouseZoom = false;

    private Camera thisCamera;

    void Start()
    {
        thisCamera = Camera.main;
        Debug.Assert(minSize <= maxSize);
    }

    void Update()
    {
        // 鼠标放置在边缘时移动
        float widthArea = Screen.width * mouseMoveAreaPercent / 2;
        float heightArea = Screen.height * mouseMoveAreaPercent / 2;
        var mPos = Input.mousePosition;

        if (mPos.x < widthArea || mPos.x > Screen.width - widthArea ||
            mPos.y < heightArea || mPos.y > Screen.height - heightArea)
        {
            var mouseMoveDir = (new Vector3(mPos.x - Screen.width / 2, 0, mPos.y - Screen.height / 2)).normalized;
            var thisCameraNextPos = thisCamera.transform.position + mouseMoveDir * mouseMoveSpeed * thisCamera.orthographicSize;
            var thisCameraHeight = thisCamera.transform.position.y;
            var deltaZSinceHeight = thisCameraHeight / Mathf.Tan(thisCamera.transform.eulerAngles.x * (float) Math.PI / 180);
            var limitWidth = Trees.I.radius + 10f;
            if (thisCameraNextPos.x > -limitWidth / 2 && thisCameraNextPos.x < limitWidth / 2 &&
                thisCameraNextPos.z > -limitWidth / 2 - deltaZSinceHeight && thisCameraNextPos.z < limitWidth / 2 - deltaZSinceHeight)
                thisCamera.transform.position = thisCameraNextPos;
        }

        // 在鼠标右键按下并拖拽的时候，模拟相机的拉取移动
        // if (Input.GetMouseButton(1))
        // {
        //     thisCamera.transform.position -= new Vector3(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y")) * mouseMoveSpeed * thisCamera.orthographicSize; ;
        // }

        if (enableMouseZoom)
        {
            thisCamera.orthographicSize -= Input.mouseScrollDelta.y;
            thisCamera.orthographicSize = Mathf.Clamp(thisCamera.orthographicSize, minSize, maxSize);
        }
    }
}
