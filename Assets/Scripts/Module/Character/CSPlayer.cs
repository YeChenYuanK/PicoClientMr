using System;
using System.Collections;
using System.Collections.Generic;
using com.gamestudio.cs;
using UnityEngine;
using com.leke.redSea;
using BNG;
using Mirror;
using static DataManager;

using System.Net;
using System.Linq;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static ParametersManager;
using Unity.XR.PXR;
public class CSPlayer : NetworkBehaviour
{


    public SelfCharacter selfCharacter;
    public OtherCharactor otherCharacter;

    public Transform selfTrans;
    public Transform OtherTrans;
    public Transform cameraPoint;

    public GunController gunController;
    public bool isDie => CurHp <= 0;
    [SyncVar]
    public int kills;

    [SyncVar]
    public int Combokills;
    [SyncVar]
    public int Dead;
    [SyncVar]
    public int shotHead;
    [SyncVar(hook = nameof(OnHightChange))]
    public float Hight;
    public void OnHightChange(float oldhight, float newhight)
    {

        if (!isLocalPlayer)
            otherCharacter.UpdateAnim(XAngle, newhight);
    }

    [SyncVar(hook = nameof(OnXAngleChange))]
    public float XAngle;
    public void OnXAngleChange(float OldXAngle, float NewXAngle)
    {

        if (!isLocalPlayer)
            otherCharacter.UpdateAnim(NewXAngle, Hight);
    }
    [Command]
    public void CmdSetXAngleAndHight(float Angle, float hight)
    {
        Hight = hight;
        XAngle = Angle;
        otherCharacter.UpdateAnim(Angle, hight);
    }
    [SyncVar(hook = nameof(OnProtectChange))]
    public bool IsProtect;
    public void OnProtectChange(bool oldprotect, bool newprotect)
    {
        if (isLocalPlayer)
            selfCharacter.ShowProtect(newprotect);
        else
            otherCharacter.ShowProtect(newprotect);
    }
    [Command]
    public void SetCmdProtect(bool protect)
    {
        IsProtect = protect;
        otherCharacter.ShowProtect(protect);
    }

    public GameObject CameraRig;


    public List<Transform> Bodys = new List<Transform>();
    public GunController _gunController { get; set; }

    public List<AudioClip> hurtAudios;
    public AudioClip rebirthAudio;
    public AudioClip breathingAudio;
    public AudioSource playerAudio;
    public AudioSource otherAudio;

    private void OnEnable()
    {
        Debug.Log($"[NET-DIAG] CSPlayer.OnEnable: netId={netId}, isLocalPlayer={isLocalPlayer}");
        GameMng.Instance._playerInfoMng.AddPlayer(this);

    }
    #region 同步
    [SyncVar(hook = nameof(OnPlayerIdChange))]
    public int playerId;
    public void OnPlayerIdChange(int oldid, int newid)
    {
        if (GameMng.Instance._staMng == null)
        {
            return;
        }
        if (GameMng.Instance.isGameState(0) && isLocalPlayer)
            GameMng.Instance._playerInfoMng.mySelfId = newid;

    }
    [Command]
    public void CmdChangeAnimator(bool isdie)
    {
        RpcChangeAnimator(isdie);
        otherCharacter.ChangeAnim(isdie);
    }
    [ClientRpc]
    public void RpcChangeAnimator(bool isdie)
    {
        otherCharacter.ChangeAnim(isdie);
    }
    public bool InitCamp;
    //队伍阵营
    [SyncVar(hook = nameof(OnPlayerCampChange))]
    public int Camp = -1;
    public void OnPlayerCampChange(int oldCamp, int newCamp)
    {

        otherCharacter.OnChangeCamp(newCamp);
        if (isLocalPlayer)
        {
            ApplyLocalCampUI(newCamp);
        }
       
    }   
    private void ApplyLocalCampUI(int camp)
    {
        GameMng.Instance._playerInfoMng.MySelfCamp = camp;
        GameMng.Instance._playerInfoMng.ChangeCamp(camp);
        if (GameMng.Instance.isGameState(0))
        {
            if (PrepareLogic.Instance != null)
            {
                PrepareLogic.Instance.ShowCamp(camp);
            }
        }
        else
        {
            if (BattleContext.Instance != null)
            {
                BattleContext.Instance.CurBattleField.ShowCamp(camp);
            }
        }
    }
    [ClientRpc]
    public void RpcBroadcastCampAndName(int camp, string playerName)
    {
        if (otherCharacter != null)
        {
            otherCharacter.OnChangeCamp(camp);
            otherCharacter.OnPlayerNameChange(playerName);
        }
        if (isLocalPlayer && GameMng.Instance != null && GameMng.Instance._playerInfoMng != null)
        {
            ApplyLocalCampUI(camp);
        }
    }
    [Command]
    public void CmdSetCamp(int camp)
    {
        Camp = camp;
        otherCharacter.OnChangeCamp(camp);
        RpcBroadcastCampAndName(Camp, PlayerName);

        //触发UI刷新
    }

