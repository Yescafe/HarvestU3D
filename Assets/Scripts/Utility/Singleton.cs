//-------------------------------------------------
//            OpenWorldFramework
//    Copyright © 2015-2018 SakuraGaming
//           Author:Raphael A Stuart
//-------------------------------------------------

using UnityEngine;
/// <summary>
/// 双检锁单例父类
/// </summary>
/// <typeparam name="T">单例</typeparam>
public abstract class DCLSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    protected static object @lock = new object();

    protected static bool application_is_quitting;

    /// <summary>
    /// Instance of this Singleton
    /// </summary>
    public static T I => Instance;

    /// <summary>
    /// Instance of this Singleton
    /// </summary>
    protected static T Instance
    {
        get
        {
            if (application_is_quitting)
            {
                return null;
            }
            lock (@lock)
            {
                if (null == instance)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return instance;
                    }

                    if (null == instance)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton)" + typeof(T);

                        if (Application.isPlaying)
                            DontDestroyOnLoad(singleton);
                    }
                }
                return instance;
            }
        }
    }

    public static T CreateInstance() => Instance;

    public virtual void OnDestroy()
    {
        application_is_quitting = true;
    }
}

/// <summary>
/// 无锁，无双检单例，非线程安全
/// </summary>
/// <typeparam name="T">单例类</typeparam>
public abstract class NonLockSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T I => Instance;

    protected static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singleton = new GameObject();
                instance = singleton.AddComponent<T>();
                singleton.name = "(singleton)" + typeof(T);

                if (Application.isPlaying)
                    DontDestroyOnLoad(singleton);
            }

            return instance;
        }
    }
}