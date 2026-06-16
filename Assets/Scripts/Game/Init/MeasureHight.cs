using UnityEngine;
using System.Collections;

public class MeasureHight : MonoBehaviour {

	public Transform cross;
	public SelfCharacter character;
	public bool isTriger = false;

	public float selfHight = 0;
	public float selfMinHight = 0;
	public const float HeadMaxHight = 1.7f;
	public const float HeadMinHight = 0.8f;

	// Use this for initialization
	void Start () {
		selfHight = HeadMaxHight;
		selfMinHight = HeadMinHight;
//		character.htcInput.leftHandle.TriggerLoss.Add (htcTrigger);
	}

	void htcTrigger(object param)
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 1000))
		{
			if (hit.collider.GetComponent<FollowHead> () != null) 
			{
				//测量身高
				selfHight = transform.localPosition.y;
				selfMinHight = selfHight / HeadMaxHight * HeadMinHight;
				Debug.Log ("Hit hight:" + selfHight);
				isTriger = true;
				cross.gameObject.SetActive (false);
			}
		}
	}


	// Update is called once per frame
	void Update () {

		if(!isTriger)
		{
			if(Input.GetKey(KeyCode.F))
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position, transform.forward, out hit, 1000))
				{
					if (hit.collider.GetComponent<FollowHead> () != null) 
					{
						//测量身高
						selfHight = transform.localPosition.y;
						selfMinHight = selfHight / HeadMaxHight * HeadMinHight;
						Debug.Log ("Hit hight:" + selfHight);
						isTriger = true;
						cross.gameObject.SetActive (false);
					}
				}
			}
		}
	}
}
