using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
{
    if (instance != null && instance != this)
    {
        Destroy(gameObject);
    }
    else
    {
        instance = (T)this;

        // Если объект не в корне — перемещаем в корень
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);
    }
}

}
