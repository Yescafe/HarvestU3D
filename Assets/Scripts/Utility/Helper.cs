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
}