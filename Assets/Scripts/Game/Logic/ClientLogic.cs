using UnityEngine;
using System.Collections;
using LekeNet.Room;
using LekeNet;
using com.gamestudio.tank;
using System;
using LekeNet.Util;
using UnityEngine.SceneManagement;
using com.gamestudio.cs;
using System.Collections.Generic;
using com.gamestudio.room;

public class ClientLogic 
{

    private static ClientLogic instance; 

    public static ClientLogic Instance
    {
        get 
		{
            if(instance == null)
            {
                instance = new ClientLogic();
            }

            return instance;
        }
    }

   
    public ClientLogic()
    {
        fightManager = new FightManager();
    }

    private RoomClient roomClient;
    private FightManager fightManager;

    private LekeNet.Room.Room room;
    public LekeNet.Room.Room Room
    {
        get 
		{
            return this.room;
        }
    }

    public FightManager FightManager { get { return this.fightManager; } }

    public RoomClient RoomClient
    {
        get
        {
            return roomClient;
        }

        set
        {
            roomClient = value;
        }
    }

    private OperationTigger operTrigger = new OperationTigger();

    public void Init(RoomClient roomClient)
    {
        RegisterHandlers();
        this.roomClient = roomClient;
        this.roomClient.AddEventListener(RoomEvent.ROOM_JOIN, new EventDispatcher.EventHandler(OnRoomJoinSuc));
        this.roomClient.AddEventListener(RoomEvent.ROOM_MEMBER_ADD, new EventDispatcher.EventHandler(OnMemAdd));
        this.roomClient.AddEventListener(RoomEvent.ROOM_MEMBER_LEAVE, new EventDispatcher.EventHandler(OnMemLeave));
        this.roomClient.AddEventListener(RoomEvent.LOST_SERVER_CONNECTION, new EventDispatcher.EventHandler(OnLostServerConn));
        this.roomClient.AddEventListener(RoomEvent.ROOM_STATUS_CHANGE, new EventDispatcher.EventHandler(OnRoomStatusChange));
        this.roomClient.AddEventListener(RoomEvent.OPERATION, new EventDispatcher.EventHandler(OnOperation));
        this.roomClient.AddEventListener(RoomEvent.SYS_OPERATION, new EventDispatcher.EventHandler(OnSysOperation));
        this.roomClient.AddEventListener(RoomEvent.KICK_OFF, new EventDispatcher.EventHandler(OnKickOff));
    }


    private void RegisterHandlers()
    {
        operTrigger.AddTrigger(new PlayerSelfInitHandler());
        operTrigger.AddTrigger(new PlayerOtherInitHandler());
        operTrigger.AddTrigger(new PlayerOthersInitHandler());
        operTrigger.AddTrigger(new FightUnitSyncHandler());
        operTrigger.AddTrigger(new MemberLeaveHandler());
        operTrigger.AddTrigger(new DriverSyncHandler());
        operTrigger.AddTrigger(new TeleporterSyncHandler());
        operTrigger.AddTrigger(new FireHandler());
        operTrigger.AddTrigger(new HarmHandler());
        operTrigger.AddTrigger(new DeadHandler());
        operTrigger.AddTrigger(new RebirthHandler());
        operTrigger.AddTrigger(new ScoreGetHandler());
        operTrigger.AddTrigger(new TeleporterInitHandler());
        operTrigger.AddTrigger(new PlayerMoveSyncHandler());
		operTrigger.AddTrigger(new GunSynHandler());
        operTrigger.AddTrigger(new BattleSettleOverHandler());
		operTrigger.AddTrigger (new MovebleNodeSyncHandler());
        

        operTrigger.AddSysOperTrigger(new AssignClientHostHandler());
        operTrigger.AddSysOperTrigger(new ServerTimeSyncHandler());
    }

    // 本机玩家加入房间成功之后，需要调用选择机器人请求
    private void OnRoomJoinSuc(LekeNet.Event ev)
    {
        this.room = (LekeNet.Room.Room)ev.Data;
    }

