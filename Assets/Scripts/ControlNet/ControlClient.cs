//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using LinNet;
//using com.leke;
//using System;
//using UnityEngine.SceneManagement;
//using Newtonsoft.Json;

//public class AppStartParam
//{
//    public string serverIp;
//    public string sceneSize;
//    public string lang;
//}

//public class ControlClient : Photon.PunBehaviour
//{
//    private static ControlClient instance;
//    public static ControlClient Instance
//    {
//        get
//        {
//            return instance;
//        }
//    }

//    private string curName;

//    string GetServerip()
//    {

//        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

//        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

//        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

//        string text = intent.Call<string>("getStringExtra", "startExtraDataStr");
//        Debug.Log("获得启动额外参数 : " + text);

//        AppStartParam appStartParam = JsonConvert.DeserializeObject<AppStartParam>(text);
//        SceneDefine.CurGameStartSceneSizeParam = appStartParam.sceneSize;
//        LekeLang.GlobalLangeDefine = LangDefine.CN;
//        if (appStartParam.lang.ToLower() == "cn")
//        {
//            LekeLang.GlobalLangeDefine = LangDefine.CN;
//        }
//        else if(appStartParam.lang.ToLower() == "en")
//        {
//            LekeLang.GlobalLangeDefine = LangDefine.EN;
//        }
        
//        return appStartParam.serverIp;
//    }

//    private void Awake()
//    {
//        curName = "BigSpace";
//        instance = this;
//        client.NewSessionHandler += ConnectSucHandler;
//        client.Register(NetCode.HEART_BEAT, OnHeartBeat);
//        client.Register(NetCode.INIT_S2C, OnInitResp);
//        client.Register(NetCode.GAME_STATE_SYNC, OnGameStateSync);
//        client.Register(NetCode.CHOOSE_PROC, OnChooseProc);
//        client.Register(NetCode.UNCHOOSE_PROC, OnUnChooseProc);
//        client.Register(NetCode.CHANGE_CAMP, OnChangeCamp);
//        client.Register(NetCode.CHANGE_NAME, OnChangeName);
//        client.Register(NetCode.CHANGE_WEAPON, OnChangeWeapon);
//        client.Register(NetCode.CHANGE_HANDTYPE, OnChangeGunHandType);
//        client.Register(NetCode.SERVER_PUSH_GAME_OVER, OnGameOver);
//        client.Register(NetCode.SERVER_PUSH_GAME_ROUND_OVER, OnNext);

//        serverIp = ""; // = GetServerip();
//    }

//    private void Update()
//    {
//        if(client != null)
//        {
//            client.Update();

//            if (client.ClientSession != null)
//            {
//                if (client.ClientSession.LastCheckTime == 0)
//                {
//                    client.ClientSession.LastCheckTime = Time.time;
//                }
//                else if (Time.time - client.ClientSession.LastCheckTime > 0.3f)
//                {
//                    try
//                    {
//                        HeartBeat heartBeat = new HeartBeat();
//                        Packet packet = Packet.CreatePacket(NetCode.HEART_BEAT, heartBeat);
//                        heartBeat.time = Time.time;
//                        client.ClientSession.Send(packet);
//                    }
//                    catch (Exception e)
//                    {
//                        client.DisConnect();
//                    }
//                }
//            }
//        }
//    }

//    public string serverIp;

//    private LinClient client = new LinClient();
//    private bool tryingToConnectPhoton;

//    public void Connect()
//    {
//        connectFailCount = 0;
//        client.Connect(serverIp, ConfigHelper.UserCfg.ServerPort, OnConnectFail);
//        Debug.Log("unity connect to " + serverIp + "  " + ConfigHelper.UserCfg.ServerPort);
//    }

//    private int connectFailCount = 0;

//    private void OnConnectFail()
//    {
//        // 重连十次，重连间隔300ms
//        connectFailCount++;
//        if (connectFailCount >= 10) return;
//        this.StartCoroutine(ReconnectDelay());
//    }

//    private IEnumerator ReconnectDelay()
//    {
//        yield return new WaitForSeconds(0.3f);
//        client.Connect(serverIp, ConfigHelper.UserCfg.ServerPort, OnConnectFail);
//    }

