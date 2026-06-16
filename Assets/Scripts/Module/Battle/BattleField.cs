using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField : MonoBehaviour {

    /// <summary>
    /// 战场编号
    /// </summary>
    public int BattleFieldId;

    /// <summary>
    /// 出生点集合
    /// </summary>
    public List<BirthPoint> birthPoints;

    public List<BirthPoint> GetBirthPoint(int camp)
    {
        List<BirthPoint> result = new List<BirthPoint>();
        for (int i=0;i<birthPoints.Count;i++)
        {
            if(birthPoints[i].Camp == camp)
            {
                result.Add(birthPoints[i]);
            }
        }
        return result;
    }

    public void ShowCamp(int camp)
    {
        for (int i = 0; i < birthPoints.Count; i++)
        {
            if (birthPoints[i].Camp == camp || camp == -1)
            {
                birthPoints[i].gameObject.SetActive(true);
            } else
            {
                birthPoints[i].gameObject.SetActive(false);
            }
        }
    }

    void Start () {
		
	}
	
	void Update () {
		
	}
}
