using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    /// <summary>
    /// 控制的门
    /// </summary>
    public Door ControlDoor;

    public GameObject touchableObj;

    public GameObject unTouchableObj;

    public AudioSource audio;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("DoorSwitch : " + collider.name);
        if (collider.name.Equals("Gun"))
        {
            if (!this.ControlDoor.IsClose())
            {
                return;
            }
            // 播放触碰声音
            this.audio.Play();
            // 開啟們
            this.ControlDoor.OpenDoor();
        }
    }

    public void UpdateSwitchState(DoorState doorState)
    {
        if (doorState == DoorState.CLOSE)
        {
            this.touchableObj.SetActive(true);
            this.unTouchableObj.SetActive(false);
        } else if (doorState == DoorState.OPENING)
        {
            this.touchableObj.SetActive(false);
            this.unTouchableObj.SetActive(true);
        } else if (doorState == DoorState.OPENED)
        {
            this.touchableObj.SetActive(false);
            this.unTouchableObj.SetActive(false);
        } else if (doorState == DoorState.CLOSING)
        {
            this.touchableObj.SetActive(false);
            this.unTouchableObj.SetActive(false);
        }
    }

}
