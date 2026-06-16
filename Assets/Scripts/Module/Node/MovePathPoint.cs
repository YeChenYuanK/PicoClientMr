using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePathPoint : MonoBehaviour {

	public Transform NextPoint;
	public Transform LastPoint;
	public List<BasePoint> list;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool ContainPoint(BasePoint point)
	{
		if (list != null && list.Contains (point)) {
			return true;
		} else {
			return false;
		}
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "Node1.tif" , true);
		if(NextPoint != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine (transform.position, NextPoint.position);
		}
	}
}
