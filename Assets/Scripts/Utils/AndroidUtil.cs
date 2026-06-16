using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidUtil
{
    private static AndroidJavaObject javaClass;

    public static AndroidJavaObject GetActivityObject()
    {
        if (javaClass == null)
        {
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            // javaClass = new AndroidJavaObject("com.unity3d.player.UnityPlayerActivityV2");
            javaClass = activity;
        }
        return javaClass;
    }

    public static void StartLauncher()
    {
        try
        {
            Debug.Log("call StartLauncher");
            var j = GetActivityObject();
            j.Call("startLauncher");
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            Debug.Log("开启游戏失败 原因:" + ex.Message);
        }
    }


}
