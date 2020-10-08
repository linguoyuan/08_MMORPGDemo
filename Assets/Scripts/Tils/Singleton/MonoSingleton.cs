using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _single;

    public static T Single
    {
        get
        {
            if (_single == null)
            {
                //GameObject go = new GameObject(typeof(T).Name);
                //DontDestroyOnLoad(go);
                //_single = go.AddComponent<T>();

                //适合这个项目的单例类
                //GameObject go = GameObject.Find("GameRoot");
                //_single = go.AddComponent<T>();
                GameObject go = GameObject.Find("GameRoot");
                _single = go.GetComponent<T>();
            }

            return _single;
        }
    }
}
