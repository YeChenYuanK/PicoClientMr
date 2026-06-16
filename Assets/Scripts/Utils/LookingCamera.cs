using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingCamera : MonoBehaviour {

	
	void Start () {
		
	}
	
	void Update () {
		if(Camera.main != null)
        {
            this.transform.LookAt(new Vector3(Camera.main.transform.position.x, this.transform.position.y, Camera.main.transform.position.z));
        }
	}
}
