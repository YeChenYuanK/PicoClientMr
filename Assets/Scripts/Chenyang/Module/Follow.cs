using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    void Start()
    {
        
    }

    public void FixedUpdate()
    {
        if (Target == null) return;
        transform.position = Target.position;
        transform.rotation = Target.rotation;
    }
}
