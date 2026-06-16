using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.SceneManagement;

public class ProcessTest : MonoBehaviour
{
    public List<string> list = new List<string>()
    {
        "PrepareScene_12X7",
        "01_12x7_Storage",
        "02_12x7_Factory",
        "03_12x7_Ruins",
        "GameOverScene_12x7"
    };


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += this.OnSceneLoaded;
    }

    void Start()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(list.IndexOf(scene.name) >= 0)
        {
            GameObject.Instantiate((GameObject)Resources.Load("Prefbs/OVR/OVRPlayerController Variant -XRRig"));
        }
    }

    private int idx = 0;
    void Update()
    {
        if(InputBridge.Instance != null)
        {
            if(InputBridge.Instance.AButtonUp || UnityEngine.InputSystem.Keyboard.current.pKey.wasReleasedThisFrame)
            {
                // 直接往后面跳转场景
                SceneLoader.Instance.StartScene(list[idx]);
                idx++;
            }
        }
    }
}
