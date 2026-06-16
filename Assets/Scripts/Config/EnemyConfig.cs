using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class EnemyConfig : BaseConfig
{
	public	int	id;
	public	string	Name;
	public	int	SoldierType;
	public	string	PrefabsPath;
	public	string	MuzzleEffectPath;
	public	string	BulletPath;
	public	int	Hp;
	public	int	Damage;
	public	int	DamageRange;
	public	int	ContinuesShoot;
	public	float	ShootSpeed;
	public	int	Magazine;
	public	int	BasicHitRatio;
	public	float	MoveSpeed;
	public	int	RefreshTime;
	public	int	RefreshCD;
	public	int	RefreshNum;
	public	int	AttackMinCD;
	public	int	AttackMaxCD;
	public	int	TriggerRefreshId;
	public	int	EnterPriority;
	public	float	AttackRatio;
	public	float	AttackIncraseRatio;

    
    public EnemyConfig()
    {
    }
    
    public override void InitConfig(SqliteDataReader sdr)
    {
        base.InitConfig(sdr);
        
        	id	=	ReadInt("id");
	Name	=	ReadString("Name");
	SoldierType	=	ReadInt("SoldierType");
	PrefabsPath	=	ReadString("PrefabsPath");
	MuzzleEffectPath	=	ReadString("MuzzleEffectPath");
	BulletPath	=	ReadString("BulletPath");
	Hp	=	ReadInt("Hp");
	Damage	=	ReadInt("Damage");
	DamageRange	=	ReadInt("DamageRange");
	ContinuesShoot	=	ReadInt("ContinuesShoot");
	ShootSpeed	=	ReadFloat("ShootSpeed");
	Magazine	=	ReadInt("Magazine");
	BasicHitRatio	=	ReadInt("BasicHitRatio");
	MoveSpeed	=	ReadFloat("MoveSpeed");
	RefreshTime	=	ReadInt("RefreshTime");
	RefreshCD	=	ReadInt("RefreshCD");
	RefreshNum	=	ReadInt("RefreshNum");
	AttackMinCD	=	ReadInt("AttackMinCD");
	AttackMaxCD	=	ReadInt("AttackMaxCD");
	TriggerRefreshId	=	ReadInt("TriggerRefreshId");
	EnterPriority	=	ReadInt("EnterPriority");
	AttackRatio	=	ReadFloat("AttackRatio");
	AttackIncraseRatio	=	ReadFloat("AttackIncraseRatio");

        
    }

    public override int getId()
    {
        return id;
    }
    
  }