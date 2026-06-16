using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserConfig
{
    public float DoorOpenTime = 1.0f;
    public float DoorOpenKeepTime = 10f;
    public float DoorCloseTime = 1.0f;
    public int gunType = 1; // 1 是ppgun 2 是手柄枪
    public float xRotation = -30f;
    public float zRotation = 0f;

    public string ServerIp = "192.168.1.123";
    public int ServerPort = 9596;
}
