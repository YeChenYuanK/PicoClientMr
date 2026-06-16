using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleContext : MonoBehaviour {

    private static BattleContext instance;

    public static BattleContext Instance
    {
        get
        {
            return instance;
        }
    }
    /// <summary>
    /// 所有的战场地图
    /// </summary>
    public List<BattleField> battleFields;
    /// <summary>
    /// 当前游戏进行使用的战场地图
    /// </summary>
    public BattleField CurBattleField;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        
    }

    public List<BirthPoint> GetBirthPoint(int camp)
    {
        return CurBattleField.GetBirthPoint(camp);
    }

    public Transform FindNearRebirthPoinntByCamp(Transform player,int camp)
    {
        List<BirthPoint> points = CurBattleField.GetBirthPoint(camp);
        Transform tempPoint = null;
        for (int i = 0; i < points.Count; i++)
        {
            if (tempPoint == null)
            {
                tempPoint = points[i].transform;
            } else if(Vector3.Distance(player.position,tempPoint.position) > Vector3.Distance(player.position, points[i].transform.position))
            {
                tempPoint = points[i].transform;
            }
        }
        return tempPoint;
    }
    
    void Update () {
		
	}
}
