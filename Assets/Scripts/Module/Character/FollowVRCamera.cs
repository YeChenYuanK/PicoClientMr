using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVRCamera : MonoBehaviour
{
    public Transform TargetPoint;
    public Transform FollowPoint;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(TargetPoint != null)
        {
            transform.LookAt(new Vector3(TargetPoint.transform.position.x , transform.position.y , TargetPoint.transform.position.z));
        }
        if(FollowPoint != null)
        {
            transform.position = new Vector3(FollowPoint.transform.position.x, transform.position.y, FollowPoint.transform.position.z);
        }
        
	}
}
