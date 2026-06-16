using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OccpyCDUI : MonoBehaviour 
{
	public Image cdImage;
	public TextMesh cdTxt;
	private bool IsOver = false;
	private long StartTime;
	private long unitTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame`
	void Update () {
		if (!IsOver) {
			if (DateUtil.NowMllSec >= StartTime + unitTime) 
			{
				IsOver = true;
			} else {
				cdImage.fillAmount = (float)(DateUtil.NowMllSec - StartTime) / (float) unitTime;
				cdTxt.text = Mathf.Clamp((unitTime - (DateUtil.NowMllSec - StartTime))/1000 + 1 , 0 , unitTime).ToString();
			}
		} else 
		{
			gameObject.SetActive (false);
		}
	}

	public void StartCD(long time)
	{
		if(time < 0)
		{
			return;
		}
		unitTime = time;
		StartTime = DateUtil.NowMllSec;
		IsOver = false;
		gameObject.SetActive (true);
		cdTxt.text = time.ToString ();
	}
}
