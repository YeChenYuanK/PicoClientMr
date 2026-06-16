using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtArrow : MonoBehaviour {

    private float hideTime = 0;

    private void Awake()
    {
        
    }

    void Start () {
		
	}
	
	void Update () {
	    if(hideTime > 0)
        {
            if(Time.time > hideTime)
            {
                this.gameObject.SetActive(false);
                hideTime = 0;
            }
        }	
	}

    public void ShowAngle(float angle)
    {
        this.gameObject.SetActive(true);
        // this.transform.rotation = Quaternion.identity;
        this.transform.localEulerAngles = new Vector3(0, 0, angle);
        hideTime = Time.time + 1.5f;
    }

}
