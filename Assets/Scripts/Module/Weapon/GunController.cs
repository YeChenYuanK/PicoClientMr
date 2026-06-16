using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using com.gamestudio.cs;
using Newtonsoft.Json;
using Mirror;
using static ParametersManager;
/// <summary>
/// 武器控制器.换枪等相关管理
/// </summary>
public class GunController : MonoBehaviour {

    public CSPlayer _player;
    public List<Transform> OtherGunTrans = new List<Transform>();
	public BaseGun currGun { get; private set; }
    private WeaponConfig config;
    public string gunid
    {
        get {
            return config.id.ToString();
        }
    }
    Transform gunTrans;
    public bool IsRepeating
    {
        get { return this.config != null && this.config.repeating; }
    }
    public BaseGun ChangeGunById(int gunId)
    {
        bool isOther = !_player.isLocalPlayer;
        if (gunId == 0)
        {
            Debug.Log("gun id is zero");
            return null;
        } 
        if (this.currGun != null)
        {
            GameObject.Destroy(this.currGun.gameObject);
        }
       
        this.config = ConfigHelper.GetWeaponCfg(gunId);
        string prefabPath = isOther ? "Prefabs/Guns/Other" + this.config.id : "Prefabs/Guns/" + this.config.id;
         gunTrans = GameObject.Instantiate ((GameObject)Resources.Load(prefabPath)).transform;
		
        if (!isOther)
        {
            gunTrans.parent = transform;

            _player.GetGunInfo(gunId.ToString() , _player.HandType);
         
         
            
        }
        else
        {

            gunTrans.parent = OtherGunTrans[_player.Camp];
            gunTrans.localPosition = Vector3.zero;
            gunTrans.localRotation = Quaternion.identity;
        }
      //  }
		currGun = gunTrans.GetComponent<BaseGun> ();
        currGun.InitWeapone(this.config,_player);
        return currGun;
	}
    public void SetPosAndRot(GunInfo config)
    {
        gunTrans.localPosition = new Vector3(config.Posx, config.Posy, config.Posz);
        gunTrans.localRotation = Quaternion.Euler(new Vector3(config.Rotx, config.Roty, config.Rotz));
    }
    public void OnChangeHandType()
    {
        
    }
    public void Kreload()
	{
		currGun.Kreload ();
	}
	public void SingleFire()
    {
    
        if (currGun == null) return;
        if (currGun.EffectPool == null) return;
        if (!currGun.IsRepeating)
		    currGun.Fire ();
	}
    public void Fire()
    {
      
        if (currGun == null) return;
        if (currGun.EffectPool == null) return;
        if (currGun.IsRepeating)
            currGun.Fire();
    }
    public void PlayFireEffectAndAudio()
    {
		if(currGun==null||currGun.EffectPool==null)return;
        currGun.PlayFireEffectAndAudio();
    }
    public void PlayFireHitEffectAndAudio(Quaternion triggerQuaternion, Vector3 HitPoint, TargetEffectType effectType)
    {
		if(currGun==null||currGun.EffectPool==null)return;
        currGun.PlayHitEffect(triggerQuaternion, HitPoint, effectType);
     
    }
    public void ReleaseTrigger()
    {
        if (currGun == null) return;
        currGun.ReleaseTriggerFlag = false;
    }

    public Transform FirePoint
    {
        get { return this.currGun.FirePoint; }
    }

   

}
