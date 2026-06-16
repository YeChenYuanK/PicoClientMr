using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTeleporter : BaseGameEntity {

	public enum TeleporterState
	{
		idle = 0,
		move = 1 
	}

	public Transform StartPoint;
	public Transform EndPoint;

	public long RunTime = 5000;
	public const long IDLE_TIME = 3000;

	public long StartTime;

	public long InitStartTime = -1;

	public TeleporterState state = TeleporterState.idle;

	private MovePathPoint nextPoint;
	public int direction = 1;
	public void Init(long serverStartTime)
	{
		InitStartTime = serverStartTime;
		RunTime = (long)(Vector3.Distance (StartPoint.position , EndPoint.position) / 2) * 1000;
	}
	// Use this for initialization
	void Start () 
	{
//		Move ();
		transform.position = StartPoint.transform.position;
		nextPoint = EndPoint.GetComponent<MovePathPoint>();
	}

	public bool GetPermission(BasePoint point)
	{
		if (state == TeleporterState.idle) 
		{
			if (direction == 0) 
			{
				return StartPoint.GetComponent<MovePathPoint> ().list.Contains(point);
			} else 
			{
				return EndPoint.GetComponent<MovePathPoint> ().list.Contains(point);
			}
		} else {
			return false;
		}
	}

	void Move()
	{
		long passTime = DateUtil.ServerTime - InitStartTime;

		if (passTime == 0) 
		{
			direction = 0;
			return;
		} else 
		{
			direction = (int)(passTime / (RunTime + IDLE_TIME)) % 2;
			if (passTime % (RunTime + IDLE_TIME) < IDLE_TIME) 
			{
				state = TeleporterState.idle;
				//停止
				return;
			} else 
			{
				state = TeleporterState.move;
				float t = (float)((passTime % (RunTime + IDLE_TIME)) - IDLE_TIME) / (float)RunTime;
				Vector3 pos;
				if (direction == 0) 
				{
					pos = Vector3.Lerp (StartPoint.position, EndPoint.position, t);
				} else 
				{
					pos = Vector3.Lerp (EndPoint.position, StartPoint.position , t);
				}
				transform.position = pos;
			}
		}
	}

	void Update () 
	{
		if(InitStartTime == -1)
		{
			return;
		}
		Move();
	}
}
