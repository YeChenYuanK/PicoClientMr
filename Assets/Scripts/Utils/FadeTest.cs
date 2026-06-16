using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTest : MonoBehaviour
{

    [ContextMenu("TestFadeOut")]
    public void TestFadeOut()
    {
        Debug.Log("Enter Fade Out");
        if(VRScreenFade.instance != null)
        {
            VRScreenFade.instance.FadeOut();
        }
    }

    [ContextMenu("TestFadeIn")]
    public void TestFadeIn()
    {
        if (VRScreenFade.instance != null)
        {
            VRScreenFade.instance.FadeIn();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
