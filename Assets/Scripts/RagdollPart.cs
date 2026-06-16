using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPart : BasePart {
    public Transform RootRagroll;
    public Rigidbody rigidBody;
    public Collider iCollider;
    // Use this for initialization
    void Awake ()
    {
        rigidBody = GetComponent<Rigidbody>();
        iCollider = GetComponent<Collider>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeState(bool IsRagdoll)
    {
        //if (IsRagdoll)
        //{
            rigidBody.useGravity = IsRagdoll;
            rigidBody.isKinematic = !IsRagdoll;
            iCollider.isTrigger = !IsRagdoll;
        //}
    }
}
