using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class OculusInputManager : MonoBehaviour
{
    public EventHandle rightHandle = new EventHandle();



  

    // Update is called once per frame
    void Update()
    {
        this.CheckOculusKey(rightHandle);
    }

    private float lastIndexTrigger = 0;
    private float lastMouseTrigger = 0;
    private void CheckOculusKey(EventHandle handle)
    {
        /*
        float indexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (indexTrigger != 0)
        {
            Debug.Log("=================== right hand index trigger");
            // 右手扳机键
            handle.ExecuteEvent(handle.TriggerUpdate);
            if(lastIndexTrigger < 0.1f && indexTrigger >= 0.1f)
            {
                handle.ExecuteEvent(handle.TriggerPress);
            } 
        }
        
        lastIndexTrigger = indexTrigger;
        */
        if(InputBridge.Instance != null)
        {
            float indexTrigger = InputBridge.Instance.RightTrigger;
            if (indexTrigger != 0)
            {
                // 右手扳机键
                handle.ExecuteEvent(handle.TriggerUpdate);
                if (lastIndexTrigger < 0.1f && indexTrigger >= 0.1f)
                {
                    handle.ExecuteEvent(handle.TriggerPress);
                }
            }

            lastIndexTrigger = indexTrigger;
        }
        if(UnityEngine.InputSystem.Mouse.current != null)
        {
            float ismousetrigger = UnityEngine.InputSystem.Mouse.current.leftButton.ReadValue();
            if (ismousetrigger > 0)
            {
                // 右手扳机键
                handle.ExecuteEvent(handle.TriggerUpdate);
                if (lastMouseTrigger == 0 && ismousetrigger > 0)
                {
                    handle.ExecuteEvent(handle.TriggerPress);
                }
            }
            lastMouseTrigger = ismousetrigger;
        }

        if(UnityEngine.InputSystem.Keyboard.current != null)
        {
            float iskpress = UnityEngine.InputSystem.Keyboard.current.kKey.isPressed ? 1 : 0;
            if (iskpress > 0)
            {
                // 右手扳机键
                handle.ExecuteEvent(handle.TriggerUpdate);
                if (lastMouseTrigger == 0 && iskpress > 0)
                {
                    handle.ExecuteEvent(handle.TriggerPress);
                }
            }
            lastMouseTrigger = iskpress;
        }

    }

   
}