    private void OnMemAdd(LekeNet.Event ev)
    {
        RoomMember roomMember = (RoomMember)ev.Data;
        this.Room.AddMember(roomMember);
        Debug.Log("玩家" + roomMember.playerName + "加入.");
    }

    /// <summary>
    /// 处理游戏操作
    /// </summary>
    /// <param name="ev">The ev.</param>
    private void OnOperation(LekeNet.Event ev)
    {
        RoomOperation operation = (RoomOperation)ev.Data;
        operTrigger.OnTrigger(operation);
    }

    /// <summary>
    /// 处理系统操作
    /// </summary>
    /// <param name="ev">The ev.</param>
    private void OnSysOperation(LekeNet.Event ev)
    {
        RoomOperation operation = (RoomOperation)ev.Data;
        operTrigger.OnSysOperTrigger(operation);
    }

    /// <summary>
    /// 处理玩家下线
    /// </summary>
    /// <param name="ev"></param>
    private void OnKickOff(LekeNet.Event ev)
    {
        Debug.Log("被踢出房间");
        //this.Destory();
        //MainFrameCall.Instance.AddCall(OnChangeScene, null);
    }

    private System.Object OnChangeScene(object obj)
    {
        SceneManager.LoadScene("MainSence");
        return null;
    }

    // 处理某个成员下线
    private void OnMemLeave(LekeNet.Event ev)
    {
        RoomMember roomMember = ev.Data as RoomMember;
        Debug.Log("玩家" + roomMember.playerName + "离开...");
    }

    private void OnLostServerConn(LekeNet.Event ev)
    {
        Debug.Log("与服务器断开连接");
    }

    private void OnRoomStatusChange(LekeNet.Event ev)
    {
            // 加载场景
        ChangeScene(null);
    }

    private System.Object ChangeScene(System.Object para)
    {
        SceneManager.LoadScene("Start");
        return null;
    }

    public void NoticeSceneLoadComplete()
    {
        this.roomClient.PushOperation((int)GameOper.MAP_LOAD_SUC, null);
    }

    /// <summary>
    /// 瞬间移动
    /// </summary>
    /// <param name="index">The index.</param>
	public void InstantMove(int index, Vector3 currPoint, Vector3 targetPoint )
	{
        PlayerInstantMove instantMove = new PlayerInstantMove();
        instantMove.targetIndex = index;
        //instantMove.playerId = PlayerInfo.RoomUnitId;
		instantMove.currentPos = ProtoHelper.ConvertToProto (currPoint);
        instantMove.targetPoint = ProtoHelper.ConvertToProto(targetPoint);
        this.roomClient.PushOperation((int)GameOper.PLAYER_INSTANT_MOVE, instantMove);
    }

	public void InstantChangeGun(int unitid , int weaponeId)
	{
		MChangeWeapon changeWeapone = new MChangeWeapon ();
		changeWeapone.unitid = unitid;
		changeWeapone.weaponid = weaponeId;
		this.roomClient.PushOperation ((int)GameOper.CHANGE_WEAPON , changeWeapone);
	}

    /// <summary>
    /// Pushes the synchronize fight unit.
    /// </summary>
    /// <param name="unitId">The unit identifier.</param>
    public void PushSyncFightUnit(int unitId)
    {
        FightUnit fightUnit = this.fightManager.GetFightUnit(unitId);
        if(fightUnit != null)
        {
            this.roomClient.PushOperation((int)GameOper.FIGHT_UNIT_SYNC, fightUnit.ToProto());
        }
    }

    public void Destory()
    {
        if(this.roomClient != null)
        {
            this.roomClient.Destory();
        }
    }

    public void Fire(int targetId, Vector3 point, Quaternion triggerRotation, CharacterPart triggerPart)
    {
        FireInfo fireInfo = new FireInfo();
        fireInfo.targetUnitId = targetId;
        fireInfo.triggerPos = ProtoHelper.ConvertToProto(point);
        //fireInfo.shooterId = PlayerInfo.RoomUnitId;
        fireInfo.triggerRotation = ProtoHelper.ConvertToProto(triggerRotation);
        fireInfo.triggerPart = triggerPart;
        this.roomClient.PushOperation((int)GameOper.FIRE, fireInfo);
    }

   
}