//    private void OnChangeCamp(Session session, Packet packet)
//    {
//        // 切换camp了
//        ChangeCamp changeCamp = packet.GetProto<ChangeCamp>();
//        if(changeCamp.playerIndex == PrepareData.Instance.SelfAllocateIndex)
//        {
//            PrepareData.Instance.Camp = changeCamp.targetCamp;
//            CSPlayer player = CSPlayerManager.Instance.GetCSPlayerByAllocateIndex(changeCamp.playerIndex);
//           // player.PlayerInfo.Camp = changeCamp.targetCamp;
//        } 
        
//    }

//    /// <summary>
//    /// 游戏状态同步
//    /// </summary>
//    /// <param name="session"></param>
//    /// <param name="packet"></param>
//    private void OnGameStateSync(Session session, Packet packet)
//    {
//        GameStateSync gameStateSync = packet.GetProto<GameStateSync>();
//        GameState gameState = gameStateSync.state;
//        switch(gameState)
//        {
//            case GameState.START:
//                PrepareData.Instance.PlayerCount = gameStateSync.playerCount;
//                Debug.Log("receive data player count : " + PrepareData.Instance.PlayerCount);
//                PrepareData.Instance.MapId = gameStateSync.mapId;
//                if(gameStateSync.camp >= 0)
//                {
//                    PrepareData.Instance.Camp = gameStateSync.camp;
//                }

//                Debug.Log("游戏开启 我方阵营 : " + PrepareData.Instance.Camp);
//              //  GameMng.Instance.ChangeState(typeof(WaitForStartState));
//                break;
//            case GameState.STOP:
//                // 控制端强制要求关闭
//                Debug.Log("request enter state ForceStopState");
//              //  GameMng.Instance.ChangeState(typeof(ForceStopState));
//                break;
//        }
//    }

//    /// <summary>
//    /// 被选中
//    /// </summary>
//    /// <param name="session"></param>
//    /// <param name="packet"></param>
//    private void OnChooseProc(Session session, Packet packet)
//    {
//        ChooseGameProc chooseGameProc = packet.GetProto<ChooseGameProc>();
//        curName = chooseGameProc.roomName;
//        // PrepareLogic.Instance.ShowPlayer(chooseGameProc.index);
//    }

//    /// <summary>
//    /// 取消选中
//    /// </summary>
//    /// <param name="session"></param>
//    /// <param name="packet"></param>
//    private void OnUnChooseProc(Session session, Packet packet)
//    {
//        UnChooseGameProc unChooseGameProc = packet.GetProto<UnChooseGameProc>();
      
//        if(unChooseGameProc.index == PrepareData.Instance.SelfAllocateIndex)
//        {
//            //PhotonNetwork.LeaveRoom(true);
//            //PhotonNetwork.Disconnect();
//            //Debug.Log("離開房間");
//        }
//    }

//    /// <summary>
//    /// 初始化
//    /// </summary>
//    /// <param name="session"></param>
//    /// <param name="packet"></param>
//    void OnInitResp(Session session, Packet packet)
//    {
//        InitResp initResp = packet.GetProto<InitResp>();
//        // 记录玩家所在的位置
//        PrepareData.Instance.SelfAllocateIndex = initResp.allocateIndex;
//        PrepareData.Instance.AllocatePhotonAddress = initResp.allocatePhotonAddress;
//        Debug.Log("收到 photon address " + initResp.allocatePhotonAddress);
//        PrepareData.Instance.RoomName = initResp.curRoomId;
//        PrepareData.Instance.Camp = initResp.defaultCamp;
//        PrepareData.Instance.GameTime = initResp.gameTime;
//        PrepareData.Instance.PlayerName = initResp.playerName;
//        PrepareData.Instance.WeaponId = initResp.defaultWeapon;
//        Debug.Log("player weapin id : " + initResp.defaultWeapon + ", handtype : " + PrepareData.Instance.GunHandType);

//        PhotonNetwork.PhotonServerSettings.ServerAddress = PrepareData.Instance.AllocatePhotonAddress;

//       // if (GameMng.Instance.GetCurrStateType() != typeof(PrepareState))
//            if (true)
//            {
//            //GameMng.Instance.ChangeState(typeof(PrepareState));
//        } else
//        {
//            if(!SceneManager.GetActiveScene().name.Equals(SceneDefine.SCENE_PREPARE))
//            {
//                SceneLoader.Instance.StartScene(SceneDefine.SCENE_PREPARE);
//            }
//            else
//            {
//                if (!PhotonNetwork.inRoom)
//                {
//                    this.ConnectToPhoton();
//                }
//            }
//        }
        
