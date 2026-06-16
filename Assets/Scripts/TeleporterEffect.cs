using UnityEngine;
using System.Collections;
using System;

public class TeleporterEffect : MonoBehaviour 
{
	private Action teleportCallback;
	private static long unitTime = 500;
	private long teleportTime;
	private long startDate;
	private long FOV = 100;

	// Use this for initialization
	void Start () 
	{
		//方案1. 按移动方向旋转相机、拉伸FOV、恢复相机位置。
	}
	
	// Update is called once per frame
	void Update () 
	{
		long rate = (startDate + teleportTime) - DateUtil.NowMllSec;
		Mathf.Lerp (FOV , 20 , Mathf.Clamp(rate , 0 , 1));
	}

	public void Teleport(Action callback,float distance)
	{
		if(!enabled)
		{
			enabled = true;
		}
		teleportTime = long.Parse((distance / 15 * 1000).ToString());
		startDate = DateUtil.NowMllSec;


	}

//	public void 
}
