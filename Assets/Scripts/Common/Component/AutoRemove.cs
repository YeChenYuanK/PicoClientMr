using UnityEngine;
using System.Collections;

public class AutoRemove : MonoBehaviour {

	public long RemoveUnit = 5000;
	private long RemoveTime = 0;

	// Use this for initialization
	void Start () {
		RemoveTime = DateUtil.NowMllSec + RemoveUnit;
	}
	
	// Update is called once per frame
	void Update () {
		if(RemoveTime < DateUtil.NowMllSec)
		{
			GameObject.Destroy (gameObject);
		}
	}
}
