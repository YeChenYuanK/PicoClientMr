using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCompositorUtil 
{
    public static void HideLoadingLayer()
    {/*
        GameObject loadingCompositorObj = GameObject.Find("LoadingCompositor");
        if (loadingCompositorObj != null)
        {
            CompositorLayerManager.GetInstance().UnsubscribeFromLayerManager(loadingCompositorObj.GetComponent<CompositorLayer>());
        }
        */
    }

    public static void ShowLoadingLayer()
    {
        /*
        GameObject loadingCompositorObj = GameObject.Find("LoadingCompositor");
        if (loadingCompositorObj != null)
        {
            CompositorLayerManager.GetInstance().SubscribeToLayerManager(loadingCompositorObj.GetComponent<CompositorLayer>());
        }
        */
    }
}
