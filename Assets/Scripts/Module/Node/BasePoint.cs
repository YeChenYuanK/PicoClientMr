using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.gamestudio.cs;
using System;

public class BasePoint : MonoBehaviour 
{
    // 点的类型
	public TeleporterType type = TeleporterType.FREE;
    // 点的索引Id
    public int index = 0;
	public int birthCamp =1;

    public TeleporterStatus status = TeleporterStatus.EMPTY;
    
	public GameObject NoneOccupyEffect;
	public GameObject GreenOccupyEffect;
	public GameObject RedOccupyEffect;
    public Transform hitEffect;
	public Transform telePorter;
	public Transform ChangeEffect;

	private int LastCamp = -1;


    public int scorePointId = 0;
    
    // 可以条转过来的索引Id集合
	public List<BasePoint> list;
	public Vector3 RangeSize = new Vector3 (2.5f, 2.5f, 2.5f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (type != TeleporterType.BIRTH && type != TeleporterType.AUTO_MOVE) 
		{
			MTeleporter mTeleporter = MapManager.Instance.GetTeleporter (index);
			if(LastCamp != -1 && LastCamp != mTeleporter.curCampIndex)
			{
				ChangeEffect.gameObject.SetActive (true);
			}
			if (mTeleporter.curCampIndex == (int)Camp.Red) {
				SetActive (NoneOccupyEffect , false);
				SetActive (RedOccupyEffect , true);
				SetActive (GreenOccupyEffect , false);

			} else if (mTeleporter.curCampIndex == (int)Camp.Blue) {
				SetActive (NoneOccupyEffect , false);
				SetActive (RedOccupyEffect , false);
				SetActive (GreenOccupyEffect , true);
			} else {
				SetActive (NoneOccupyEffect , true);
				SetActive (RedOccupyEffect , false);
				SetActive (GreenOccupyEffect , false);
			}
//			if(mTeleporter.)
//			{
//			}
			LastCamp = mTeleporter.curCampIndex;
		}
	}

	public void SetActive(GameObject obj , bool active)
	{
		if(obj.activeSelf != active)
		{
			obj.SetActive (active);
		}
	}

	public void HitEffect(bool isShow)
	{
		if (hitEffect != null) 
		{
			hitEffect.gameObject.SetActive (isShow);
        }
	}

	public bool IsTele(BasePoint point)
	{
		if(type == TeleporterType.AUTO_MOVE )
		{
			return AutoMoveExe (this , point);
		}
		//自动移动的模块比较特殊，子节点配置的List决定了双向移动权限
		if(point.type == TeleporterType.AUTO_MOVE )
		{
			return AutoMoveExe (point , this);
		}

		if ((list != null && list.Contains (point))) {
			return true;
		} else {
			return false;
		}
	}

	private bool AutoMoveExe(BasePoint basePoint , BasePoint targetPoint)
	{
		MoveTeleporter moveTele = basePoint.telePorter.parent.GetComponent<MoveTeleporter> ();
		if (moveTele.state == MoveTeleporter.TeleporterState.idle) 
		{
			if (moveTele.GetPermission (targetPoint)) 
			{
				return true;
			} else 
			{
				return false;
			}
		} else 
		{
			return false;
		}
	}

	public bool IsOcppty(int campIndex)
	{
		MTeleporter mTeleporter = MapManager.Instance.GetTeleporter (index);
		if (mTeleporter.curCampIndex == campIndex) {
			return true;
		} else {
			return false;
		}
	}

	public void Ocppty()
	{
	}

	public void OnDrawGizmos()
	{
		#if UNITY_EDITOR
		Gizmos.DrawIcon (transform.position, "Node.tif" , true);

		if(list != null)
		{
			Gizmos.DrawWireCube (transform.position, RangeSize);
			if (UnityEditor.Selection.activeGameObject == gameObject) 
			{
				foreach (BasePoint point in list) 
				{
					Gizmos.DrawLine (transform.position, point.transform.position);
				}
			}
		}
		#endif
	}

    /// <summary>
    /// 切换点的状态
    /// </summary>
    /// <param name="origin">The origin.</param>
    /// <param name="mt">The mt.</param>
    public void Change(MTeleporter origin, MTeleporter mt)
    {
        
    }
}