//    }

//    private void OnChangeName(Session session, Packet packet)
//    {
//        ChangeName changeName = packet.GetProto<ChangeName>();
//        // 更改玩家名字
//        PrepareData.Instance.PlayerName = changeName.playerName;
//        if(CSPlayerManager.Instance != null)
//        {
//            CSPlayer player = CSPlayerManager.Instance.GetCSPlayerByAllocateIndex(PrepareData.Instance.SelfAllocateIndex);
//            if(player != null)
//            {
               
//            }
//        }
//    }

//    private void OnGameOver(Session session, Packet packet)
//    {
//        // AndroidUtil.StartLauncher();
//        Application.Quit();
//    }

//    private void OnChangeWeapon(Session session, Packet packet)
//    {
//        ChangeWeapon changeWeapon = packet.GetProto<ChangeWeapon>();
//        if (changeWeapon.playerIndex == PrepareData.Instance.SelfAllocateIndex)
//        {
//            PrepareData.Instance.WeaponId = changeWeapon.targetWeapon;
//        }
//        if (CSPlayerManager.Instance != null)
//        {
//            CSPlayer player = CSPlayerManager.Instance.GetCSPlayerByAllocateIndex(changeWeapon.playerIndex);
//            if (player != null)
//            {
//                player.PlayerInfo.weaponId = changeWeapon.targetWeapon;
//            }
//        }
//    }

//    private void OnChangeGunHandType(Session session, Packet packet)
//    {
//        ChangeWeapon changeWeapon = packet.GetProto<ChangeWeapon>();
//        Debug.Log("&&&&&&&&&&& 收到持枪类型 : " + changeWeapon.targetWeapon);
//        if (changeWeapon.playerIndex == PrepareData.Instance.SelfAllocateIndex || PrepareData.Instance.SelfAllocateIndex == 0)
//        {
//            PrepareData.Instance.GunHandType = changeWeapon.targetWeapon;
//            Debug.Log("&&&&&&&&&&& 设置持枪类型到 PrepareData.Instance.GunHandType: " + changeWeapon.targetWeapon);
//        }
//        if (CSPlayerManager.Instance != null)
//        {
//            CSPlayer player = CSPlayerManager.Instance.GetCSPlayerByAllocateIndex(changeWeapon.playerIndex);
//            if (player != null)
//            {
//                player.PlayerInfo.gunHandType = changeWeapon.targetWeapon;
//            }
//        }
//    }

//    void OnHeartBeat(Session session, Packet packet)
//    {
        
//    }

//    public void DisConnect()
//    {
//        if(client != null)
//        {
//            client.DisConnect();
//        }
//    }

//    private void ConnectSucHandler(Session session)
//    {
//        StartCoroutine(SendInitReq(session));
//    }

//    private IEnumerator SendInitReq(Session session)
//    {
//        yield return new WaitForSeconds(0);
//        InitReq initReq = new InitReq();
//        initReq.clientAddr = NetHelper.GetLocalIP();
//        Packet initPacket = Packet.CreatePacket(NetCode.INIT_C2S, initReq);
//        session.Send(initPacket);
//        Debug.Log("发送消息 init req ：" + initReq.clientAddr);
//    }

//    public void Send(Packet packet)
//    {
//        this.client.ClientSession.Send(packet);
//    }

//    public void ConnectToPhoton()
//    {
//        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
//        {
//            PhotonNetwork.ConnectUsingSettings("0.9");
//        }
//    }

//    public override void OnConnectedToMaster()
//    {
//        base.OnConnectedToMaster();
//        Debug.Log("OnConnectedToMaster");
//        OnFailRoom();
//    }

//    public override void OnCreatedRoom()
//    {
//        base.OnCreatedRoom();
//        Debug.Log("创建房間成功: " + curName + "," + PhotonNetwork.AuthValues.UserId);
//        ControlClient.Instance.Send(Packet.CreatePacket(NetCode.ENTER_PREPARE_C2S, null));
//    }

