using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class AnimatorParam : MonoBehaviour {

    public double serverStartTime = 0;
    public virtual double PassTime
    {
        get
        {
            return (NetworkTime.time - serverStartTime);
        }
    }
    public float ActionTime = 0;
    public Animator anim;

    // Use this for initialization
    void Start() {
        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(anim != null)
        anim.SetFloat("Time", Mathf.Max(0,(float)PassTime - ActionTime));
	}
}
