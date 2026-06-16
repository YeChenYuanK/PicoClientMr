using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保护状态
/// </summary>
public class CharacterProtectedState : State
{
    private float enterTime;
    protected SelfCharacter Player;
    public CharacterProtectedState(BaseCharacter target) : base(target)
    {
        Player = target as SelfCharacter;
    }

    public override void Enter()
    {
        base.Enter();
        Player.CmdSetProtect(true);
        enterTime = Time.time;
      
    }

    public override void Update()
    {
        base.Update();
        if(enterTime > 0)
        {
            if(Time.time - enterTime >= SystemData.ProtectTime) 
            {
                this.Player.ChangeState(typeof(CharacterPlayingState));
                enterTime = 0;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        Player.CmdSetProtect(false);
       
    }
}
