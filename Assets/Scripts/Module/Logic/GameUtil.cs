using com.leke.redSea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil {

    /// <summary>
    /// 计算积分
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    public static int ClacScore(PlayerRecord record)
    {
        return record.Kills + record.HeadShots * 2 + record.winScore;
    }
    public static int CountScore(PlayerInfo record)
    {
        return record.Kills + record.HeadShots;
    }

}
