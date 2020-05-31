using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bird : MonoBehaviour, IEntity
{
    // 转向速度，单位是 度每秒
    public float rotSpeed = 1f;
    public float maxRotZ = 30f;
    public float rotZScale = 10f;
    public float rotXScale = 10f;
    public float watchDist = 5f;
    public float attackDist = 1f;
    public int atk = 2;
    public float atkRange = 1f;

    public float moveVel = 10f;
    public float moveBackVel = -10f;

    private Vector3 target;
    private Lumberer toChase = null;
    private float curVel;


    public bool Chasing => toChase != null;
    public bool idling => curState == Idle;

    private Action curState;

    void Start()
    {
        curState = Idle;
        curVel = moveVel;
    }

    void Update()
    {
        ScreenLogger.I.Clear();
        // if (Input.GetMouseButtonDown(0))
        // {
        //     SetTargetOnPlane();
        // }
        UpdateTarget();
        curState();
    }

    public void SetTarget(Vector3 target_)
    {
        target = target_;
        if (idling) {
            transform.rotation = Quaternion.LookRotation(target.OmitY() - transform.position.OmitY());
        }
        curState = Aim2Target;
    }

    void Idle()
    {
    }

    void UpdateTarget()
    {
        if (Chasing)
        {
            target = toChase.transform.position + Vector3.up * toChase.transform.localScale.y;
        }
        else
        {
            var lumb = LumbererManager.I.GetClosest(transform.position);
            if (lumb == null)
            {
                return;
            }
            var deltaPos = lumb.transform.position.OmitY() - transform.position.OmitY();
            if (deltaPos.magnitude < watchDist)
            {
                toChase = lumb;
                curState = Aim2Target;
            }
        }
    }

    void Aim2Target()
    {
        var rot = transform.rotation.eulerAngles;

        var deltaPos = target - transform.position;
        var distance = deltaPos.OmitY().magnitude;

        if (distance < 0.1f)
        {
            curState = Idle;
            return;
        }

        Vector3 local2TargetDir = (transform.worldToLocalMatrix * deltaPos).normalized;

        if (Mathf.Abs(local2TargetDir.x) < 0.2f && distance < attackDist)
        {
            curState = ChaseToAttack;
            Debug.Log($"Prepare to attack {local2TargetDir}");
            return;
        }

        float deltaRotY = 0f;
        float targetRotZ = 0f;
        float targetRotX = 0f;

        // 侧旋转（y轴 影响移动方向，z轴 展示倾斜效果）
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
            targetRotX = -Mathf.Asin(dy / dist);
        }

        rot.y += deltaRotY;
        if (Mathf.Abs(Mathf.DeltaAngle(rot.z, targetRotZ)) > 0.1f)
            rot.z = Mathf.LerpAngle(rot.z, targetRotZ, Time.deltaTime * rotZScale);
        if (Mathf.Abs(Mathf.DeltaAngle(rot.x, targetRotX)) > 0.1f)
            rot.x = Mathf.LerpAngle(rot.x, targetRotX, Time.deltaTime * rotXScale);

        ScreenLogger.I.Add($"RotZ {rot.z}\n RotY {rot.y}\n deltaRotY {deltaRotY}\n {local2TargetDir}\n\n");
        transform.rotation = Quaternion.Euler(rot);

        MoveForward();
    }

    void ChaseToAttack()
    {
        if (toChase.IsDead)
        {
            Helper.Log("ToChase lumberer is dead");
            var tree = Trees.I.GetClosestTree(transform.position);
            target = tree.transform.position;
            curState = Aim2Target;
        }
        
        // 直接锁定目标
        transform.rotation = Quaternion.LookRotation(target - transform.position);
        var deltaPos = target - transform.position;
        var distance = deltaPos.magnitude;
        
        curVel = Mathf.Lerp(curVel, moveVel, Time.deltaTime);

        ScreenLogger.I.AddLine($"target {target}, deltaPos {deltaPos}, distance {distance}");

        if (distance < atkRange)
        {
            Helper.Log($"attack");
            toChase.TakeDamage(atk, gameObject);
            curVel = moveBackVel;
        }
        MoveForward();
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
    
    void OnTriggerEnter(Collider collider)
    {
        if (!Chasing && collider.CompareTag("Lumberer"))
        {
            Debug.Log($"Chase Target {collider.name}");
            toChase = collider.GetComponent<Lumberer>();
            curState = Aim2Target;
        }
    }


    void MoveForward()
    {
        Vector3 deltaMove = transform.localToWorldMatrix * Vector3.forward * Time.deltaTime * curVel;
        transform.position = transform.position + deltaMove;
        // transform.Translate(deltaMove);
    }
}