//    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
//    {
//        base.OnPhotonCreateRoomFailed(codeAndMsg);
//        Debug.Log("创建房間失败: " + curName + "," + PhotonNetwork.AuthValues.UserId);
//        OnFailRoom();
//    }

//    public override void OnJoinedRoom()
//    {
//        base.OnJoinedRoom();
//        Debug.Log("加入房間成功: " + curName + "," + PhotonNetwork.AuthValues.UserId);
//        ControlClient.Instance.Send(Packet.CreatePacket(NetCode.ENTER_PREPARE_C2S, null));
//    }

//    private void OnFailRoom()
//    {
//        // 进入准备状态需要进入准备场景，并且加入photon房间
//        PhotonNetwork.AuthValues = new AuthenticationValues() { UserId = PrepareData.Instance.SelfAllocateIndex.ToString() };
//        RoomOptions roomOptions = new RoomOptions();
//        roomOptions.MaxPlayers = 10;
//        // 我是quest 头盔端 我只加入 不创建
//        Debug.Log("开启加入房间");
//        PhotonNetwork.JoinRoom(curName);
//    }

//    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
//    {
//        base.OnPhotonJoinRoomFailed(codeAndMsg);
//        Debug.Log("加入房間失败: " + curName + "," + PhotonNetwork.AuthValues.UserId);
//        OnFailRoom();
//    }

//    public override void OnConnectedToPhoton()
//    {
//        base.OnConnectedToPhoton();
//        Debug.Log("OnConnectedToPhoton");
//    }

//    public override void OnConnectionFail(DisconnectCause cause)
//    {
//        base.OnConnectionFail(cause);
//        Debug.Log("OnConnectionFail ： " + cause);
//        this.StartCoroutine(PhotonReconnectDelay());
//    }

//    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
//    {
//        base.OnFailedToConnectToPhoton(cause);
//        Debug.Log("OnFailedToConnectToPhoton" + PhotonNetwork.connectionStateDetailed);
//        tryingToConnectPhoton = true;
//        //if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
//        //{
//        PhotonNetwork.Disconnect();
//        Debug.Log("PhotonNetwork.Disconnect()");
//        //}
//    }

//    public override void OnDisconnectedFromPhoton()
//    {
//        base.OnDisconnectedFromPhoton();
//        if (tryingToConnectPhoton)
//        {
//            // 500ms 之后重连，缓解压力
//            this.StartCoroutine(PhotonReconnectDelay());
//            tryingToConnectPhoton = false;
//        }
//    }

//    private IEnumerator PhotonReconnectDelay()
//    {
//        yield return new WaitForSeconds(0.5f);
//        PhotonNetwork.ConnectUsingSettings("0.9");
//    }

//    private void OnNext(Session session, Packet packet)
//    {
//        this.StartCoroutine(NextDelay());
//    }

//    private IEnumerator NextDelay()
//    {
//        yield return new WaitForSeconds(10.0f);
//        // 清空数据
//        GameInfo.Reset();
//        // 销毁所有网络连接
//        ControlClient.Instance.DisConnect();
//        // 销毁INIT组件
//        if (InitManager.Instance != null)
//        {
//            GameObject.DestroyImmediate(InitManager.Instance.gameObject);
//        }
//        // 加载InitScene
//        SceneManager.LoadScene("InitScene");
//        UnityEngine.Debug.Log("重新打开InitScene");
//    }

//    public IEnumerator StartAskServerIter()
//    {
//        BroadcastAskServer broadcastAskServer = this.gameObject.GetComponent<BroadcastAskServer>();
//        if(broadcastAskServer == null)
//        {
//            broadcastAskServer = this.gameObject.AddComponent<BroadcastAskServer>();
//            broadcastAskServer.Init();
//        }
//        int idx = 0;
//        while(string.IsNullOrEmpty(broadcastAskServer.ResultServerIp))
//        {
//            if(idx++ % 30 == 0)
//            {
//                broadcastAskServer.AskServer();
//            }
//            yield return null;
//        }

//        this.serverIp = broadcastAskServer.ResultServerIp;
//        this.Connect();
//        DebugLog.Log("recv serer ip : " + this.serverIp + "  start to connect ");
//    }

//    public void StartAskServer()
//    {
//        this.StartCoroutine(this.StartAskServerIter());
//    }
//}

