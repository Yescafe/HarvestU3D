using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EntityManager<EntityT, ManagerT> : DCLSingletonBase<ManagerT> 
    where EntityT : MonoBehaviour
    where ManagerT : MonoBehaviour
{
    [SerializeField] protected GameObject entityPrefab;
    protected KdTree<EntityT> entitys = new KdTree<EntityT>();

    public void ArrangeChild()
    {
        var tmps = transform.GetComponentsInChildren<EntityT>();
        foreach (var lumb in tmps)
        {
            entitys.Add(lumb);
        }
    }

    public virtual EntityT CreateEntity(Vector3 pos)
    {
        var go = Instantiate(entityPrefab, pos, Quaternion.identity, transform);
        var entity = go.GetComponent<EntityT>() ?? go.AddComponent<EntityT>();
        entitys.Add(entity);
        go.name = $"{typeof(EntityT)} {entitys.Count}";
        // Debug.Log($"Created {entity.name} at {pos}");
        return entity;
    }

    public virtual void RemoveEntity(EntityT entity)
    {
        entitys.Remove(entity);
        Destroy(entity.gameObject);
    }

    public EntityT GetClosest(Vector3 pos) => entitys.FindClosest(pos);
}