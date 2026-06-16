using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrepareState : CSCharacterState {

    public CharacterPrepareState(BaseCharacter player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //this.Player.selfCharacter.CheckFoot.Check();
        //if (this.Player.selfCharacter.CheckFoot.CheckPoint != null)
        //{
        //    // 已经站在出生点。
        //    Player.PlayerInfo.GameReady = true;
        //}
    }

}
