using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using System.Collections.Generic;

/// <summary>
/// 积分管理器
/// </summary>
public class ScoreManager {

    private static ScoreManager instance ;

    public static ScoreManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ScoreManager();
            }
            return instance;
        }
    }

	private Dictionary<int, int> scoreMap = new Dictionary<int, int> ();

    public void UpdateScore(int camp, int score)
    {
        scoreMap[camp] = score;
    }

	public int GetScroeByCamp(int camp)
	{
		return scoreMap[camp];
	}
	
}
