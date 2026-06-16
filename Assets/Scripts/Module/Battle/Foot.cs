using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour {

    /// <summary>
    /// 主角定义
    /// </summary>
    public CSPlayer player;
    /// <summary>
    /// 接触出生点的时候
    /// </summary>
    public float birthPointTime;
    /// <summary>
    /// 接触出生点的持续时间
    /// </summary>
    public float birthPointContinuedTime;
    /// <summary>
    /// 判断是否靠近出生点的距离判定
    /// </summary>
    public float justiceDistance = 0.5f;
    /// <summary>
    /// 当前接近的出生点
    /// </summary>
    public BirthPoint CheckPoint = null;

    void Start () {
		
	}
	
	void Update () {
        
    }

    public void Clear()
    {
        if(CheckPoint != null)
        {
            CheckPoint.HideProgress();
        }
        CheckPoint = null;
        birthPointTime = 0;
        birthPointContinuedTime = 0;
    }

    public void Check()
    {
      
        if (BattleContext.Instance == null) return;
        int camp = player.Camp;
        List<BirthPoint> birthPoints = BattleContext.Instance.GetBirthPoint(camp);
        
        for (int i = 0; i < birthPoints.Count; i++)
        {
            BirthPoint birthPoint = birthPoints[i];
            Vector3 pos = new Vector3(this.transform.position.x, birthPoints[i].transform.position.y, this.transform.position.z);
            float distance = Vector3.Distance(birthPoints[i].transform.position, pos);
         
            if (distance <= justiceDistance)
            {
                if (birthPoint == CheckPoint || CheckPoint == null)
                {
                    if (birthPointTime > 0)
                    {
                        birthPointContinuedTime = Time.time - birthPointTime;
                    }
                    else
                    {
                        birthPointTime = Time.time;
                        birthPointContinuedTime = 0;
                    }
                }
                else
                {
                    birthPointTime = Time.time;
                    birthPointContinuedTime = 0;
                }
                CheckPoint = birthPoint;
                
                break;
            }
            if (CheckPoint != null && Vector3.Distance(CheckPoint.transform.position, pos) > justiceDistance)
            {
                Clear();
            }
        }

    }

}
