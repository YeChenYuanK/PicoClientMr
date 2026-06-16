using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float ShootSpeed = 80.0f;

	void Start () {
		
	}
	
	void FixedUpdate () {
        this.transform.Translate(this.transform.forward * ShootSpeed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
