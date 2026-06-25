using UnityEngine;
using System.Collections;
using LekeNet;
using com.gamestudio.cs;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;
using static DataManager;

/// <summary>
/// Self character.监听各种输入和外部关联
/// </summary>
public class SelfCharacter : BaseCharacter
{
    public CSPlayer _player;
    public Transform selfTrans;

    public BleedBehavior bleedBehavior;
    public BleedBehaviorByDrawMesh bleedBehaviorByDrawMesh;
    public HurtArrow hurtArrow;
    public FollowVRCamera rebirthArrow;
    public GameObject lifeObj;

    [Header("幽灵光圈")]
    [Tooltip("美术做好的幽灵光圈特效 prefab")]
    [SerializeField] private GameObject ghostFxPrefab;
    [Tooltip("光圈挂载节点（留空则挂 selfTrans 下）")]
    [SerializeField] private Transform ghostFxParent;
    private GameObject spawnedGhostFx;

    private float lastHirtAudioTime;
    public bool isBreath { get; set; }
    public Foot CheckFoot;

  
    public  void ShowBleed(HurtDirection hurtDirect,float maxAlpha,float BloodAmout)
    {
       
        bleedBehavior.maxAlpha = maxAlpha;
        bleedBehavior.ChangeMaterial(hurtDirect);
        BleedBehavior.BloodAmount = BloodAmout;

    }
    public void CmdSetProtect(bool protect)
    {
        _player.SetCmdProtect(protect);
    }
    public void ShowProtect(bool protect)
    {
        if (!protect) return;
        BleedBehavior.BloodAmount = 0.0f;
       _player.CmdSetCurHp(SystemData.MAXHP);
       _player.PlayAudio(AudioType.AUDIO_RESURGENCE);
    }
    public void OnCurHpChange(int curHp)
    {
        if (curHp == SystemData.MAXHP) return;
        if (!isBreath)
        {
            // 播放声音
            float hp = curHp / (SystemData.MAXHP * 1.0f);
            if (hp < 0.3f)
            {
               _player.PlayAudio(_player.breathingAudio, true);
                isBreath = true;
            }
            else
            {
                if (lastHirtAudioTime < NetworkTime.time)
                {
                    AudioClip randomAudio = _player.hurtAudios[UnityEngine.Random.Range(0, 2)];

                    _player.PlayAudio(randomAudio, false);

                    lastHirtAudioTime = Time.time + SystemData.SelfHurtAudioCD;
                }
            }
        }
    }


    public void OnStartLocalPlayer ()
    {
        stateMachine.ChangeState(new CharacterPlayingState(this));
    }
   
       

    public override void Update()
    {
        stateMachine.SMUpdate();
    }
    public void OnDie()
    {
        this.isBreath = false;
        stateMachine.ChangeState(new CharacterDieState(this));
        ShowGhostFx();
    }

    private void ShowGhostFx()
    {
        if (ghostFxPrefab == null) return;
        HideGhostFx();
        Transform parent = ghostFxParent != null ? ghostFxParent : selfTrans;
        spawnedGhostFx = Instantiate(ghostFxPrefab, parent);
        spawnedGhostFx.transform.localPosition = Vector3.zero;
        spawnedGhostFx.transform.localRotation = Quaternion.identity;
    }

    private void HideGhostFx()
    {
        if (spawnedGhostFx != null)
        {
            Destroy(spawnedGhostFx);
            spawnedGhostFx = null;
        }
    }
   

    public void ChangeProtectedState()
    {
        stateMachine.ChangeState(new CharacterProtectedState(this));
    }
    public void ResetDeadInfo()
    {
        HideGhostFx();
    }
    public void ShowRebirthArrow(Transform rebirthPoint)
    {
        ObjectUtil.UpdateObjectActive(rebirthArrow.gameObject, true);
        rebirthArrow.TargetPoint = rebirthPoint;
    }

    public void HideRebirthArrow()
    {
        if (rebirthArrow.TargetPoint != null)
        {
            ObjectUtil.UpdateObjectActive(rebirthArrow.gameObject, false);
            rebirthArrow.TargetPoint = null;
        }
    }

    public void ShowLifeEffect()
    {
        if(_player != null)
        {
            Vector3 pos = _player.Body(BodyState.Body_Head).position;
            lifeObj.transform.position = new Vector3(pos.x, lifeObj.transform.position.y, pos.z);
        }
        ObjectUtil.UpdateObjectActive(lifeObj, true);
    }
    public void HideLifeEffect()
    {
        ObjectUtil.UpdateObjectActive(lifeObj, false);
    }

    /// <summary>
    /// 显示死亡倒计时
    /// </summary>
    /// <param name="leftSec">The left sec.</param>
    public void ShowDeadCount(int leftSec)
    {
		
    }

	
}
