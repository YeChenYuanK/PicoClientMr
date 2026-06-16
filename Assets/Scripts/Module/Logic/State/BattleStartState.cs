using com.leke.redSea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartState : State {

    private float enterTime;
    private bool initRecord;

    public BattleStartState(BaseGameEntity gameEntity) : base(gameEntity)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 这里就是开始刷新兵的地方
        this.enterTime = Time.time;
        //  开始刷兵
    //    PhotonNetwork.room.CustomProperties["ServerStartTime"] = Game.GameLogic.ServerStartTime = PhotonNetwork.time;
        InitGameRecord();

        //Game.GameLogic gameLogic = GameObject.FindObjectOfType<Game.GameLogic>();
        //if(gameLogic != null)
        //{
        //    if(gameLogic.director != null)
        //    {
        //        gameLogic.director.Play();
        //      //  gameLogic.directorPlayTime = PhotonNetwork.time;
        //    } 
        //}
    }

    public override void Exit()
    {
        base.Exit();

    }

    public bool InitGameRecord()
    {
        // 初始化record的初始记录
        GameRecord record = GameMng.Instance._gameRecordMng;
        record.Clear();
        bool isFail = CSPlayerManager.Instance.AllPlayer.Count == 0;
        foreach (CSPlayer player in CSPlayerManager.Instance.AllPlayer)
        {

            PlayerRecord playerRecord = new PlayerRecord();
            playerRecord.Deads = 0;
            playerRecord.Kills = 0;
            playerRecord.winScore = 0;
            playerRecord.Index = player.playerId;
            playerRecord.Camp = player.Camp;
            playerRecord.name = player.PlayerName;
            record.gameRecordInfo.playerRecords.Add(playerRecord);
        }

        return !isFail;
    }

    public override void Update()
    {
        base.Update();

        //if(PhotonNetwork.time - Game.GameLogic.ServerStartTime >= PrepareData.Instance.GameTime)
        //{
        //    if(PhotonNetwork.isMasterClient)
        //    {
               
        //        //GameMng.Instance.ChangeStatePV("GameOverState");
        //    }
        //}
    }
}
