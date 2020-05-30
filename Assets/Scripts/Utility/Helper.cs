using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

static public class Helper
{
    public static void TraverseChild(Transform parent, Action<Transform> action)
    {
        // http://answers.unity.com/answers/1282989/view.html
        parent.Cast<Transform>().ToList().ForEach(action);
    }
    public static void ClearAllChild(Transform parent)
    {
        TraverseChild(parent, (trans) => GameObject.DestroyImmediate(trans.gameObject));
    }

    /// <summary>
    /// 以一个平行于 xz 平面的圆，获得在其周边上的随机点
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    public static Vector3 RandomOnCircle(Vector3 origin, float radius)
    {
        Debug.Assert(radius > 0f);
        var angle = UnityEngine.Random.Range(0, 360f);
        return origin + radius * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
    }

    public static Vector3 OmitY(this Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }

    public static void Log(object message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
}