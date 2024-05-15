using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (!instance)
                {
                    Debug.LogError("There needs to be one active $typeof(T) script on a GameObject in your scene.");
                }
            }

            return instance;
        }
    }
}
