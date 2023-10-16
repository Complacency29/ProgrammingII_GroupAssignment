using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();

                if (results.Length == 0)
                {
                    Debug.LogError("Error: No object could be found.");
                    return null;
                }

                if (results.Length > 1)
                {
                    Debug.LogWarning("Warning: There is more than once object.");
                }

                _instance = results[0];
            }

            return _instance;
        }
    }
}
