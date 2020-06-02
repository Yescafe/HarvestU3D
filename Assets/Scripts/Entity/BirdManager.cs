using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : EntityManager<Bird, BirdManager>
{
    [SerializeField] Material birdSelectedMaterial;
    [SerializeField] Material treeSelectedMaterial;
    [SerializeField] Material lumbererSelectedMaterial;
    [SerializeField] Material unSelectedMaterial;

    private Material[] treeUnSelectedMaterials;
    private Material[] lumbererUnSelectedMaterials;

    public int spawnCount = 10;
    public float spawnHeight = 2f;

    public float holdDownToRectSelect = 0.2f;

    // List<Bird> birds = new List<Bird>();
    List<Bird> selectedBirds = new List<Bird>();

    private float mouseDownTime = 0f;
    private bool drawRectangle = false;
    private Vector3 rectStart, rectEnd;

    /// <summary>
    /// 当前鼠标悬浮所指的物体，假设同时不可能指向两个
    /// </summary>
    private GameObject curHover;

    public bool Selecting => selectedBirds.Count > 0;

    void Start()
    {
        SpawnBird();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownTime = Time.time;
            rectStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            drawRectangle = Time.time - mouseDownTime > holdDownToRectSelect;
            if (drawRectangle)
            {
                CameraRectDraw.I.drawRectangle = true;
                CameraRectDraw.I.rectStart = rectStart;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - mouseDownTime < holdDownToRectSelect)
            {
                ClickSelect();
            }
            else
            {
                RectSelect();
            }
        }

        if (Selecting)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 50f, LayerMask.GetMask("Tree", "Lumberer")))
            {
                HoverObject(raycastHit.collider.gameObject);
            }
            else
            {
                UnHoverObject();
            }
        }
    }

    void HoverObject(GameObject go)
    {
        UnHoverObject();
        curHover = go;
        var mesh = curHover.GetComponentInChildren<MeshRenderer>();
        if (go.CompareTag("Tree"))
        {
            treeUnSelectedMaterials = mesh.materials;
            mesh.materials = new Material[] {treeSelectedMaterial, treeSelectedMaterial};
        }
        else
        {
            lumbererUnSelectedMaterials = mesh.materials;
            mesh.material = lumbererSelectedMaterial;
        }
    }

    void UnHoverObject()
    {
        if (curHover != null)
        {
            Debug.Log("Unhover");
            var mesh = curHover.GetComponentInChildren<MeshRenderer>();
            if (curHover.CompareTag("Tree"))
            {
                Debug.Log("交换回来了树的 material");
                mesh.materials = treeUnSelectedMaterials;
            }
            else
            {
                mesh.materials = lumbererUnSelectedMaterials;
            }
            curHover = null;
        }
    }


    void ClickSelect()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        bool selectAction = false;
        if (Physics.Raycast(ray, out raycastHit, 50f, LayerMask.GetMask("Bird", "Tree", "Lumberer")))
        {
            Debug.Log($"Hit a object {raycastHit.transform.name}");
            var hitCol = raycastHit.collider;
            switch (hitCol.tag)
            {
                case "Bird":
                    {
                        Debug.Log($"Selected a bird, {hitCol.name}");
                        DoSelectBird(hitCol.gameObject);
                        selectAction = true;
                        break;
                    }
                case "Tree":
                case "Lumberer":
                    Debug.Log($"Set target to {hitCol.name} at {raycastHit.point}");
                    foreach (var bird in selectedBirds)
                    {
                        bird.SetTarget(raycastHit.point);
                    }
                    break;
                default:
                    break;
            }
            Debug.DrawRay(ray.origin, raycastHit.point);
        }

        if (!selectAction)
        {
            UnSelectAll();
        }
    }

    void RectSelect()
    {
        CameraRectDraw.I.drawRectangle = drawRectangle = false;
        rectEnd = CameraRectDraw.I.rectEnd;
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        if (rectStart.x > rectEnd.x)
        {//这些判断是用来确保p1的xy坐标小于p2的xy坐标，因为画的框不见得就是左下到右上这个方向的
            p1.x = rectEnd.x;
            p2.x = rectStart.x;
        }
        else
        {
            p1.x = rectStart.x;
            p2.x = rectEnd.x;
        }

        if (rectStart.y > rectEnd.y)
        {
            p1.y = rectEnd.y;
            p2.y = rectStart.y;
        }
        else
        {
            p1.y = rectStart.y;
            p2.y = rectEnd.y;
        }

        foreach (var obj in entitys)
        {
            Vector3 location = Camera.main.WorldToScreenPoint(obj.transform.position);//把对象的position转换成屏幕坐标
            if (location.x < p1.x || location.x > p2.x || location.y < p1.y || location.y > p2.y
            || location.z < Camera.main.nearClipPlane || location.z > Camera.main.farClipPlane)//z方向就用摄像机的设定值，看不见的也不需要选择了
            {
                UnSelectBird(obj);
            }
            else
            {
                SelectBird(obj);
            }
        }
    }



    #region Selection

    void DoSelectBird(GameObject go)
    {
        var bird = go.GetComponent<Bird>();
        if (selectedBirds.Contains(bird))
        {
            UnSelectBird(bird);
        }
        else
        {
            SelectBird(bird);
        }
    }

    void SelectBird(GameObject go)
    {
        go.GetComponent<MeshRenderer>().material = birdSelectedMaterial;
        selectedBirds.Add(go.GetComponent<Bird>());
    }
    void SelectBird(Bird bird)
    {
        bird.GetComponent<MeshRenderer>().material = birdSelectedMaterial;
        selectedBirds.Add(bird);
    }

    void UnSelectBird(Bird bird)
    {
        bird.GetComponent<MeshRenderer>().material = unSelectedMaterial;
        selectedBirds.Remove(bird);
    }
    void UnSelectAll()
    {
        foreach (var bird in selectedBirds)
        {
            bird.GetComponent<MeshRenderer>().material = unSelectedMaterial;
        }
        selectedBirds.Clear();
    }

    #endregion

    void SpawnBird()
    {
        while (entitys.Count < spawnCount)
        {
            CreateEntity(Helper.RandomOnCircle(new Vector3(0, spawnHeight, 0), 2f));
        }
    }
}
