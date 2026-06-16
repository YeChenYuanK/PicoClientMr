using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeviceInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 收集设备信息输出到日志
        Debug.Log("SystemInfo.supportsComputeShaders : " + SystemInfo.supportsComputeShaders);
        Debug.Log("SystemInfo.maxComputeBufferInputsVertex : " + SystemInfo.maxComputeBufferInputsVertex);
        Debug.Log("SystemInfo.maxComputeBufferInputsVertex : " + SystemInfo.maxComputeBufferInputsVertex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
