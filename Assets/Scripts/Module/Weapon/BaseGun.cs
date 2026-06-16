using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using PathologicalGames;
using System.Collections.Generic;
using BNG;
using Mirror;
using static DataManager;
using VRLeBigSpaceSDK;

public enum TargetEffectType
{
    none,
    brickImpact,
    dritImpact,
    metalImpact,
    helicopterImpact,
    bodyBlood,
    headBlood,
    woodImpact
}

public class BaseGun : BaseGameEntity {

	public int gunId;
	public Transform BulletPoint;
	public Transform FirePoint;

    public GameObject BulletEffect;
	public GameObject MuzzleEffect;
	public GameObject BrickImpact;
	public GameObject DritImpact;
	public GameObject MetalImpact;
    public GameObject HelicopterImpact;

    public GameObject BodyBlood;
	public GameObject HeadBlood;
	public TextMesh BulletNumUI;
	public AudioSource fireAudioSource;
    public AudioSource audioSource;
	public AudioClip fireAudio;
	public AudioClip emptyAudio;
	public AudioClip ReloadAudio;

    public Transform ppgunFireTransform;
    public Transform handGunFireTransform;

    protected WeaponConfig weaponConfig;

    public GameObject trueGunBody;
    public GameObject ghostGunBody;

    public GameObject FpsObj;

    //子弹数量、总数量
    protected int BulletNum;
    protected int BulletTotolNum;
    //换弹夹完成时间
    protected long kreloadOverTime;

    private Dictionary<Transform, float> effectDict = new Dictionary<Transform, float>();

    public SpawnPool EffectPool
	{
		get 
		{   
            if (PoolManager.Pools.ContainsKey("GunEffect"))
            {
                return PoolManager.Pools["GunEffect"];
            }
            return null;
         
		}
	}

    //射击当前振幅
    protected float currRecoil;

    protected long firingRate = 0;

    protected long lastFireTime = 0;

    public bool ReleaseTriggerFlag { get; set; }

    // ========== 掩体穿透检测 ==========
    [Header("掩体穿透检测")]
    public LayerMask barrierLayer;                  // Wall/Barrier 物理层（Inspector 配置）
    public float barrierEnvRadius = 0.15f;          // 第一级：枪口环境检测球半径
    public float barrierFrontRadius = 0.05f;        // 第二级：枪口前方检测球半径
    public float barrierFrontDist = 0.3f;           // 第二级：枪口前方检测距离
    public GameObject barrierBlockUI;               // "请后退" UI 提示（Inspector 拖入）

    private Collider[] _barrierBuf = new Collider[4]; // NonAlloc 缓存
    private bool _barrierBlocked;

    public bool IsRepeating
	{
		get
		{
			return weaponConfig.repeating;
		}
	}

	public long KreloadTime
	{
		get 
		{
			return weaponConfig.kreloadTime;
		}
	}

    public GameObject handGun;
    public GameObject ppGun;

    public GameObject gunModel;
    public GameObject shadowGunModel;
    private CSPlayer _player;
	void Start () 
	{
        if(FpsObj != null) FpsObj.SetActive(false);
        ChangeState (new NormalState(this));
	    
        if(this.gunModel != null)
        {
            this.gunModel.transform.SetParent(null);
        }
    }

    private bool isReloading;

   
	// Update is called once per frame
	public override void Update () {
        base.Update();
		stateMachine.SMUpdate ();

        //更新子弹UI
        if (weaponConfig != null && BulletNumUI != null)
        {
            BulletNumUI.text = BulletNum.ToString() + "/" + weaponConfig.magazine;
        }

        float flag = Quaternion.LookRotation(FirePoint.transform.forward).eulerAngles.x;

        //Kreload 换子弹
        if (flag > 180)
        {
            flag = 360 - flag;
        }
        if (flag >= 40)
        {
            if(!isReloading)
            {
                Kreload();
            }
            isReloading = true;
        } else
        {
            isReloading = false;
        }
        
        if (FpsObj != null && GameMng.Instance != null && SystemData.AllowGunFpsShow
            && InputBridge.Instance != null && InputBridge.Instance.BButtonUp)
        {
            FpsObj.SetActive(!FpsObj.activeSelf);
        }
        
        
    }
    private Vector3 tempVector;
    private void LateUpdate()
    {
        Debug.DrawRay(FirePoint.position, FirePoint.forward * 1000, Color.red);
        if (this.gunModel != null && this.shadowGunModel != null)
        {
            this.gunModel.transform.position = this.shadowGunModel.transform.position;
            this.gunModel.transform.rotation = this.shadowGunModel.transform.rotation;
        }
    }

