using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using System.Collections.Generic;

/// <summary>
/// 地图管理器
/// </summary>
public class MapManager {

    private static MapManager instance ;

    public static MapManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new MapManager();
            }
            return instance;
        }
    }

    private Dictionary<int, MTeleporter> teleMap = new Dictionary<int, MTeleporter>();

    public void AddTeleportInfo(MTeleporter teleporter)
    {
        teleMap[teleporter.index] = teleporter;
    }

    public MTeleporter GetTeleporter(int pointIndex)
    {
        if(teleMap.ContainsKey(pointIndex))
        {
            return teleMap[pointIndex];
        }
        return null;
    }
	
}
