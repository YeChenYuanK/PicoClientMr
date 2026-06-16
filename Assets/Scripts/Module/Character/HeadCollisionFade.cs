using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionFade : MonoBehaviour {

    private bool headsetColliding = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if (headsetColliding)
  //      {
  //          SteamVR_Fade.View(Color.black, 0);

  //      }
	}
    private void OnEnable()
    {
        headsetColliding = false;
        //@todo 设置不想碰撞白名单
        
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(0.1f, 0.1f, 0.1f);

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    /// <summary>
    /// The IsColliding method is used to determine if the headset is currently colliding with a valid game object and returns true if it is and false if it is not colliding with anything or an invalid game object.
    /// </summary>
    /// <returns>Returns true if the headset is currently colliding with a valid game object.</returns>
    public virtual bool IsColliding()
    {
        return headsetColliding;
    }

    private void OnDisable()
    {
        headsetColliding = false;
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.GetComponent<Rigidbody>());
    }

    private void OnTriggerStay(Collider collider)
    {
        if (enabled && !headsetColliding)
        {
            headsetColliding = true;
            //SteamVR_Fade.Start(Color.clear, 0f);
            //SteamVR_Fade.Start(Color.black, 0.5f);
            //提示退出掩体
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (enabled)
        {
            headsetColliding = false;
            //SteamVR_Fade.Start(Color.clear, 0.5f);
        }
    }

}
