using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FocusAndroidUtil
{
    public static AndroidJavaObject mainActivity;
    private static void Init()
    {
        /*
        AndroidJavaClass unityClass = new AndroidJavaClass("com.htc.vr.unity.WVRUnityVRActivity");
        Debug.Log("???activity class : " + unityClass);
        mainActivity = unityClass.CallStatic<AndroidJavaObject>("getInstance");
        Debug.Log("???activity instance : " + mainActivity);
        */
        if (mainActivity == null)
        {
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            mainActivity = activity;
        }
    }

    //public static void StartGameByPackageName(string packageName, AppStartParam appStartParam)
    //{
    //    try
    //    {
    //        if (mainActivity == null)
    //        {
    //            Init();
    //        }
    //        DebugLog.Log("call android app start " + packageName + " " + appStartParam);
    //        AndroidJavaClass unityClass = new AndroidJavaClass("com.lekegame.androidutil.AppHelper");
    //        Debug.Log("??? app helper class : " + unityClass);
    //        unityClass.CallStatic("startApp", mainActivity, packageName, JsonConvert.SerializeObject(appStartParam));
    //        Debug.Log("app helper start activity : " + packageName);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError(ex);
    //        DebugLog.Log("?????????? ???:" + ex.Message);
    //    }
    //}

    public static void StartLauncher()
    {
        try
        {
            if (mainActivity == null)
            {
                Init();
            }
            AndroidJavaClass unityClass = new AndroidJavaClass("com.lekegame.androidutil.AppHelper");
            Debug.Log("??? app helper class : " + unityClass);
            unityClass.CallStatic("startApp", mainActivity, "com.LekeGame.LekePicoLauncher", "");
            Debug.Log("app helper start activity : com.LekeGame.FocusLauncher");
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            DebugLog.Log("?????????? ???:" + ex.Message);
        }
    }

}
