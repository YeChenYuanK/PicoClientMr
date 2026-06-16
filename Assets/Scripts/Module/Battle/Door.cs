using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState
{
    CLOSE,
    OPENING,
    OPENED,
    CLOSING
}

public class Door : MonoBehaviour
{
    /// <summary>
    /// 当前门的状态
    /// </summary>
    private DoorState doorState;
    /// <summary>
    /// 默认关闭位置
    /// </summary>
    public Transform ClosePoint;
    /// <summary>
    /// 默认开启位置
    /// </summary>
    public Transform OpenPoint;
    /// <summary>
    /// 门对象
    /// </summary>
    public GameObject doorObj;
    /// <summary>
    /// 所有开关集合
    /// </summary>
    public List<DoorSwitch> DoorSwitches;
    /// <summary>
    /// 开始打开时间
    /// </summary>
    private float startOpenTime;
    /// <summary>
    /// 開始關閉時間
    /// </summary>
    private float startCloseTime;

    public bool defaultOpen;

    public AudioSource audio;
    public AudioClip audioClip;

   
    void Start ()
    {
      
        if (defaultOpen)
        {
            this.OpenDoor0();
        }
    }

    /// <summary>
    /// 开启门
    /// </summary>
    public void OpenDoor()
    {
        if (doorState != DoorState.CLOSE)
        {
            return;
        }
     
    }

    public bool IsClose()
    {
        return this.doorState == DoorState.CLOSE;
    }

   
    public void OpenDoor0()
    {
        doorObj.transform.position = ClosePoint.position;
        this.doorState = DoorState.OPENING;
        this.UpdateDoorSwitch();
        this.startOpenTime = Time.time;
        // 播放门开的声音
        this.audio.PlayOneShot(this.audioClip);
    }

    private void UpdateDoorSwitch()
    {
        for (int i = 0; i < this.DoorSwitches.Count; i++)
        {
            this.DoorSwitches[i].UpdateSwitchState(this.doorState);
        }
    }

    private void Update()
    {
        if (this.doorState == DoorState.OPENING)
        {
            if (this.startOpenTime == 0)
            {
                Debug.Log("startOpenTime doorState error ");
                this.doorState = DoorState.CLOSE;
                this.UpdateDoorSwitch();
                return;
            }
            float deltaTime = ConfigHelper.UserCfg.DoorOpenTime - (Time.time - this.startOpenTime);
            if (deltaTime < 0)
            {
                this.doorState = DoorState.OPENED;
                this.UpdateDoorSwitch();
                this.startCloseTime = Time.time + ConfigHelper.UserCfg.DoorOpenKeepTime;
                return;
            }
            this.doorObj.transform.position = Vector3.Lerp(this.ClosePoint.position, this.OpenPoint.position,
                (Time.time - this.startOpenTime) / ConfigHelper.UserCfg.DoorOpenTime);
        } else if (this.doorState == DoorState.OPENED)
        {
            if (Time.time >= this.startOpenTime)
            {
                this.doorState = DoorState.CLOSING;
                // 播放门开的声音
                this.audio.PlayOneShot(this.audioClip);
                this.UpdateDoorSwitch();
            }
        }
        else if(this.doorState == DoorState.CLOSING)
        {
            float deltaTime = ConfigHelper.UserCfg.DoorCloseTime - (Time.time - this.startCloseTime);
            if (deltaTime < 0)
            {
                this.doorState = DoorState.CLOSE;
                this.UpdateDoorSwitch();
                return;
            }
            this.doorObj.transform.position = Vector3.Lerp(this.OpenPoint.position, this.ClosePoint.position,
                (Time.time - this.startCloseTime) / ConfigHelper.UserCfg.DoorCloseTime);
        }
    }

}
