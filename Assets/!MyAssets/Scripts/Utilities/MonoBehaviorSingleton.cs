using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviorSingleton<T>
{
    private static T _instance;

    public static T Instance {  get { return _instance; } }

    protected virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = (T)this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
}