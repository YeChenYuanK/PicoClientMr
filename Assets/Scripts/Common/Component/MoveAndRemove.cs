using UnityEngine;
using System.Collections;

public class MoveAndRemove : MonoBehaviour {

	public float MoveSpeed = 10f;
	private Vector3 targetPos;
	private Vector3 currPos;
	private float moveTime;
	private long startDate;

	public void Init(Vector3 currPos , Vector3 targetPos)
	{
		this.currPos = currPos  + new Vector3(0,1.5f,0);
		this.targetPos = targetPos + new Vector3(0,1.5f,0);
		float dis = Vector3.Distance (currPos , targetPos);
		moveTime = dis / MoveSpeed;
		startDate = DateUtil.NowMllSec;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (currPos, targetPos , Mathf.Clamp((DateUtil.NowMllSec - startDate) / (moveTime*1000) , 0 , 1));
		if(Vector3.Distance(transform.position  , targetPos) < 0.02f)
		{
			Destroy (gameObject);
		}
	}
}
