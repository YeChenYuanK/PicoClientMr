using com.leke;
using com.leke.redSea;
using LinNet;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverState : State
{
    private bool isShowWin = false;
    private double enterTime;
    private float cameraTime = 1.5f;

    public GameOverState(BaseGameEntity gameEntity) : base(gameEntity)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        this.enterTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(this.enterTime > 0 && NetworkTime.time >= (this.enterTime + cameraTime))
        {
            this.enterTime = 0;
          
        }
    }

}
