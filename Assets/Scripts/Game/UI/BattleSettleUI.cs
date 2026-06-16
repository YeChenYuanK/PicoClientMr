using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.gamestudio.cs;

public class BattleSettleUI : MonoBehaviour 
{
	public Text bluePointText;
	public Text RedPointText;
	public Transform Item1;
	public Transform Item2;

	public void Init(MBattleSettle battleSettle)
	{
		foreach (MCampSettleInfo settleInfo in battleSettle.campSettleInfo) 
		{
			if(settleInfo.campid == (int)Camp.Blue)
			{
				UpdateItem (Item1 , settleInfo.fightUnitSettleInfo);
				bluePointText.text = settleInfo.occupyCount.ToString ();
			}
			else if(settleInfo.campid == (int)Camp.Red)
			{
				UpdateItem (Item2 , settleInfo.fightUnitSettleInfo);
				RedPointText.text = settleInfo.occupyCount.ToString ();
			}
		}
	}

	private void UpdateItem(Transform item , List<MFightUnitSettleInfo> settleInfos)
	{
		if (settleInfos == null || settleInfos.Count == 0) 
		{
			item.gameObject.SetActive (false);
			return;
		}
		Text idText = item.Find("IDText").GetComponent<Text>();
		Text killNumText = item.Find("KillNumText").GetComponent<Text>();
		Text DeadNumText = item.Find("DeadNumText").GetComponent<Text>();
		idText.text = settleInfos[0].playerid.ToString();
		killNumText.text = settleInfos[0].killCount.ToString();
		DeadNumText.text = settleInfos[0].deathCount.ToString();
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
