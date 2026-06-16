using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using System.Collections.Generic;

public class TeleporterManager : MonoBehaviour 
{

    private static TeleporterManager instance;

    public static TeleporterManager Instance
    {
        get { return instance; }
    }

    public TeleporterManager()
    {
        instance = this;
    }

    // 地图编号
    public int levelId;

    private BasePoint[] basePoints;

    void Awake()
    {
        // 收集所有的BasePoint
        basePoints = this.transform.GetComponentsInChildren<BasePoint>();

    }

	public BasePoint[] BasePoints
	{
		get {
			return basePoints;
		}
	}

    public BasePoint FindBasePoint(int birthIndex)
    {
        foreach(BasePoint point in this.basePoints)
        {
            if(point.index == birthIndex)
            {
                return point;
            }
        }
        return null;
    }

	public List<BasePoint> GetMovePoints()
	{
		List<BasePoint> list = new List<BasePoint> ();
		foreach(BasePoint point in this.basePoints)
		{
			if(point.type == TeleporterType.AUTO_MOVE)
			{
				list.Add (point);
			}
		}
		return list;
	}

    // Use this for initialization
    void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
