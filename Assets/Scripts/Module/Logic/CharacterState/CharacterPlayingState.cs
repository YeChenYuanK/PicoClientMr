using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayingState : State
{
    protected SelfCharacter Player;
    public CharacterPlayingState(BaseCharacter target) : base(target)
    {
        Player = target as SelfCharacter;
    }

    public override void Enter()
    {
        base.Enter();
        BleedBehavior.BloodAmount = 0.0f;
        Player._player.CmdSetCurHp(SystemData.MAXHP);
   
        Player._player.CmdChangeAnimator(false);
    }

    public override void Update()
    {
        base.Update();


    }



}
