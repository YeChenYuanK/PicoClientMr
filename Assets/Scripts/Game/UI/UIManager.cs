using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.gamestudio.cs;

/// <summary>
/// 临时UI管理器.
/// </summary>
public class UIManager : MonoBehaviour 
{
	public Text camp1Score;
	public Text camp2Score;
	public MiniMap miniMap;
	public Image CampImage;
	public ReviveCount reviveCount;
	public BattleSettleUI battleSettleUI;
	public OccpyCDUI OccpyCD;

	public Material RedMat;
	public Material GreenMat;

	public static UIManager Instance;
	// Use this for initialization
	void Awake () 
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void ShowReviveCount(int leftSec)
	{
		if(!reviveCount.gameObject.activeSelf)
		{
			reviveCount.gameObject.SetActive (true);
		}
		reviveCount.ShowNum (leftSec);
	}

	public void HideReviveCount()
	{
		if(reviveCount.gameObject.activeSelf)
		{
			reviveCount.gameObject.SetActive (false);
		}
	}

	public void ShowOccupyCD(long endTime)
	{
		OccpyCD.StartCD (endTime);
	}

	public void HideOccupyCD()
	{
		OccpyCD.gameObject.SetActive (false);
	}

	public void UpdateScroe(int camp, int score)
	{
		if (camp == 1) 
		{
			camp1Score.text = "Camp1:" + score;
		} else 
		{
			camp2Score.text = "Camp2:" + score;
		}
	}

	public void ShowBattleSettleUI(MBattleSettle battleSettle)
	{
		battleSettleUI.gameObject.SetActive (true);
		battleSettleUI.Init (battleSettle);
	}
}
