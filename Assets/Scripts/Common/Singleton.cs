using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    // 싱글톤 패턴.
    static T instance;
    public static T Instance
    {
        get
        {
            // 오브젝트가 없으면 찾는다.
            if (instance == null)
                instance = GameObject.FindObjectOfType<T>();

            // 그래도 없으면 만든다.
            if (instance == null)
                instance = new GameObject("BlockManager").AddComponent<T>();

            return instance;
        }
    }

    protected void Awake()
    {
        instance = this as T;
    }
}