using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraRig;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        var p = cameraRig.localPosition;
        p.y = OVRManager.profile.eyeHeight;
        cameraRig.localPosition = p;
        
        if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.EyeLevel)
        {
            p.y = OVRManager.profile.eyeHeight;
        }
        else if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.FloorLevel)
        {
            p.y = -(0.5f * 2) + 0;
        }
        
        cameraRig.localPosition = p;
        */
    }
}
