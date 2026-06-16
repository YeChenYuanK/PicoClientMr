using UnityEngine;
using System.Collections;

public class AutoHide : MonoBehaviour {

	public long HideUnit = 5000;
	private long HideTime = 0;

	// Use this for initialization
	void Start () {
		HideTime = DateUtil.NowMllSec + HideUnit;
	}

    void OnEnable()
    {
        HideTime = DateUtil.NowMllSec + HideUnit;
    }
	
	// Update is called once per frame
	void Update () {
		if(HideTime < DateUtil.NowMllSec)
		{
			gameObject.SetActive (false);
		}
	}
}
