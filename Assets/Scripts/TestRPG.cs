using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRPG : MonoBehaviour {
    public GameObject rpgPrefabs;
    public GameObject target;
    Vector3 lastPosition = Vector3.zero;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = transform.position - lastPosition;
        if(lastPosition != Vector3.zero && (direction.z > 0.02f || direction.x > 0.02f))
        {
            Debug.Log(transform.InverseTransformDirection(direction) / Time.deltaTime);
        }

        lastPosition =  transform.position;
        //if (Input.GetKeyUp(KeyCode.F))
        //{
        //    GameObject rpgObj = GameObject.Instantiate<GameObject>(rpgPrefabs, transform.position, transform.rotation);
        //    rpgObj.GetComponent<Rocket>().Init(target.transform.position,3 ,2);
        //}
    }
}
