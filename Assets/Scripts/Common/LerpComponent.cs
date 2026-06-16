using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpComponent : MonoBehaviour {

    public float ExeTime;
    public Transform Target;

    private float StartTime = -1;
    private Quaternion orgRotation;
    private Vector3 orgScale;
    private Vector3 orgPosition;

    private Quaternion targetRotation;
    private Vector3 targetScale;
    private Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {
        orgRotation = transform.rotation;
        orgPosition = transform.position;
        orgScale = transform.localScale;
        targetPosition = Target.position;
        targetRotation = Target.rotation;
        targetScale = Target.localScale;
        //TweenRotation.
        //Init();
    }

    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        StartTime = Time.fixedTime;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (StartTime == -1)
        {
            return;
        }
        if (ExeTime <= Time.fixedTime - StartTime)
        {
            //Quaternion.FromToRotation()
            //orgRotation.
            transform.rotation = targetRotation;
            transform.localScale = targetScale;
            transform.position = targetPosition;
            return;
        }
        float t = Mathf.Clamp((Time.fixedTime - StartTime) / ExeTime, 0, 1);
        transform.rotation = Quaternion.Lerp(orgRotation, targetRotation, t);
        transform.localScale = Vector3.Lerp(orgScale, targetScale, t);
        transform.position = Vector3.Lerp(orgPosition, targetPosition, t);
        
    }
}
