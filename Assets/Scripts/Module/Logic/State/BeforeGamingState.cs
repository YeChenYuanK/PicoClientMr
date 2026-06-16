using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeGamingState : State
{
    public BeforeGamingState(BaseGameEntity gameEntity) : base(gameEntity)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // 现在测试阶段直接进入BattleStart 不需要等待所有人走到出生点
       // GameMng.Instance.ChangeStatePV("BattleStartState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

}
