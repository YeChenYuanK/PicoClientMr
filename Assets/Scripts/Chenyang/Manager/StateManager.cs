using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using static ParametersManager;

public class StateManager : NetworkBehaviour
{
    public static StateManager _instance;

    //public static StateManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = FindObjectOfType<StateManager>();
    //        }
    //        return _instance;
    //    }
    //}

  
    public LogicLoad _LogicLoad;
    private float StartTime;
    [SyncVar]
    public string size;
    [SyncVar]
    public int LanguageState;
    [SyncVar]
    public int bloothCloth;
    public SyncDictionary<string, string> BloothConnects = new SyncDictionary<string, string>();

    public void CmdSetGameState(int state, int GameTime)
    {
        RpcSetState(state, GameTime);
        GameMng.Instance.SetGameState(state, GameTime);
    }
    
    protected void Awake()
    {
        _instance = this;
       
       
    }
    public void Start()
    {
        if (GameMng.Instance.isGameState(1))
        {
            StartTime = Time.time;

        }
        else
        {
            StartTime = 0;
        }
        if (isServer)
        {
            bloothCloth = GameMng.Instance._parameterMng._parameters.BloothCloth;
            size = GameMng.Instance._parameterMng._parameters.sceneSize;
            LanguageState = GameMng.Instance.LanguageState;
            for (int i = 0; i < GameMng.Instance._parameterMng._parameters.BloothConnectInfos.Count; i++)
            {
                BloothConnectInfo info = GameMng.Instance._parameterMng._parameters.BloothConnectInfos[i];
                if (!BloothConnects.ContainsKey(info.sn))
                {
                    BloothConnects.Add(info.sn, info.bloothName);
                }
            }
            SetLan(LanguageState);
            if (GameMng.Instance.isGameState(1))
            {
                Env.Instance.PlayGameStart();
            }
            else
            {
                Env.Instance.PlayRoomBGM();
            }
        }
        else
        {
            GameMng.Instance.ChangeLanguageState(LanguageState);
        }
    }
    public void SetLan(int lan)
    {
        LanguageState = lan;
        GameMng.Instance.ChangeLanguageState(lan);
        RpcSetLan(lan);
    }
    [ClientRpc]
    public void RpcSetLan(int state)
    {
        GameMng.Instance.ChangeLanguageState(state);
    }

    [ClientRpc]
    public void RpcSetState(int state, int MaxTime)
    {
        GameMng.Instance.SetGameState(state, MaxTime);
    }
    public void Update()
    {

        if (!isServer) return;


        if (GameMng.Instance.GameState == 1 && StartTime != 0)
        {
            int leftTime = (int)(GameMng.Instance._parameterMng._parameters.GameTime - (Time.time - StartTime));

            if (leftTime <= 0)
            {
                leftTime = 0;
                StartTime = 0;
              
                    GameMng.Instance.isGameing = false;
                    GameMng.Instance._playerInfoMng.EndCountRecords();
                    GameMng.Instance.RefreshUI?.Invoke();
                    ShowUIEndCount();
                    GameMng.Instance.GameEnd();
                
            }
            GameMng.Instance.GameTime = leftTime;
            SetGameTime(leftTime);
        }
    }
    [ClientRpc]
    public void SetGameTime(int Time)
    {
        GameMng.Instance.GameTime = Time;
    }
    [ClientRpc]
    public void ShowUIEndCount()
    {
        GameMng.Instance.isGameing = false;
       int win= GameMng.Instance._playerInfoMng.ClinetEndCountRecords();  
        Env.Instance.PlayGameOverAudio(win);
            
        
        //if (GameSceneManager.Instance != null)
        //{
        //    GameSceneManager.Instance.GameEnd = true;
        //}
    }
    [ClientRpc]
    public void RpcQuit()
    {
        Debug.Log("[EXIT-DIAG] StateManager.RpcQuit -> calling Application.Quit()");
        Application.Quit();
    }
    [ClientRpc]
    public void RpcStopFire()
    {
        GameMng.Instance.isFire = false;
        if (GameMng.Instance._mySelf != null && GameMng.Instance._mySelf.gunController != null)
        {
            GameMng.Instance._mySelf.gunController.ReleaseTrigger();
        }
    }
}



[System.Serializable]
public class PicoInfo
{
   
    public string bloothName;
    public int index;
}









//    //0是 准备中 1是游戏中

//}