    //玩家名字
    [SyncVar(hook = nameof(OnPlayerNameChange))]
    public string PlayerName = "";
    public void OnPlayerNameChange(string oldNmae, string newName)
    {
        otherCharacter.OnPlayerNameChange(newName);

    }
    [Command]
    public void CmdSetPlayerName(string name)
    {
        PlayerName = name;
        otherCharacter.OnPlayerNameChange(name);
        RpcBroadcastCampAndName(Camp, PlayerName);
    }

    [SyncVar(hook = nameof(OnPlayerStateChange))]
    public int PlayerState=2;
    public void OnPlayerStateChange(int oldstate, int newstate)
    {
       // CmdRefreshUI();
    }
    [Command]
    public void CmdSetPlayerState(int state)
    {
       
        PlayerState = state;
   
        GameMng.Instance._playerInfoMng.ChnageState(playerId, state);
        GameMng.Instance.RefreshUI?.Invoke();
    }

    //武器ID
    [SyncVar(hook = nameof(OnWeaponIdChange))]
    public int WeaponId;
    public void OnWeaponIdChange(int oldweaponid, int newWeaponid)
    {

        gunController.ChangeGunById(newWeaponid);
    }

    [Command]
    public void CmdSetWeaponid(int weaponid)
    {

        WeaponId = weaponid;
        gunController.ChangeGunById(weaponid);
        GameMng.Instance._playerInfoMng.ServerChangeWeaponId(playerId, weaponid);
        GameMng.Instance.RefreshUI?.Invoke();
    }

    [SyncVar(hook = nameof(OnCurHpChange))]
    public int CurHp;
    public void OnCurHpChange(int oldCurHp, int newCurHp)
    {
        if (isDie) return;
        //根据血量播放音效
        if (isLocalPlayer)
            selfCharacter.OnCurHpChange(newCurHp);
    }
    [Command]
    public void CmdSetCurHp(int Hp)
    {
        CurHp = Hp;
        GameMng.Instance._playerInfoMng.SetHp(playerId, CurHp);
    }
    [SyncVar(hook = nameof(OnHandTypeChange))]
    public int HandType;
    public void OnHandTypeChange(int oldType,int newType)
    {
        if (isLocalPlayer)
            GetGunInfo(gunController.gunid, newType);
    }
  
    [Command]
    public void GetGunInfo(string gunid,int Type)
    {
       GunInfo info= GameMng.Instance._parameterMng.GetGunInfo(gunid, Type);
        SetGunInfo(info);
    }
    [ClientRpc]
    public void SetGunInfo(GunInfo info)
    {
        if (isLocalPlayer && gunController != null)
        {
            gunController.SetPosAndRot(info);
        }
           
    }

    public Transform Body(BodyState state)
    {
        return Bodys[(int)state];
    }
    public XRController XRRightController;
    public Camera XRCamera;
    public Color XRColor;
    //初始化
    public override void OnStartLocalPlayer()
    {
        Debug.Log($"[NET-DIAG] OnStartLocalPlayer called: netId={netId}, connectionId={connectionToServer?.connectionId}");
        if (!isServer)
        {
            bool seeThroughManual = GameMng.Instance.isGameState(1);
            if (!seeThroughManual)
            {
                XRCamera.clearFlags = CameraClearFlags.SolidColor;
                XRCamera.backgroundColor = XRColor;
                Env.Instance.PlayRoomBGM();
            }
            else
            {
                Env.Instance.PlayGameStart();
                XRCamera.clearFlags = CameraClearFlags.Skybox;
            }
            PXR_Manager.EnableVideoSeeThrough = !seeThroughManual;
        }
        GameMng.Instance._playerInfoMng._mySelfPlayer = this;
        if (!GameMng.Instance.isClientTest)
        {
            CameraRig.SetActive(true);
            for (int i = 0; i < Bodys.Count; i++)
                Bodys[i].GetComponent<Follow>().enabled = true;
        }
        else
            Destroy(CameraRig);


        if (GameMng.Instance.isTest)
        {
            
            CmdInit(GameMng.Instance._playerInfoMng.mySelfId, 2,"-1");
        }
        else
        {
             CmdInit(GameMng.Instance._playerInfoMng.mySelfId, 2, GameMng.Instance.truegearConnectorMng.PicoSn);
        }
        


        CmdRefreshUI();
        selfCharacter.enabled = isLocalPlayer;
        otherCharacter.enabled = !isLocalPlayer;
        selfCharacter.OnStartLocalPlayer();
        selfTrans.gameObject.SetActive(true);
        GameMng.Instance.isFire = true;
        //Invoke("XRReady", 5F);

    }
    public LayerMask LAYER;
    public void XRReady()
    {
        XRCamera.clearFlags = CameraClearFlags.SolidColor;
        XRCamera.backgroundColor = XRColor;

        XRCamera.cullingMask = LAYER;
        PXR_Manager.EnableVideoSeeThrough=true;

    }

