using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : DCLSingletonBase<BirdManager>
{
    [SerializeField] Material selectedMaterial;
    [SerializeField] Material unSelectedMaterial;

    List<Bird> birds = new List<Bird>();
    List<Bird> selectedBirds = new List<Bird>();
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

    }

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
        go.GetComponent<MeshRenderer>().material = selectedMaterial;
        selectedBirds.Add(go.GetComponent<Bird>());
    }
    void SelectBird(Bird bird)
    {
        bird.GetComponent<MeshRenderer>().material = selectedMaterial;
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

    void SpawnBird()
    {

    }
}
