using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }

            if (_instance == null)
            {
                GameObject gameObject = new GameObject();
                _instance = gameObject.AddComponent<T>();
                DontDestroyOnLoad(gameObject);
            }

            return _instance;
        }

        set
        {
            _instance = value;
        }
    }
}