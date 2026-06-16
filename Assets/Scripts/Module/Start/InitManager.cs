using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class InitManager : MonoBehaviour {

    public bool IsControlCenter = false;
    private Dictionary<string, string> cmdDict = new Dictionary<string, string>();
    private static InitManager instance;
    public static InitManager Instance
    {
        get
        {
            return instance;
        }
    }

    public bool IsControl;
    public GameObject controlObj;
#if OCULUS
    public OVROverlay cubemapOverlay;
    public OVROverlay loadingTextQuadOverlay;
#endif
    private void Awake()
    {
        instance = this;
#if OCULUS
        cubemapOverlay.enabled = false;
        loadingTextQuadOverlay.enabled = false;
#endif
        LogHelper.InitLogger();


        Debug.Log("Init Manager Awake");
    }
    
    void Start ()
    {
        Debug.Log("Init Manager Start");
      
        // ConfigManager.Instance.InitDB();

        if (IsControl)
        {
            controlObj.gameObject.SetActive(true);
            XRSettings.enabled = false;
        }
        else
        {
            controlObj.gameObject.SetActive(false);
            XRSettings.enabled = true;
        }

        DontDestroyOnLoad(this.gameObject);
        Debug.Log("Init Manager InitController");
        InitControl();
#if OCULUS
        cubemapOverlay.enabled = true;
        loadingTextQuadOverlay.enabled = true;
        Camera mainCamera = Camera.main;
        Transform camTransform = mainCamera.transform;
        Transform uiTextOverlayTrasnform = loadingTextQuadOverlay.transform;
        Vector3 newPos = camTransform.position + camTransform.forward * 3;
        newPos.y = camTransform.position.y;
        uiTextOverlayTrasnform.position = newPos;
        uiTextOverlayTrasnform.LookAt(camTransform);
        uiTextOverlayTrasnform.Rotate(new Vector3(0, 180, 0));
#endif
    }

    public void DisableOverlay()
    {
#if OCULUS
        loadingTextQuadOverlay.enabled = false;
        cubemapOverlay.enabled = false;
#endif
    }


    private void InitControl()
    {
       
       
    }

    private void OnApplicationQuit()
    {
        Debug.Log("[EXIT-DIAG] InitManager.OnApplicationQuit FIRED (real quit, process is terminating)");
        Debug.Log("ControlClient DisConnect");

        // ControlCenter.Instance.StopServer();
        FocusAndroidUtil.StartLauncher();
    }

}
