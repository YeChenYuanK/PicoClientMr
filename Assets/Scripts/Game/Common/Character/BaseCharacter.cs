using UnityEngine;
using System.Collections;
using System;
using com.gamestudio.cs;

public class BaseCharacter : BaseGameEntity {

    public int unitId = 0;
	

	

    public void Destory()
    {
        GameObject.Destroy(this.gameObject);
    }

    // Use this for initialization
   
    //
    public virtual void Fire(FireInfo fireInfo)
    {
        // Debug.Log("Fire unit : " + unitId);
    }

    /// <summary>
    /// 受伤害
    /// </summary>
    public virtual void Hurt(int shooterId, Vector3 triggerPos, Quaternion triggerQuaternion, CharacterPart triggerPart)
    {
        //Debug.Log("Hurt unit : " + unitId);
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    public virtual void Dead(FightUnit fightUnit)
    {
        //Debug.Log("dead unit : " + unitId);
    }

  
   
    /// <summary>
    /// 角色复活
    /// </summary>
    /// <param name="fightUnit">The fight unit.</param>
    public virtual void Rebirth(FightUnit fightUnit)
    {
       
    }

	public virtual void GameOver(MBattleSettle battleSettle)
	{
		
	}

	/// <summary>
	/// Changes the weapon.
	/// </summary>
	/// <param name="weapone">Weapone.</param>
	public virtual void ChangeWeapon(MChangeWeapon weapone)
	{
		
	}

}
