using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPlayerManager : MonoBehaviour {
    private static CSPlayerManager instance;
    public static CSPlayerManager Instance
    {
        get { return instance; }
    }
    public List<CSPlayer> playerList = new List<CSPlayer>();
    public Dictionary<int, CSPlayer> playerDict = new Dictionary<int, CSPlayer>();

    /// <summary>
    /// 添加玩家
    /// </summary>
    /// <param name="player"></param>
  

    public CSPlayer GetCSPlayer(int ppId)
    {
        for(int i=0;i<playerList.Count;i++)
        {
            if(playerList[i].playerId == ppId)
            {
                return playerList[i];
            }
        }
        return null;
    }

    public CSPlayer GetCSPlayerByAllocateIndex(int index)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].playerId == index)
            {
                return playerList[i];
            }
        }
        return null;
    }

    public List<CSPlayer> AllPlayer { get { return playerList; } }


  

    public CSPlayer GetCSPlayerByNearWithOutCheckAlive(Vector3 position)
    {
        float nearValue = 100;
        CSPlayer tempPlayer = null;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] == null)
            {
                continue;
            }
            float tempValue = Vector3.Distance(playerList[i].transform.position, position);
            if (nearValue > tempValue || tempPlayer == null)
            {
                nearValue = tempValue;
                tempPlayer = playerList[i];
            }
        }
        return tempPlayer;
    }

   

    private void Awake()
    {
        instance = this;
    }

    void Start () {
		
	}
	
	void Update () {
	
	}

   
}