    public void InitWeapone(WeaponConfig weaponConfig,CSPlayer player)
	{
        _player = player;
		this.weaponConfig = weaponConfig;
		firingRate = 1000 / weaponConfig.firingRate;
		BulletNum = weaponConfig.magazine;


	}
    public void PlayFireEffectAndAudio()
    {
      PlayFireAudio();
        if (GameMng.Instance.isGameState(0)&&!GameMng.Instance.isServerHost)
            return;
       PlayMuzzleEffect();
    }
    /// <summary>
    /// 换弹夹.
    /// </summary>
    public void Kreload()
	{
        if (weaponConfig == null) return;
		if(GetCurrStateType() == typeof(KreloadState) || BulletNum == weaponConfig.magazine)
		{
			return;
		}
		ChangeState (new KreloadState(this));
        BulletNull = false;
        if (audioSource == null || ReloadAudio == null) return;
		audioSource.PlayOneShot (ReloadAudio);
	}

	public void KreloadOver()
	{
		BulletNum = weaponConfig.magazine;
        BulletNumUI.text = BulletNum.ToString() + "/" + weaponConfig.magazine;
    }
    private bool BulletNull;
   
    /// <summary>
    /// 掩体穿透三级检测。
    /// 第一级 OverlapSphere（大口径）：检测枪口是否已在掩体内部。
    /// 第二级 SphereCast（小口径）：检测枪口前方极近处是否有掩体。
    /// </summary>
    /// <returns>true = 被掩体阻挡，应禁止开火</returns>
    private bool CheckBarrierPenetration(Vector3 muzzlePos, Vector3 fireDir)
    {
        // —— 第一级：枪口环境检测（大口径，检查枪口是否已穿透掩体）——
        int hitCount = Physics.OverlapSphereNonAlloc(
            muzzlePos, barrierEnvRadius, _barrierBuf, barrierLayer);

        if (hitCount > 0)
            return true;

        // —— 第二级：枪口前方短距离检测（小口径，检查枪口即将穿透）——
        if (Physics.SphereCast(muzzlePos, barrierFrontRadius, fireDir,
                               out RaycastHit _, barrierFrontDist, barrierLayer))
            return true;

        return false;
    }

