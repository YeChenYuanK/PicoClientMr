using UnityEngine;
using System.Collections;
//RoleCommand
public class PositionLimit : MonoBehaviour {

	//示例飞行员数据
	//L :  x:-0.53 -0.278 y:1 1.793F z:-0.141F 0.403
	//R ： x:0.3f 0.5f y:同上 z:同上
	public Vector3 minPosition;
	public Vector3 maxPosition;
	public Transform orginalPosition;
	private float MaxHight = 1.75f;
	private Vector3 currPosition = Vector3.zero;

	void Awake () {
		MaxHight = orginalPosition.localPosition.y;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		
		currPosition.x = Mathf.Clamp (transform.localPosition.x , minPosition.x , maxPosition.x);
		currPosition.y = Mathf.Clamp (transform.localPosition.y - orginalPosition.localPosition.y , minPosition.y - MaxHight , maxPosition.y - MaxHight);
		currPosition.y += orginalPosition.localPosition.y;
		currPosition.z = Mathf.Clamp (transform.localPosition.z , minPosition.z , maxPosition.z);
		transform.localPosition = currPosition;

	}
//
//	public void Execute(Notification notification)
//	{
//		//Role_attack
//		switch(opcode)
//		{
//		case "attack":
//			//处理数据，操作model层
//			//派发事件，需要的Mediator（View的代理）捕获
//			break;
//		case "move":
//			break;
//		}
//		//notification.option
//	}

}