    [Command]
    public void CmdInit(int index,int state,string picosn)
    {
        Debug.Log($"[NET-DIAG] CmdInit called: index={index}, state={state}, picosn={picosn}, GameState={GameMng.Instance.isGameState(0)}, currentIndex={GameMng.Instance.index}");

        if (GameMng.Instance.isGameState(0) && index == -1)
        {
            int count = GameMng.Instance.index;
            Debug.Log($"[NET-DIAG] CmdInit assigning: count={count}, nextIndex={count + 1}");
            
            GameMng.Instance.index++;
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
            string name = UnityEngine.Random.Range(20, 99).ToString();
            for (int i = 0; i < GameMng.Instance._parameterMng._parameters.BloothConnectInfos.Count; i++)
            {
                if (GameMng.Instance._parameterMng._parameters.BloothConnectInfos[i].sn == picosn) {
                    name = GameMng.Instance._parameterMng._parameters.BloothConnectInfos[i].index.ToString();
                }
            }
             playerId = int.Parse(name);
          
            PlayerName = PlayerName + name;
            Camp = count % 2;
            otherCharacter.OnPlayerNameChange(PlayerName);
            otherCharacter.OnChangeCamp(Camp);
            HandType = GameMng.Instance._parameterMng._parameters.Type;
            WeaponId = 1001001;
            gunController.ChangeGunById(WeaponId);
            PlayerState = state;
            GameMng.Instance._playerInfoMng.ChnageState(playerId, state);
            GameMng.Instance.RefreshUI?.Invoke();
            PlayerInfo info = new PlayerInfo();
            info.Playerid = playerId;
            info.Camp = Camp;
            info.Weaponid = WeaponId;
            info.PlayerName = PlayerName;
            info.State = PlayerState;
            info.HandType = HandType;
            GameMng.Instance._playerInfoMng.AddInfos(playerId, info);
            Debug.Log("CSPLAYER====ID====" + playerId + "=====PLAYER======" + name);
            RpcBroadcastCampAndName(Camp, PlayerName);

        }
        else
        {
            Debug.LogWarning($"[NET-DIAG] CmdInit SKIPPED: isGameState(0)={GameMng.Instance.isGameState(0)}, index={index}");
            PlayerInfo Info = GameMng.Instance._playerInfoMng.GetPlayerInfoById(index);
            PlayerName = Info.PlayerName;
            otherCharacter.OnPlayerNameChange(PlayerName);
            playerId = Info.Playerid;
            
            Camp = Info.Camp;
            HandType = Info.HandType;
            otherCharacter.OnChangeCamp(Camp);
            WeaponId = Info.Weaponid;
            gunController.ChangeGunById(WeaponId);
            if (GameMng.Instance.isGameState(0))
                PlayerState = 0;
            else
                PlayerState = 1;

            GameMng.Instance._playerInfoMng.ChnageState(playerId, state);
            GameMng.Instance.RefreshUI?.Invoke();
            RpcBroadcastCampAndName(Camp, PlayerName);
        }
      
    }
  
    [ClientRpc]
    public void AddPlayerInfo(int id, PlayerInfo info)
    {

        GameMng.Instance._playerInfoMng.AddInfos(id, info);
    }

    string GetLocalIPAddress()
    {
        // 获取所有局域网IP地址
        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

        // 获取第一个IPv4地址
        IPAddress ipAddress = ips.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

        // 返回IPv4地址的字符串形式
        return ipAddress.ToString();
    }
    /// <summary>
    /// 添加玩家到人物管理器
    /// </summary>
    /// <param name="player"></param>
    [Command]
    public void CmdRefreshUI()
    {
        Debug.Log("调用");
        GameMng.Instance.RefreshUI?.Invoke();
    }


    #endregion

