using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour, IEntity
{
    // 转向速度，单位是 度每秒
    public float rotSpeed = 1f;
    public float maxRotZ = 30f;
    public float rotZScale = 10f;
    public float rotXScale = 10f;
    public float attackDist = 1f;

    public float moveSpeed = 10f;

    private Vector3 target;
    private string logInfo;
    private Lumberer toChase = null;

    public void SetTarget(Vector3 target_) => target = target_;
    public bool Chasing => toChase != null;

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     SetTargetOnPlane();
        // }
        UpdateTarget();
        Aim2Target();

        Vector3 deltaPos = transform.localToWorldMatrix * Vector3.forward * Time.deltaTime * moveSpeed;
        transform.position = transform.position + deltaPos;
        // transform.Translate(deltaPos);
    }

    void UpdateTarget()
    {
        if (Chasing)
        {
            target = toChase.transform.position;
        }
    }

    void Aim2Target()
    {
        var rot = transform.rotation.eulerAngles;

        Vector3 local2TargetDir = (transform.worldToLocalMatrix * (target - transform.position)).normalized;

        float deltaRotY = 0f;
        float targetRotZ = 0f;
        float targetRotX = 0f;
        float dist = Vector3.Distance(target, transform.position);
        if (local2TargetDir.x > 0.1f)
        {
            deltaRotY = Time.deltaTime * rotSpeed;
            targetRotZ = -maxRotZ;
        }
        else if (local2TargetDir.x < -0.1f)
        {
            deltaRotY = -Time.deltaTime * rotSpeed;
            targetRotZ = maxRotZ;
        }
        // 瞄准对方了，尝试进行攻击
        else if (Chasing && dist < attackDist)
        {
            float dy = Mathf.Abs(target.y - transform.position.y);
            targetRotX = -Mathf.Asin(dy/dist);
        }

        rot.y += deltaRotY;
        if (Mathf.Abs(Mathf.DeltaAngle(rot.z, targetRotZ)) > 0.1f)
            rot.z = Mathf.LerpAngle(rot.z, targetRotZ, Time.deltaTime * rotZScale);
        if (Mathf.Abs(Mathf.DeltaAngle(rot.x, targetRotX)) > 0.1f)
            rot.x = Mathf.LerpAngle(rot.x, targetRotX, Time.deltaTime * rotXScale);

        logInfo = $"RotZ {rot.z}\n RotY {rot.y}\n deltaRotY {deltaRotY}\n {local2TargetDir}";
        transform.rotation = Quaternion.Euler(rot);
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

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(target, 1f);
    }

    public void TakeDamage(float damage, GameObject attaker)
    {

    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 80), logInfo);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!Chasing && collider.CompareTag("Lumberer"))
        {
            Debug.Log($"Chase Target {collider.name}");
            toChase = collider.GetComponent<Lumberer>();
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        
    }
}