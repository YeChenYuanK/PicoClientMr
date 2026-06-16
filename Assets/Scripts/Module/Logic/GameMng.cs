using com.leke.redSea;
using Mirror;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using VRLeBigSpaceSDK;
using static DataManager;
using static ParametersManager;

public class AppStartParam
{
    public string serverIp;
    public string sceneSize;
    public string lang;
    public string extraData;
    public string DeviceIndex;
}
public class GameMng : MonoBehaviour
{

    public Action RefreshUI;
    public Action ChangeLanguage;
    public Action<int, int> RefreshHpUI;
    public bool isTest;
    private static GameMng instance;
    public static GameMng Instance
    {
        get
        {
            return instance;
        }
    }
    public CSPlayer _mySelf
    {
        get { return _playerInfoMng._mySelfPlayer; }
        set { _playerInfoMng._mySelfPlayer = value; }
    }
    //游戏状态




    public bool AllowTestDieState = true;

    public bool isGetServerInfo;
    public int DoctorTalkTime = 30;

    public float SelfHurtAudioCD = 5.0f;
    public bool IsOnlyBoss = false;
    public int DefaultCamp = 1;


    public bool friendHurt = false;
    private Dictionary<int, bool> readyDict = new Dictionary<int, bool>();
    /// <summary>
    /// 当前死亡玩家复活方式
    /// </summary>
    public RebirthType CurRebirthType = RebirthType.FORMER_PLACE;

    //Chenyang
    public bool isServerHost;
 
 
 
    public TestHaptic truegearConnectorMng { get; private set; }
    //mirrior 管理类
    public MirrorManager _mirrorMng;
    //时间管理类
    public TimeManager _TimeMng { get; private set; }
    //Record类
    public GameRecord _gameRecordMng { get; private set; }
    //人员管理类
    public PlayerInfoManager _playerInfoMng;
    //资源管理类
    public PrefebManager _prefebMng { get; private set; }

    public ParametersManager _parameterMng { get; private set; }
    public GameUIManager _uiMng { get; private set; }
    public CameraManager _camMng { get; private set; }
    public bool isClientTest;
    public bool isGameing;
    private StateManager staMng;
    public StateManager _staMng
    {
        get
        {
            if (staMng == null)
            {
                staMng = StateManager._instance;
            }
            return staMng;

        }
    }
    public int LanguageState;
    public void ChangeLan(int index)
    {
        if(_staMng!=null)
        _staMng.SetLan(index);
    }
    public void ChangeLanguageState(int index)
    {
        LanguageState = index;
         ChangeLanguage?.Invoke();
       
    }
    public int GameTime { get; set; }
    public int GameState { get; set; }
    public bool isGameState(int state)
    {
        if (GameState == state)
            return true;
        return false;
    }
    public int FightCount { get; set; }
    public List<UnityEngine.Object> Scenes = new List<UnityEngine.Object>();
    public string PlayerName;
    public int index = 1;
  
    void GetServerip()
    {

        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        string text = intent.Call<string>("getStringExtra", "startExtraDataStr");
        Debug.Log("获得启动额外参数 : " + text);

        AppStartParam appStartParam = JsonConvert.DeserializeObject<AppStartParam>(text);
        //appStartParam.DeviceIndex = appStartParam.DeviceIndex;
        PlayerName = appStartParam.DeviceIndex;

       
    }
    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 72;
        //if (!isServerHost&&!isTest) {
        //    GetServerip();
        //}
       
