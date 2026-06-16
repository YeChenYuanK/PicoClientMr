using UnityEngine;
using System.Collections;

/// <summary>
/// 临时UI管理器.
/// </summary>
public class TempUIManager : MonoBehaviour 
{
	public UnityEngine.UI.Text camp1Score;
	public UnityEngine.UI.Text camp2Score;
	public static TempUIManager Instance;
	// Use this for initialization
	void Awake () 
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
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
}