    bool wasTriggerPressed;
    public void Fire()
    {
        if (!GameMng.Instance.isFire) return;

        if (NetworkClient.isLoadingScene || NetworkServer.isLoadingScene) return;
        if (!NetworkClient.ready || NetworkClient.connection == null) return;
        if (!isDie)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {

                gunController.Fire();
            }
            if (XRRightController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
            {
                // 1. 单次按下（从 false → true）
                if (isTriggerPressed && !wasTriggerPressed)
                {
                    gunController.SingleFire();
                }
               
                // 2. 持续按住（isTriggerPressed 保持 true）
                if (isTriggerPressed)
                {
                    gunController.Fire();
                }

                // 3. 单次释放（从 true → false）
                if (!isTriggerPressed && wasTriggerPressed)
                {
                    gunController.ReleaseTrigger();
                }

                wasTriggerPressed = isTriggerPressed; // 更新状态
            }
          
           
        }
    }

    //所有端开枪特效 声音
    [ClientRpc]
    public void PlayFireEffectAndAudio()
    { //所有人开枪动画
		if(gunController==null||gunController.currGun==null)return;
		if(gunController.currGun.EffectPool==null)return;
        gunController.PlayFireEffectAndAudio();
    }
    [ClientRpc]
    public void PlayFireHitEffect(Quaternion triggerQuaternion, Vector3 HitPoint, TargetEffectType effectType)
    {
		if(gunController==null||gunController.currGun==null)return;
		if(gunController.currGun.EffectPool==null)return;
        gunController.PlayFireHitEffectAndAudio(triggerQuaternion, HitPoint, effectType);
    }
    public  bool IsHitFromFront(Vector3 hitPoint)
    {
        // 计算从目标中心指向命中点的方向（世界坐标）
        Vector3 directionToHit = (hitPoint - OtherTrans.transform.position).normalized;

        // 使用目标当前的forward方向进行点积计算
        float dotProduct = Vector3.Dot(OtherTrans.transform.forward, directionToHit);

        // 点积 > 0 表示在目标的前方，< 0 表示在后方
        return dotProduct > 0;
    }
    public LayerMask player;
    [Command]
    public void CmdFireSingle(Vector3 bulletDirection, Vector3 FirePos, int normalHit, int cirt)
    {
        if (NetworkServer.isLoadingScene) return;
        if (!GameMng.Instance.isFire) return;
        PlayFireEffectAndAudio();
        gunController.PlayFireEffectAndAudio();

        int targetId = 0;
        CharacterPart triggerPart = CharacterPart.NONE;
        Vector3 direct = Vector3.zero;
        TargetEffectType targetEffectType = TargetEffectType.none;
        RaycastHit hit;
        string partName = "";

        int layerMask = player;
     

        if (Physics.Raycast(FirePos, bulletDirection, out hit, 1000.0f, layerMask))
        {
            partName = hit.collider.name;
            // Debug.LogError(partName);
            direct = hit.point;
            //同步击中的全局坐标 : hit.point;
            BasePart part = hit.collider.GetComponent<BasePart>();
            Quaternion triggerQuaternion = new Quaternion(0, 0, 0, 0);
           
            //如果打中人,播放蹦血特效
            if (part != null)
            {
                CSPlayer targetPlayer = part.ParentCharacter.GetComponent<CSPlayer>();
                targetId = targetPlayer.playerId;
                if (targetPlayer.Camp == Camp)
                    return;
                if (targetPlayer.IsProtect)
                    return;
                if (targetPlayer.isDie)
                    return;
                if (!GameMng.Instance.isGameing)
                    return;
                //if (GameMng.Instance.isGameState(GameState.GAME_PREPARE))
                //    return;

                triggerQuaternion = Quaternion.LookRotation(hit.normal);
                triggerPart = part.partType;
                if (part.partType == CharacterPart.HEAD|| part.partType == CharacterPart.BODY)
                {
                    //头部中枪特效
                    targetEffectType = TargetEffectType.headBlood;
                    PlayFireHitEffect(triggerQuaternion, hit.transform.position, targetEffectType);
                    // gunController.PlayFireHitEffectAndAudio(triggerQuaternion, hit.transform.position, targetEffectType);
                }
             
                else
                {
                    Debug.LogError("命中目标身体部位类型没有选择！！！");
                }
                if (targetId != 0)
                {
                    CSPlayer target = GameMng.Instance._playerInfoMng.GetPlayerById(targetId);
                    target.DamageEnemy(targetId, playerId, (int)triggerPart, normalHit, cirt, direct, partName, this.transform.position);
                }
                //player.NoticeFire(targetId, triggerPart, weaponConfig.normalHit, weaponConfig.cirt, direct, partName);
            }
            else
            {
                Debug.Log("没打中人");
                //Debug.Log("Hit : " + hit.transform.name);
                //枪花
                triggerQuaternion = Quaternion.LookRotation(hit.normal);
                //PlayNoTargetEffect(triggerQuaternion, hit.point);
                if (hit.collider.tag == "Metal")
                {
                    targetEffectType = TargetEffectType.metalImpact;
                }
                else if (hit.collider.tag == "Dirt")
                {
                    targetEffectType = TargetEffectType.dritImpact;
                }
                else if (hit.collider.tag == "Wood")
                {
                    targetEffectType = TargetEffectType.woodImpact;
                }
                else
                {
                    targetEffectType = TargetEffectType.dritImpact;
                }
                PlayFireHitEffect(triggerQuaternion, hit.point, targetEffectType);
                gunController.PlayFireHitEffectAndAudio(triggerQuaternion, hit.point, targetEffectType);
            }

        }

    }
    public void PlayAudio(AudioClip clip = null, bool loop = false)
    {

        playerAudio.Stop();
        if (clip != null)
        {
            playerAudio.clip = clip;
            playerAudio.loop = loop;
            playerAudio.Play();
        }
    }
    public void TestMove()
    {
        float moveInput = Input.GetAxis("Vertical"); // 范围 [-1, 1]

        // 沿 Z 轴（前后方向）移动
        selfCharacter.transform.Translate(Vector3.forward * moveInput * 5 * Time.deltaTime);
        float rotateInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, rotateInput * 50 * Time.deltaTime);
    }
    float MaxY;
    float MinY;
    float waitTime;

    // ====== R3-5 下蹲优化: EMA平滑 + 迟滞阈值 + 0.3s防误触 ======
    private float _smoothedHeadY = 0f;                    // EMA平滑后的头部Y坐标
    private bool _headYInitialized = false;               // 平滑值是否已初始化
    private const float EMA_ALPHA = 0.15f;                // EMA平滑系数(越小越平滑,减少VR追踪抖动)
    private bool _isCrouchState = false;                  // 当前确认的蹲伏状态
    private bool _pendingCrouchState = false;              // 待确认的蹲伏状态(计时器未到)
    private float _crouchConfirmTimer = 0f;                // 状态确认计时器
    private const float CROUCH_ENTER_THRESHOLD = 0.35f;    // 进入蹲伏的hight阈值(低于此值触发)
    private const float CROUCH_EXIT_THRESHOLD = 0.55f;     // 退出蹲伏的hight阈值(高于此值触发,迟滞=0.2)
    private const float CROUCH_CONFIRM_DELAY = 0.3f;       // 防误触确认延迟(秒)
    private float _outputHight = 1f;                       // 输出用的平滑hight值(避免动画跳变)
   
    public void Update()
    {
       
        if (isLocalPlayer)
        {

            if (PlayerState == 2)
            {
                if (StateManager._instance != null)
                {
                    if (StateManager._instance.bloothCloth == 1)
                    {
                      
                        if (GameMng.Instance.truegearConnectorMng.isConnect)
                        {
                            
                            CmdSetPlayerState(0);
                        }
                    }
                    else
                    {
                      
                        CmdSetPlayerState(0);
                    }
                }
            }
            if (!GameMng.Instance.isGameState(1))
            {
                waitTime += Time.deltaTime;
                if (waitTime >= 1)
                {
                    XRReady();
                    waitTime = 0;

                }
                
            }
            //射击
            Fire();
            //TestMove();

            // R3-5 下蹲优化: EMA平滑 + 迟滞阈值 + 0.3s防误触
            Vector3 pos = Body(BodyState.Body_Head).position;
            if (pos.y > MaxY)
            {
                MaxY = pos.y;
            }
            MaxY = Mathf.Clamp(MaxY, 0.8f, 1.65f);

            // (1) EMA平滑头部高度,减少VR追踪抖动
            if (!_headYInitialized)
            {
                _smoothedHeadY = pos.y;
                _headYInitialized = true;
            }
            _smoothedHeadY += (pos.y - _smoothedHeadY) * EMA_ALPHA;

            MinY = MaxY * 0.7f;
            float rawHight = Mathf.Clamp((_smoothedHeadY - MinY) / (MaxY - MinY), 0, 1);

            // (2) 迟滞阈值: 进入/退出蹲伏使用不同阈值,避免边界反复切换
            bool detectCrouch = _isCrouchState
                ? rawHight < CROUCH_EXIT_THRESHOLD    // 已蹲下:需站起到0.55以上才退出
                : rawHight < CROUCH_ENTER_THRESHOLD;   // 站立中:需蹲到0.35以下才进入

            // (3) 0.3s防误触: 状态变化需持续确认后才生效
            if (detectCrouch != _pendingCrouchState)
            {
                _pendingCrouchState = detectCrouch;
                _crouchConfirmTimer = 0f;
            }
            else
            {
                _crouchConfirmTimer += Time.deltaTime;
            }

            if (_crouchConfirmTimer >= CROUCH_CONFIRM_DELAY)
            {
                _isCrouchState = _pendingCrouchState;
            }

            // 输出平滑hight值用于动画混合(避免状态切换时动画跳变)
            float targetHight = _isCrouchState ? Mathf.Clamp(rawHight, 0f, 0.5f) : 1f;
            _outputHight = Mathf.Lerp(_outputHight, targetHight, Time.deltaTime * 10f);

            float hight = _outputHight;

            Vector3 eulerAngle = Quaternion.LookRotation(Body(BodyState.Body_Right).forward).eulerAngles;

            CmdSetXAngleAndHight(eulerAngle.x + 15, hight);

            if (Camp != -1 && !InitCamp)
            {
                if (GameMng.Instance.isGameState(0))
                {
                    if (PrepareLogic.Instance != null)
                    {
                        PrepareLogic.Instance.ShowCamp(Camp);
                        InitCamp = true;
                    }
                }
            }

            
           
        }
        else
        {
            //模型位置
            Vector3 pos = Body(BodyState.Body_Head).transform.position;
            pos.y = 0;

            if ((OtherTrans.transform.position == Vector3.zero && OtherTrans.transform.position != pos)
              || Vector3.Distance(pos, OtherTrans.transform.position) > 0.5f)
            {
                OtherTrans.transform.position = pos;
            }
            if (isMove)
            {
                if (Vector3.Distance(OtherTrans.transform.position, pos) > 0.001f)
                {
                    OtherTrans.transform.position = Vector3.SmoothDamp(OtherTrans.transform.position, pos, ref moveVelocity, 0.1f, 1.2f * 1.412f);
                    Vector3 tempVector = transform.InverseTransformDirection(moveVelocity);

                    if (Vector3.Dot(new Vector3(tempVector.x, OtherTrans.transform.position.y, tempVector.z), OtherTrans.transform.forward) < 0)
                    {
                        tempVector.z = Mathf.Abs(tempVector.z);
                    }
                    otherCharacter.moveDirect = tempVector;
                }
                else
                {
                    otherCharacter.moveDirect = Vector3.zero;
                    isMove = false;
                }
            }
            else if (Vector3.Distance(OtherTrans.transform.position, pos) > 0.01f)
            {
                isMove = true;
            }
            //模型旋转
            float y = Body(BodyState.Body_Head).transform.rotation.eulerAngles.y;
          
            OtherTrans.transform.rotation = Quaternion.Euler(new Vector3(0, y, 0));
        }

    }
    private Vector3 OldOtherTranPos;
    private Vector3 moveVelocity;
    private bool isMove;
    //cmd
    public void DamageEnemy(int targetId, int shooterId, int part, int normalDamage, int critDamage, Vector3 direct, string partName, Vector3 firePos)
    {
        if (NetworkServer.isLoadingScene) return;
        //死亡
        if (isDie) return;
        int damage = 0;
        if ((CharacterPart)part == CharacterPart.HEAD)
            damage = critDamage;
        else
            damage = normalDamage;

        CurHp = Mathf.Max(0, CurHp - damage);
        GameMng.Instance._playerInfoMng.SetHp(playerId, CurHp);
        HitEffect(IsHitFromFront(direct));

        //服务器判断
        if (CurHp > 0)
            OnDamage(firePos, part);
        else
        {
            CSPlayer player = GameMng.Instance._playerInfoMng.GetPlayerById(shooterId);
            player.Combokills++;
            if ((CharacterPart)part == CharacterPart.HEAD)
                player.shotHead++;
            else
                player.kills++;
            Dead++;

            
            OnDie(part, shooterId);
            bool isFrist = GameMng.Instance._playerInfoMng.isFristblood();
            
            if (Env.Instance != null&&isFrist)
            {
                Env.Instance.PlayFirstBloodAudio(false);
            }
            bool TeamKill = GameMng.Instance._playerInfoMng.ChectkPlayerDie(Camp);
            player.shooter(isFrist, part, Combokills, TeamKill, player.Combokills);
            otherCharacter.ChangeAnim(true);
            BeShooter(isFrist,part,TeamKill, shooterId);
            Combokills = 0;
        }

    }
    [ClientRpc]
    public void HitEffect(bool ishit)
    {
        if (isLocalPlayer)
        {
            GameMng.Instance.truegearConnectorMng.OnBit(ishit);
        }
    }
    [ClientRpc]
    public void SpcPlayKillTeam(bool ifrist,bool isteamkill)
    {
        if (ifrist)
        {
            if (Env.Instance != null)
            {
                Env.Instance.PlayFirstBloodAudio(false);
            }
        }
        if (isteamkill)
        {
          
                if (Env.Instance != null)
                {
                    Env.Instance.PlayKillTeam(ifrist);
                }
            
        }
    }
    [ClientRpc]
    public void BeShooter(bool isFristBlood, int part,bool TeamKill,int shootid)
    {

        if (GameMng.Instance._mySelf.playerId != shootid)
        {
            //首杀音效
            if (isFristBlood)
            {
                if (Env.Instance != null)
                {
                    Env.Instance.PlayFirstBloodAudio(TeamKill);
                }
            }
            else
            {
                if (TeamKill)
                {
                    if (Env.Instance != null)
                    {
                        Env.Instance.PlayKillTeam(false);
                    }

                }
            }
        }

        if (isLocalPlayer)
        {
            this.playerAudio.Stop();
            this.playerAudio.clip = null;
            this.playerAudio.loop = false;

            selfCharacter.OnDie();
            //死亡音效
           float wait= PlayAudio(AudioType.AUDIO_DEATH_NORMAL);
            if (!isFristBlood)
            {
                if (Combokills > 2)
                {
                    if (Env.Instance != null)
                    {
                        Env.Instance.PlayBeFinishKill(wait);
                    }
                }
            }
        }
        else
        {
            otherCharacter.ShowDieFlag(part);
        }
    }
    [ClientRpc]
    public void shooter(bool isFristBlood,int part,int otherCombo,bool Teamkill,int comboKills)
    {
      
        if (isLocalPlayer)
        {
           
            if (isFristBlood)
            {
                if (Env.Instance != null)
                {
                    Env.Instance.PlayFirstBloodAudio(false);
                }
            }
            if (Teamkill)
            {
                if (comboKills > 1)
                {
                    CountKills(Teamkill, comboKills);
                }
                else
                {
                        if (Env.Instance != null)
                        {
                            Env.Instance.PlayKillTeam(isFristBlood);
                        }
                }
            }
            else
            {
                if (comboKills > 1)
                {
                    CountKills(Teamkill, comboKills);
                }
                else
                {
                    if (!isFristBlood)
                    {
                        float wait = 0;
                        //击杀音效
                        if ((CharacterPart)part == CharacterPart.HEAD)
                        {

                            wait = PlayAudio(AudioType.AUDIO_DEAD_HEAD);
                        }
                        else
                        {
                            wait = PlayAudio(AudioType.AUDIO_DEAD_NORMAL);
                        }
                        if (otherCombo > 2)
                        {
                            if (Env.Instance != null)
                            {
                                Env.Instance.PlayBeFinishKill(wait);
                            }
                        }
                    }
                }
            }
        }
      
    }
    public void CountKills(bool teamkill,int combokills)
    {
       
        AudioType audio = AudioType.NONE;
        if (combokills == 2)
        {
            audio = AudioType.AUDIO_DOUBLE_KILL;
        }
        else if (combokills == 3)
        {
            audio = AudioType.AUDIO_TRI_KILL;
        }
        else if (combokills == 4)
        {
            audio = AudioType.AUDIO_ULTRA_KILL;
        }
        else if (combokills == 5)
        {
            audio = AudioType.AUDIO_MONSTER_KILL;
        }
        else if (combokills == 6)
        {
            audio = AudioType.AUDIO_GOD_LIKE;
        }
        else if (combokills > 6)
        {
            audio = AudioType.AUDIO_RANMPAGE;
        }
      
    
        if (Env.Instance != null && Env.Instance.ContiansAudio(audio))
        {
            Env.Instance.PlayEnvEffect(audio, teamkill);
        }
        PlayAudioKill(audio,teamkill);
    }
    [Command]
    public void PlayAudioKill(AudioType audio,bool teamkill)
    {
        if (Env.Instance != null && Env.Instance.ContiansAudio(audio))
        {
            Env.Instance.PlayEnvEffect(audio, teamkill);
         
        }
    }
  
    //[ClientRpc]
    //public void CountDie(int part, int shooterId, bool isFristBlood)
    //{
    //    //首杀P1 


    //    CSPlayer shooter = GameMng.Instance._playerInfoMng.GetPlayerById(shooterId);

    //    if (!isLocalPlayer)
    //    { 
    //        if (shooter.playerId == playerId)
    //        {
    //            CountKills(part);

    //        }
    //}
    //        else
    //        {
          
               
    //            if (!isFristBlood)
    //        {
               
    //            ///P2 如果首杀 成立 就省略
    //            if ((CharacterPart)part == CharacterPart.HEAD)
    //                {
    //                    PlayAudio(AudioType.AUDIO_DEATH_HEAD);
    //                    shooter.PlayAudio(AudioType.AUDIO_DEAD_HEAD);
    //                }
    //                else
    //                {
    //                    PlayAudio(AudioType.AUDIO_DEATH_NORMAL);
    //                    shooter.PlayAudio(AudioType.AUDIO_HURT_NORMAL);
    //                    // Game.GameLogic.Instance.Record(shooterId, RecordType.KILL);
    //                }
    //                // Game.GameLogic.Instance.Record(playerId, RecordType.DEAD);
    //            }
    //        }
    //}
       


    public void OnDie(int part, int shooterId)
    {
        if ((CharacterPart)part == CharacterPart.HEAD)
        {
            GameMng.Instance.Record(shooterId, RecordType.HEADSHOT);
        }
        else
        {
            GameMng.Instance.Record(shooterId, RecordType.KILL);
        }
        GameMng.Instance.Record(playerId, RecordType.DEAD);
    }
    [Command]
    public void spcDie()
    {
        //死亡
        if (isDie) return;
      
        CurHp = 0;
        GameMng.Instance._playerInfoMng.SetHp(playerId, CurHp);
        //服务器判断
     
          
            Dead++;


        GameMng.Instance.Record(playerId, RecordType.DEAD);
        bool isFrist = GameMng.Instance._playerInfoMng.isFristblood();
       
            if (Env.Instance != null && isFrist)
            {
                Env.Instance.PlayFirstBloodAudio(false);
            }
            bool TeamKill = GameMng.Instance._playerInfoMng.ChectkPlayerDie(Camp);
    
            if (Env.Instance != null&&isFrist)
            {
                Env.Instance.PlayKillTeam(isFrist);
            }
        otherCharacter.ChangeAnim(true);
        BeShooter(isFrist, 0, TeamKill, -1);


         Combokills = 0;
        }
    

        [ClientRpc]
    public void OnDamage(Vector3 firePos, int part)
    {

        if (isLocalPlayer)
        {
            //Vector3 a = firePos - this.transform.position;
            //Vector3 b = Body(BodyState.Body_Head).forward;

            //Vector3 e = Vector3.Cross(b, firePos);
            //float angle = Vector3.Angle(a, b);
            //if (e.y > 0)
            //{
            //   selfCharacter.hurtArrow.ShowAngle(0 - angle);
            //}
            //else
            //{
            //// 左邊
            // selfCharacter.hurtArrow.ShowAngle(angle);
            //}

            selfCharacter.ShowBleed(HurtDirection.DEAD, 0.6f, 1f);
        }
        //所有端 这个玩家 播放
        if ((CharacterPart)part == CharacterPart.HEAD)
        {
            PlayAudio(AudioType.AUDIO_HURT_HEAD);
        }
        else
        {
            PlayAudio(AudioType.AUDIO_HURT_NORMAL);
        }
    }




    public float PlayAudio(AudioType audio)
    {
        this.playerAudio.loop = false;                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
        AudioType audioType = (AudioType)audio;
        AudioClip clip = null;
        switch (audioType)
        {                                   
            case AudioType.AUDIO_HURT_NORMAL:
                this.otherAudio.clip = hurtAudios[0];
                this.otherAudio.PlayOneShot(hurtAudios[0], 0.7f);
                clip = hurtAudios[0];
                break;
            case AudioType.AUDIO_HURT_HEAD:
                this.otherAudio.clip = hurtAudios[1];
                this.otherAudio.PlayOneShot(hurtAudios[1], 0.7f);
                clip = hurtAudios[1];
                break;
            case AudioType.AUDIO_DEATH_HEAD:
                this.otherAudio.clip = hurtAudios[3];
                this.otherAudio.PlayOneShot(hurtAudios[3], 0.7f);
                clip = hurtAudios[3];
                break;
            case AudioType.AUDIO_DEATH_NORMAL:
                this.otherAudio.clip = hurtAudios[3];
                this.otherAudio.PlayOneShot(hurtAudios[3], 0.7f);
                clip = hurtAudios[3];
                break;
        }
        if (isLocalPlayer)
        {
            switch (audioType)
            {
                case AudioType.AUDIO_DEAD_HEAD:
                    this.otherAudio.clip = hurtAudios[2];
                    this.otherAudio.PlayOneShot(hurtAudios[2], 0.7f);
                    clip = hurtAudios[2];
                    break;
                case AudioType.AUDIO_DEAD_NORMAL:
                    this.otherAudio.clip = hurtAudios[6];
                    this.otherAudio.PlayOneShot(hurtAudios[6], 0.7f);
                    clip = hurtAudios[6];
                    break;
                case AudioType.AUDIO_RESURGENCE:
                    this.otherAudio.clip = hurtAudios[4];
                    this.otherAudio.PlayOneShot(hurtAudios[4], 0.6f);
                    clip = hurtAudios[4];
                    break;
            }
        }
        return clip.length;
    }


}
