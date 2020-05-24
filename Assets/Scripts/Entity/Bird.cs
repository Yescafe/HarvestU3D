using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // 转向速度，单位是 度每秒
    public float rotSpeed = 1f;
    public float maxRotZ = 30f;
    public float rotZScale = 10f;

    public float moveSpeed = 10f;

    // TODO 删除 cc
    private CharacterController cc;
    private Vector3 target;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    public void SetTarget(Vector3 target_) => target = target_;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetOnPlane();
        }
        // TraceTarget();
        HandleByUserInput();

        Vector3 deltaPos = transform.localToWorldMatrix * Vector3.forward * Time.deltaTime * moveSpeed;
        transform.Translate(deltaPos);
        // cc.Move(deltaPos);
    }

    void TraceTarget()
    {
        var oldRot = transform.rotation;
        var rotZ = oldRot.z;

        Vector3 local2TargetDir = (transform.worldToLocalMatrix * (target - transform.position)).normalized;
        Debug.Log(local2TargetDir);
        float deltaRotY = 0f;
        float targetRotZ = 0f;
        if (local2TargetDir.x > 0f)
        {
            deltaRotY = Time.deltaTime * rotSpeed;
            targetRotZ = maxRotZ;
        }
        else if (local2TargetDir.x > 0f)
        {
            deltaRotY = -Time.deltaTime * rotSpeed;
            targetRotZ = -maxRotZ;
        }
        rotZ = Mathf.LerpAngle(rotZ, targetRotZ, Time.deltaTime * rotZScale);

        transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y + deltaRotY, rotZ);
    }

    /// <summary>
    /// 通过鼠标点击发出的射线，和一个具有碰撞体的物体的交点，来设置 targetpos
    /// </summary>
    void SetTargetOnPlane()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 50f))
        {
            SetTarget(raycastHit.point);
            Debug.DrawRay(ray.origin, raycastHit.point);
        }
    }

    void HandleByUserInput()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        var oldRot = transform.rotation;
        float deltaRotY = input.x * Time.deltaTime * rotSpeed;
        float targetRotZ = maxRotZ * Mathf.Sign(input.x);
        float rotZ = Mathf.LerpAngle(oldRot.z, targetRotZ, Time.deltaTime * rotZScale);

        // 最后这个 Input.x 有回弹的痕迹，如果本身是负数的话，松开按钮会突然成为整数
        // 为了避免抖动，避免使用 直接的映射计算然后赋值
        transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y + deltaRotY, rotZ);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(target, 1f);
    }
}