using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class HelicopterConfig : BaseConfig
{
	public	int	id;
	public	string	Name;
	public	int	WaitTime;
	public	int	NormalHit;
	public	int	BeAttackValue;
	public	int	Dirct1WaitTime;
	public	int	Dirct1Weapon;
	public	int	Dirct2WaitTime;
	public	int	Dirct2Weapon;
	public	int	Dirct3WaitTime;
	public	int	Dirct3Weapon;
	public	int	FrontGunHP;
	public	int	FrontGunDamage;
	public	int	FrontGunDelay;
	public	int	OtherGunHP;
	public	int	OtherGunDamage;
	public	int	OtherGunDelay;
	public	int	RocketHP;
	public	int	RocketDamage;
	public	int	RocketDamageDelay;
	public	int	BodyHP;
	public	int	BoomSelfTime;
	public	int	BoomSelfDamage;
	public	int	PlayerCount;

    
    public HelicopterConfig()
    {
    }
    
    public override void InitConfig(SqliteDataReader sdr)
    {
        base.InitConfig(sdr);
        
        	id	=	ReadInt("id");
	Name	=	ReadString("Name");
	WaitTime	=	ReadInt("WaitTime");
	NormalHit	=	ReadInt("NormalHit");
	BeAttackValue	=	ReadInt("BeAttackValue");
	Dirct1WaitTime	=	ReadInt("Dirct1WaitTime");
	Dirct1Weapon	=	ReadInt("Dirct1Weapon");
	Dirct2WaitTime	=	ReadInt("Dirct2WaitTime");
	Dirct2Weapon	=	ReadInt("Dirct2Weapon");
	Dirct3WaitTime	=	ReadInt("Dirct3WaitTime");
	Dirct3Weapon	=	ReadInt("Dirct3Weapon");
	FrontGunHP	=	ReadInt("FrontGunHP");
	FrontGunDamage	=	ReadInt("FrontGunDamage");
	FrontGunDelay	=	ReadInt("FrontGunDelay");
	OtherGunHP	=	ReadInt("OtherGunHP");
	OtherGunDamage	=	ReadInt("OtherGunDamage");
	OtherGunDelay	=	ReadInt("OtherGunDelay");
	RocketHP	=	ReadInt("RocketHP");
	RocketDamage	=	ReadInt("RocketDamage");
	RocketDamageDelay	=	ReadInt("RocketDamageDelay");
	BodyHP	=	ReadInt("BodyHP");
	BoomSelfTime	=	ReadInt("BoomSelfTime");
	BoomSelfDamage	=	ReadInt("BoomSelfDamage");
	PlayerCount	=	ReadInt("PlayerCount");

        
    }

    public override int getId()
    {
        return id;
    }
    
  }