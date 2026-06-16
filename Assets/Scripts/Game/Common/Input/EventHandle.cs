using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventHandle 
{

	public delegate void InputDelegate(object param);

	//TouchPad的上下左右中心区域的,按下，抬起以及全局点击的回调
	public List<InputDelegate> TouchPadDownPress = new List<InputDelegate>();
	public List<InputDelegate> TouchPadDownLoss = new List<InputDelegate>();
	public List<InputDelegate> TouchPadUpPress = new List<InputDelegate>();
	public List<InputDelegate> TouchPadUpLoss = new List<InputDelegate>();
	public List<InputDelegate> TouchPadLeftPress = new List<InputDelegate>();
	public List<InputDelegate> TouchPadLeftLoss = new List<InputDelegate>();
	public List<InputDelegate> TouchPadRightPress = new List<InputDelegate>();
	public List<InputDelegate> TouchPadRightLoss = new List<InputDelegate>();
	public List<InputDelegate> TouchPadPress = new List<InputDelegate>();
	public List<InputDelegate> TouchPadLoss = new List<InputDelegate>();
	public List<InputDelegate> TouchPadClick = new List<InputDelegate>();

    public float lastTriggerAxis = 0;
    //Trigger回调
    public List<InputDelegate> Trigger = new List<InputDelegate>();
	public List<InputDelegate> TriggerPress = new List<InputDelegate>();
	public List<InputDelegate> TriggerLoss = new List<InputDelegate>();
	public List<InputDelegate> TriggerUpdate = new List<InputDelegate> ();
    public List<InputDelegate> Kreload = new List<InputDelegate>();

    public List<InputDelegate> GripPress = new List<InputDelegate> ();
	public List<InputDelegate> GripLoss = new List<InputDelegate> ();

	public InputDelegate DriveDelegate;

	public InputDelegate FireDelegate;


	public List<InputDelegate> TeleportPress = new List<InputDelegate> ();
	public List<InputDelegate> TeleportLoss = new List<InputDelegate> ();

	public List<InputDelegate> KeyCode_C = new List<InputDelegate> ();

	public List<InputDelegate> KeyCode_K = new List<InputDelegate> ();
	public List<InputDelegate> KeyCode_R = new List<InputDelegate> ();
    public List<InputDelegate> KeyCode_T = new List<InputDelegate>();

    public void ExecuteEvent (List<InputDelegate> eventList)
	{
		ExecuteEvent (eventList , null);
	}

	public void ExecuteEvent(List<InputDelegate> eventList , params object[] obj)
	{
		if(eventList != null)
		{
			foreach(InputDelegate hand in eventList)
			{
				hand (obj);
			}
		}
	}
}
