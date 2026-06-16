using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.gamestudio.cs;

public class TeleportLazer : MonoBehaviour {

	public LineRenderer line;
	public TextMesh cdTimeTxt;
	List<BasePoint> list = new List<BasePoint>();
	public Transform root;
	public Color normalColor;
	public Color cdColor;
	public static long CoolDate = 2000;
	public long RecevieTime = -1;
	private BasePoint currPoint;

	public BasePoint CurrPoint
	{
		set 
		{
			currPoint = value;
		}
		get 
		{
			return currPoint;
		}
	}

	// Use this for initialization
	void Start () {
        return;
		Transform pointRoot = GameObject.Find ("PointRoot").transform;
		Transform child;
		BasePoint basePoint;
		for(int i = 0 ; i < pointRoot.childCount ; i++)
		{
			child = pointRoot.GetChild (i);
			basePoint = child.GetComponent<BasePoint> ();
			if(basePoint != null && (basePoint.type == TeleporterType.FREE || basePoint.type == TeleporterType.SCORE || basePoint.type == TeleporterType.AUTO_MOVE))
			{
				list.Add (basePoint);
			}
		}
	}

	int index = 0;
	public BasePoint lastPoint;

	void Update () 
	{
		
		//检测所有的战斗点，找到最近的台子
		index++;
		if(index % 5 == 0)
		{
			float minAngle = 360;
			float tempAngle = 0;
			BasePoint tempPoint = null;
			foreach(BasePoint point in list)
			{
				tempAngle = angle (Vector3.forward , transform.InverseTransformPoint(point.telePorter.position));
				if(tempAngle < minAngle && tempAngle < 45 && CurrPoint != point && point.IsTele(CurrPoint) && point.status == TeleporterStatus.EMPTY)
				{
					minAngle = tempAngle;
					tempPoint = point;
				}
			}

			if (tempPoint == null) 
			{
				//隐藏效果
				if (lastPoint != null) 
				{
					lastPoint.HitEffect (false);
					lastPoint = null;
				}
			}
			else
			{
				if (tempPoint != lastPoint) 
				{
					//显示效果
					if (lastPoint != null) 
					{
						lastPoint.HitEffect (false);
					}
					tempPoint.HitEffect (true);
					lastPoint = tempPoint;
				}
			}
			if (!IsCD()) {
//				line.SetColors (normalColor, normalColor);
//				line.material.color = normalColor;
				line.material.SetColor ("_TintColor" , normalColor);
				cdTimeTxt.gameObject.SetActive (false);
			} else {
				//				line.SetColors (cdColor, cdColor);
//				line.material.color = cdColor;
				line.material.SetColor ("_TintColor" , cdColor);
				cdTimeTxt.gameObject.SetActive (true);
				cdTimeTxt.text = (Mathf.Ceil(RecevieTime - DateUtil.NowMllSec)/1000.0f).ToString();
			}

			index = 0;

		}

		if (lastPoint != null) 
		{
			line.enabled = true;
			line.SetPosition (0, line.transform.position);
			line.SetPosition (1, lastPoint.telePorter.position);
		} else 
		{
			line.enabled = false;
			line.SetPositions(new Vector3[1]{Vector3.zero});
		}
	}

	public void MoveNextNode(BasePoint point )
	{

		if (point != null) 
		{
			root.SetParent (point.telePorter);
			root.position = point.telePorter.position;
			CurrPoint = point;
			lastPoint = null;
			CurrPoint.HitEffect (false);
			lastPoint = null;
//			RecevieTime = DateUtil.NowMllSec + ConfigManager.GlobalCfg.moveCd;
//			cdTimeTxt.text = (ConfigManager.GlobalCfg.moveCd/1000.0f).ToString();

		} else 
		{
			Debug.LogError("Point Is Null!");
		}
	}

	public bool IsCD()
	{
		if (RecevieTime <= DateUtil.NowMllSec) 
		{
			return false;
		} else 
		{
			return true;
		}
	}

	static float angle_360(Vector3 from_, Vector3 to_)
	{

		Vector3 v3 = Vector3.Cross(from_,to_);

		if (v3.z > 0) 
		{
			return Vector3.Angle (from_, to_);
		}
		else
		{
			return 360-Vector3.Angle(from_,to_);
		}
	}

	static float angle(Vector3 from_, Vector3 to_)
	{
		float ang = angle_360 (from_ , to_);
		if(ang > 180)
		{
			ang = 360 - ang;
		}
		return ang;
	}
}