        //if (isClientTest||isServerHost)
        //    XRSettings.enabled = false;
        DontDestroyOnLoad(gameObject);
        if (!isServerHost)
        {
            gameObject.AddComponent<ThermalMonitor>();
        }
    }
    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
       
    }
    public void GameInit() {

        if (!isServerHost)
        {
            try
            {
                VRLeGun.Initialize();
            }
            catch (Exception e)
            {
                // VRLeBigSpaceSDK 在部分设备/参数下会抛 Socket 参数异常，降级为不启用外设，不阻断主流程。
                Debug.LogWarning("VRLeGun.Initialize failed, continue without VRLe gun: " + e);
            }
        }
        StartCoroutine(BootStart());
    }


    IEnumerator BootStart()
    {

        // yield return PreloadSceneRoutine();
        //Debug.LogError("111111111");
        yield return InitManager();
        Env.Instance.PlayRoomBGM();
       
        yield return _mirrorMng.HostServerOrClient(isServerHost);
      


    }
    IEnumerator InitManager()
    {

        truegearConnectorMng = GetComponent<TestHaptic>();
         _prefebMng = GetComponent<PrefebManager>();
    
            _parameterMng = new ParametersManager(this);
            _parameterMng.OnInit();

       

        _camMng = GetComponent<CameraManager>();
        _camMng.OnInit();
       
       _playerInfoMng = new PlayerInfoManager(this);
        _playerInfoMng.OnInit();

        _TimeMng = new TimeManager(this);

        _gameRecordMng = new GameRecord(this);
        _uiMng = GetComponent<GameUIManager>();
        _uiMng.OnInit();

       

        yield break;
    }


    //public IEnumerator PreloadSceneRoutine()
    //{
    //    for (int i = 0; i < SceneName.Count; i++)
    //    {
    //        string name = SceneName[i];
    //        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
    //        asyncLoad.allowSceneActivation = false;
    //        while (!asyncLoad.isDone&&asyncLoad.progress < 0.9f)
    //        {
    //            yield return null;
    //        }
    //        Debug.LogError(name);
    //        SceneOpHandlers.Add(name, asyncLoad);
    //    }
    //    yield break;
    //    // 等待加载到90%（Unity限制，不激活场景最多加载到90%）
    //}

    // 异步加载场景但不激活


    //public Dictionary<int,bool> ReadyDict
    //{
    //    get
    //    {
    //        return readyDict;
    //    }
    //}

    //[PunRPC]
    //private void SceneReady(int index)
    //{
    //    readyDict[index] = true;
    //}

    //[PunRPC]
    //public void StartBattle()
    //{
    //    SceneLoader.Instance.StartScene("GameScene_BigSpace_2_Factory");
    //}

    //public override void Update()
    //{
    //    base.Update();
    //    //stateMachine.SMUpdate();
    //    //if(GameMng.Instance.GetCurrStateType() == null || GameMng.Instance.GetCurrStateType() == typeof(MovieState))
    //    //{
    //    //    if (Game.GameLogic.Instance != null && Game.GameLogic.Instance.IsAllSceneReady())
    //    //    {
    //    //        // 所有人场景加载完了 要进入 准备状态
    //    //        GameMng.Instance.ChangeState(typeof(BeforeGamingState));
    //    //    }
    //    //}
    //}

    public void SetGameState(int state, int gameTime)
    {
        isFire = false;
        GameState = state;
        GameTime = gameTime;
        if (GameState == 1)
        {

            GameMng.Instance._playerInfoMng.Clear();
            FightCount += 1;
            PrepareData.Instance.LastKillTime = 0;
            PrepareData.Instance.KillNum = 0;
            GameMng.Instance.isGameing = true;
           
        }
        else
        {
            _playerInfoMng.Players.Clear();


        }

    }
  
    private int mapIndex;
    public void GameStart(int MapIndex)
    {
        if (isGameing) return;
        GameMng.Instance._staMng.CmdSetGameState(1, _parameterMng._parameters.GameTime);
        //切换UI
        isFire = false;
        if (_staMng != null) _staMng.RpcStopFire();
        mapIndex = MapIndex;
        Invoke("ChangeScene", 1f);
    }
    public void ChangeScene()
    {
        var mapinfo = GameMng.Instance._prefebMng.GetMapInfoByIndex(mapIndex);
        isFire = false;
        if (_staMng != null) _staMng.RpcStopFire();
        NetworkManager.singleton.ServerChangeScene(mapinfo.MapScene);
        _uiMng.OnChangePanel(PanelState.UIServerRankPanel);
    }



    public bool isFire;
    public void GameEnd()
    {
        GameMng.Instance._staMng.CmdSetGameState(0, _parameterMng._parameters.GameTime);
        isFire = false;
        if (_staMng != null) _staMng.RpcStopFire();
        Invoke("DelyEnd", 1f);
       

    }
    public void DelyEnd()
    {


        _uiMng.OnChangePanel(PanelState.UIServerRoomPanel);
        _uiMng.OpenServerOverPanel();
        isFire = false;
        if (_staMng != null) _staMng.RpcStopFire();
        // PrepareData.Instance.GameRecord =_gameRecordMng;
        NetworkManager.singleton.ServerChangeScene("00_Prepare");
    }

    public void Record(int playerId, RecordType recordType)
    {

        if (isServerHost)
        {
            PlayerInfo player = _playerInfoMng.GetPlayerInfoById(playerId);

            if ((RecordType)recordType == RecordType.DEAD)
            {
                player.Deads++;
            }
            else if ((RecordType)recordType == RecordType.HEADSHOT)
            {
                player.HeadShots++;
            }
            else if ((RecordType)recordType == RecordType.KILL)
            {
                player.Kills++;
            }
            RefreshUI?.Invoke();
        }

       
      

    }
    //public void PlayAudio(AudioType audio)
    //{
    //    AudioType audioType = (AudioType)audio;
    //    if (Env.Instance != null && Env.Instance.ContiansAudio(audioType))
    //    {
    //        Env.Instance.PlayEnvEffect(audioType);
           
    //    }
    //}
    public void ParseRecordData(byte[] data)
    {
        _gameRecordMng.ParseRecordData(data);
    }

    public byte[] GenData()
    {
        return _gameRecordMng.GenRecordData();
    }



}
