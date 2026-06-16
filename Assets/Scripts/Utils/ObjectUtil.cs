using UnityEngine;
using System;
using System.Collections;

public class ObjectUtil {
    
    public static void UpdateObjectActive(Transform obj, bool active)
    {
        if(obj != null)
        {
            UpdateObjectActive(obj.gameObject, active);
        }
    }
    public static void UpdateObjectActive(GameObject obj , bool active)
    {
        if(obj!= null && obj.activeSelf != active)
        {
            obj.SetActive(active);
        }
    }

    public static T GetComponent<T>(GameObject obj) where T : Component
    {
        T t = obj.GetComponent<T>();
        if (t != null)
        {
            return t;
        }else
        {
            t = obj.AddComponent<T>();
        }
        return t;
    }

    public static T UpdateComponent<T>(GameObject obj) where T : Component
    {
        T t = obj.GetComponent<T>();
        if (t != null)
        {
            GameObject.Destroy(t);
           
        }
        t = obj.AddComponent<T>();
        return t;
    }
}