	public void Fire()
	{
        if (_player == null) return;
        if (_player.isDie) return;
		if (this.weaponConfig.shotgunCount > 1)
	    {
            // 是喷子 原则必须要先松开扳机再发射 为了防止出现恶性bug 超出时间也可以发射
	        if (this.ReleaseTriggerFlag && (lastFireTime != 0 && lastFireTime > DateUtil.NowMllSec))
	        {
	            return;
	        }
	    }
        //射击速率
        if (lastFireTime != 0 && lastFireTime > DateUtil.NowMllSec) return;
        lastFireTime = DateUtil.NowMllSec + firingRate;

        // ★ 掩体穿透检测（三级：OverlapSphere → SphereCast）
        if (CheckBarrierPenetration(FirePoint.position, FirePoint.forward))
        {
            if (barrierBlockUI != null && !_barrierBlocked)
            {
                barrierBlockUI.SetActive(true);
                _barrierBlocked = true;
            }
            return;
        }

        // 解除阻挡状态
        if (_barrierBlocked)
        {
            _barrierBlocked = false;
            if (barrierBlockUI != null) barrierBlockUI.SetActive(false);
        }

        //没用子弹或者在换弹夹过程中
        if (BulletNum <= 0)
		{
           
            //@@播放卡壳的声音
            audioSource.PlayOneShot (emptyAudio);
			return;
		}
        if (GetCurrStateType() == typeof(KreloadState)) return;

	    this.ReleaseTriggerFlag = true;
        BulletNum--;

        //if (BulletNumUI != null)
        //{
        //    //更新子弹UI
        //    BulletNumUI.text = BulletNum.ToString() + "/" + weaponConfig.magazine;
        //}

       
        //animator.CrossFade ("Shoot",0);
        try
        {
            VRLeGun.Send(this.weaponConfig.name);
        }
        catch (System.Exception e)
        {
            // 外设 SDK 异常不应影响射击主逻辑。
            Debug.LogWarning("VRLeGun.Send failed: " + e);
        }

        if (this.weaponConfig.shotgunCount > 1)
	    {
	        for (int i = 0; i < this.weaponConfig.shotgunCount; i++)
	        {
                _player.CmdFireSingle(FirePoint.forward + new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), Random.Range(0, 0.1f)), FirePoint.position,weaponConfig.normalHit,weaponConfig.cirt);
                // 模拟散弹枪射击
               //. this.CmdFireSingle(FirePoint.forward + new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f), Random.Range(0,0.1f)), player.playerId);
	        }
	    }
	    else
	    {
            _player.CmdFireSingle(FirePoint.forward, FirePoint.position,weaponConfig.normalHit,weaponConfig.cirt);
        }
    }

   

    public void OtherFire()
    {

    }

	public void PlayFireAudio()
	{
        fireAudioSource.volume = 0.5f;
        fireAudioSource.PlayOneShot (fireAudio,0.5f);
	}

  

    /// <summary>
    /// 枪口特效
    /// </summary>

 

    public void PlayMuzzleEffect()
	{
        if (EffectPool == null) return;
		//播放枪口特效
		GameObject muzzleInstance = EffectPool.Spawn (MuzzleEffect.transform , FirePoint.position , FirePoint.rotation).gameObject;
		muzzleInstance.SetActive (true);

        effectDict[muzzleInstance.transform] = Time.time;
    }




    //击中特效
   
    public void PlayHitEffect(Quaternion triggerQuaternion, Vector3 HitPoint, TargetEffectType effectType)
    {
        GameObject effectInstance = null;
        switch (effectType)
        {
            case TargetEffectType.bodyBlood:
                effectInstance = EffectPool.Spawn(BodyBlood.transform, HitPoint, triggerQuaternion).gameObject;
                break;
            case TargetEffectType.headBlood:
                effectInstance = EffectPool.Spawn(HeadBlood.transform, HitPoint, triggerQuaternion).gameObject;
                break;
            case TargetEffectType.brickImpact:
                effectInstance = EffectPool.Spawn(BrickImpact.transform, HitPoint, triggerQuaternion).gameObject;
                break;
            case TargetEffectType.dritImpact:
                effectInstance = EffectPool.Spawn(DritImpact.transform, HitPoint, triggerQuaternion).gameObject;
                break;
            case TargetEffectType.metalImpact:
                effectInstance = EffectPool.Spawn(MetalImpact.transform, HitPoint, triggerQuaternion).gameObject;
                break;
            case TargetEffectType.helicopterImpact:
                effectInstance = EffectPool.Spawn(HelicopterImpact.transform, HitPoint, triggerQuaternion).gameObject;
                break;
            case TargetEffectType.woodImpact:
                effectInstance = EffectPool.Spawn(BrickImpact.transform, HitPoint, triggerQuaternion).gameObject;
                break;
            default:
                Debug.LogError("目标特效类型错误！！！");
                break;
        }
        ObjectUtil.UpdateObjectActive(effectInstance , true);
    }
    
   

    

    private void OnDestroy()
    {
        if(this.gunModel != null)
        {
            GameObject.DestroyImmediate(this.gunModel);
        }
    }

}
