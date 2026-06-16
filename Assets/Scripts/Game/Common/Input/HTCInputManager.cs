using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HTCInputManager : MonoBehaviour {

	public EventHandle leftHandle= new EventHandle ();
	public EventHandle rightHandle= new EventHandle ();
	//public SteamVR_TrackedObject leftTracked;
	//public SteamVR_TrackedObject rightTracked;
	public GunController gunController;

	public TeleportLazer lazer;
	public Transform HeadTarget;
	public Transform LeftTarget;
	public Transform RightTarget;

    //public List<SteamVR_TrackedObject> devices;
    public int activeDeviceIndex = 2;
    //public SteamVR_TrackedObject curActiveDevice;
    void Start () 
	{
        // InitDevices();
    }
    /*
    void InitDevices()
    {
        for(int i=0;i<devices.Count;i++)
        {
            if(i == (activeDeviceIndex - 1))
            {
                devices[i].gameObject.SetActive(true);
                curActiveDevice = devices[i];
            } else
            {
                devices[i].gameObject.SetActive(false);
            }
        }

        if(curActiveDevice != null)
        {
            Transform gun = this.transform.Find("[CameraRig]/Controller (right)/Gun");
            gun.SetParent(curActiveDevice.transform);
        }
    }
	*/
	// Update is called once per frame
	void Update () {
		InputCheck ();
	}

	public void ClearEvent()
	{
		
	}

	void InputCheck()
	{
        /*
		DeviceType type;
		if (leftTracked != null && ((int)leftTracked.index > 0))
		{
			//左手手柄处理
			var leftDevice = SteamVR_Controller.Input((int)leftTracked.index);
			HTCTouchPad(leftDevice , leftHandle);
			CheckTarger (leftTracked , leftHandle);
			CheckGrip (leftDevice , leftHandle);
		}
		if (rightTracked != null && ((int)rightTracked.index > 0))
		{
			var rightDevice = SteamVR_Controller.Input((int)rightTracked.index);
			//右手手柄处理
			HTCTouchPad(rightDevice , rightHandle);
			CheckTarger (rightTracked , rightHandle);
			CheckGrip (rightDevice , rightHandle);
            CheckTrackedObjectAngle(rightTracked, rightHandle);

        }
        */
	}

	long touchTime = 0;
	bool isDown = false;
	Vector2 lastPosition;
    /*
	void HTCTouchPad(SteamVR_Controller.Device device , EventHandle handle)
	{
		int moveZ = 0;
		int moveX = 0;
		int moveY = 0;
		int direction = 0;
		int crossDicY = 0;
		Vector2 touchPad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

		if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
		{
			touchTime = DateUtil.NowMllSec;
			isDown = true;
			handle.ExecuteEvent (handle.TouchPadPress, touchPad);
		}
//		if(device.GetAxis(SteamVR_Controller.ButtonMask.Touchpad))
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) 
		{
			handle.ExecuteEvent (handle.TouchPadLoss , lastPosition);
			Debug.Log ("TouchPadLoss");
			if (touchTime + 300 <= DateUtil.NowMllSec)//小于300毫秒不失为点击
			{
				handle.ExecuteEvent (handle.TouchPadClick);
			}
		}
		if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
		{
//			isDown = false;
//			handle.ExecuteEvent (handle.TouchPadLoss , touchPad);
//			Debug.Log ("TouchPadLoss");
//			if (touchTime + 300 <= DateUtil.NowMllSec)//小于300毫秒不失为点击
//			{
//				handle.ExecuteEvent (handle.TouchPadClick);
//			}
		}
		if (isDown)
		{
			if (touchPad != null && (touchPad.x != 0 || touchPad.y != 0))
			{
				lastPosition = touchPad;
				Vector2 dir = Vector2.zero - new Vector2(touchPad.x, touchPad.y);
				float angle = Vector2.Angle(dir, Vector2.left);
				if (touchPad.y < 0)
				{
					angle = 360 - angle;
				}

				//gun1
				if (angle <= 22.5f || angle > 337.5f)//右
				{
					handle.ExecuteEvent (handle.TouchPadRightPress);
				}
				else if (angle <= 67.5f && angle > 22.5f)//右上
				{
					moveX = 1;
					moveZ = 1;
				}
				else if (angle > 67.5f && angle <= 112.5f)//上
				{
					handle.ExecuteEvent (handle.TouchPadUpPress);
				}
				else if(angle > 112.5f && angle <= 157.5f)//左上
				{
				}
				else if(angle > 157.5f && angle <= 202.5f)//左
				{
					handle.ExecuteEvent (handle.TouchPadLeftPress);
				}
				else if(angle > 202.5f && angle <= 247.5)//左下
				{
					moveX = -1;
					moveZ = -1;
				}
				else if(angle > 247.5f && angle <= 292.5f)//下
				{
					handle.ExecuteEvent (handle.TouchPadUpPress);
				}
				else if(angle > 292.5f && angle <= 337.5f)//右下
				{
					moveX = 1;
					moveZ = -1;
				}
			}
		}
		
	}

    void CheckTrackedObjectAngle(SteamVR_TrackedObject trackedObj, EventHandle handle)
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);

    }
    
	void CheckTarger(SteamVR_TrackedObject trackedObj ,EventHandle handle)
	{
		SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
		Vector2 triggerVec = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);

        if(triggerVec.x == 1 && handle.lastTriggerAxis < 1)
        {
            handle.ExecuteEvent(handle.Trigger);
        }
        handle.lastTriggerAxis = triggerVec.x;
		if (triggerVec.x > 30)
		{
			SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(500);
			// handle.ExecuteEvent(handle.TriggerUpdate);
		}

		if(device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
		{
			// handle.ExecuteEvent (handle.TriggerUpdate , triggerVec);
		}
		if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))//扣动扳机
		{
			handle.ExecuteEvent (handle.TriggerPress , triggerVec);
		}
		if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) 
		{
			handle.ExecuteEvent (handle.TriggerLoss , triggerVec);
		}
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            handle.ExecuteEvent(handle.TriggerPress, triggerVec);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            handle.ExecuteEvent(handle.TriggerLoss, triggerVec);
        }
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            handle.ExecuteEvent(handle.TriggerUpdate, triggerVec);
        }
    }

	public void CheckGrip(SteamVR_Controller.Device device , EventHandle handle)
	{
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) 
		{
			handle.ExecuteEvent (handle.GripPress);
		}
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Grip)) 
		{
			handle.ExecuteEvent (handle.GripLoss);
		}
	}
    */
}
