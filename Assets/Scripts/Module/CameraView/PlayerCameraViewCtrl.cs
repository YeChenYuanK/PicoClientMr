using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraViewCtrl : MonoBehaviour {

    private static PlayerCameraViewCtrl instance;

    public static PlayerCameraViewCtrl Instance
    {
        get
        {
            return instance;
        }
    }

    private CSPlayer curViewPlayer;

    private int cursor = 0;
    public Camera TopCamera;
    public Camera viewCamera;
 
    private bool ShowPlayerCamera;
    private void Awake()
    {
        instance = this;
    }

    void Start () {
       
	}
	
	void FixedUpdate () {
	    if(curViewPlayer == null && GameMng.Instance._playerInfoMng.Players.Count>0)
        {
            curViewPlayer = GameMng.Instance._playerInfoMng.GetPlayerByindex(cursor);
            //curViewPlayer.SetOtherModelShow(false);
        }	

        if(curViewPlayer != null)
        {
            viewCamera.transform.position = curViewPlayer.cameraPoint.position;
            viewCamera.transform.rotation = curViewPlayer.cameraPoint.rotation;
        }
	}

    public void ChangeNextView()
    {
        if (GameMng.Instance._playerInfoMng.Players.Count == 0) return;
        this.cursor++;
        this.cursor = this.cursor % GameMng.Instance._playerInfoMng.Players.Count;
        this.curViewPlayer = GameMng.Instance._playerInfoMng.Players[cursor];
        Debug.Log("当前玩家：" + this.curViewPlayer.playerId);
        //this.curViewPlayer.SetOtherModelShow(false);

    }
    public void ChangeCamera()
    {

        if (!ShowPlayerCamera&&curViewPlayer==null)
        {
            return;
        }

        ShowPlayerCamera = !ShowPlayerCamera;
        //TOP
        TopCamera.gameObject.SetActive(!ShowPlayerCamera);
        viewCamera.gameObject.SetActive(ShowPlayerCamera);
    }
}
