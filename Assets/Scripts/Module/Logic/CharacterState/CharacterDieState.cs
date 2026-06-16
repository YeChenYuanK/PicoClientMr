using com.leke.redSea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDieState : State
{
    protected SelfCharacter Player;
    public CharacterDieState(BaseCharacter target) : base(target)
    {
        Player = target as SelfCharacter;
    }

   

    public override void Enter()
    {
        base.Enter();
        ShowBirthPoints();
        Player.ShowBleed(HurtDirection.DEAD, 1f, int.MaxValue);
        Player._player.playerAudio.PlayOneShot(Player._player.rebirthAudio);
       
        if (BattleContext.Instance != null)
            Player.ShowRebirthArrow(BattleContext.Instance.FindNearRebirthPoinntByCamp(Player.bleedBehavior.transform, Player._player.Camp));
        Player.bleedBehaviorByDrawMesh.hurtType = BleedState.DEAD;
        if (GameSceneManager.Instance!=null)
        GameSceneManager.Instance.PartActive = false; 
    }
    public override void Update()
    {
        base.Update();
       
        CheckCharacterInBirthPoint();
      
    }

    public void HideBirthPoints()
    {
        if (Game.GameLogic.Instance != null)
        {
            Game.GameLogic.Instance.HideMapInfoObj();
        }
    }

    public void ShowBirthPoints()
    {
        if(Game.GameLogic.Instance != null)
        {
            Game.GameLogic.Instance.ShowMapInfoObj();
        }
    }
    public void CheckCharacterInBirthPoint()
    {
       Player.CheckFoot.Check();
        if (Player.CheckFoot.CheckPoint != null)
        {
            Player.HideRebirthArrow();
            Player.ShowLifeEffect();
            Player.CheckFoot.CheckPoint.ShowRebirthProgress(Player.CheckFoot.birthPointContinuedTime / SystemData.RebirthTime);
            if (Player.CheckFoot.birthPointContinuedTime >= SystemData.RebirthTime)
            {
                Player.HideLifeEffect();
                Player.CheckFoot.Clear();
                GameSceneManager.Instance.PartActive = true;
                //待定
                Player.ResetDeadInfo();

                Player.bleedBehaviorByDrawMesh.hurtType = BleedState.NORMAL;
                // 复活咯

                HideBirthPoints();
                Player.ChangeProtectedState();
                Player.HideRebirthArrow();

                //this.character.CheckFoot.CheckPoint.HideProgress();
            }
            // 需要显示复活进度
            //@TODO
        }
        else
        {
            //this.character.CheckFoot.CheckPoint.HideProgress();
            Player.ShowRebirthArrow(Player.rebirthArrow.TargetPoint);
            Player.HideLifeEffect();
        }
     
    }
}
