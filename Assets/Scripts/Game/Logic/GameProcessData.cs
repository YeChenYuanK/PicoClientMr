using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcessData 
{
    private static GameProcessData _ins;
    public static GameProcessData Instance
    {
        get
        {
            if (_ins == null) _ins = new GameProcessData();
            return _ins;
        }
    }

    private int playcount = 0;
    public void IncrGamePlayCount()
    {
        playcount++;
        Debug.Log("进入比赛地图 开始玩一局 增加游玩次数");
    }

    public int PlayRoundCount
    {
        get
        {
            return playcount;
        }
    }
}
