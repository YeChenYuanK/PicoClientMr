using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class WeaponConfig : BaseConfig
{
	public	int	id;
	public	string	name;
	public	int	price;
	public	int	shopIndex;
	public	int	normalHit;
	public	int	cirt;
	public	int	firingRate;
	public	float	recoil;
	public	int	recive;
	public	int	magazine;
	public	int	kreloadTime;
	public	bool	repeating;
	public	int	shotgunCount;

    
    public WeaponConfig()
    {
    }
    
    public override void InitConfig(SqliteDataReader sdr)
    {
        base.InitConfig(sdr);
        
        	id	=	ReadInt("id");
	name	=	ReadString("name");
	price	=	ReadInt("price");
	shopIndex	=	ReadInt("shopIndex");
	normalHit	=	ReadInt("normalHit");
	cirt	=	ReadInt("cirt");
	firingRate	=	ReadInt("firingRate");
	recoil	=	ReadFloat("recoil");
	recive	=	ReadInt("recive");
	magazine	=	ReadInt("magazine");
	kreloadTime	=	ReadInt("kreloadTime");
	repeating	=	ReadBool("repeating");
	shotgunCount	=	ReadInt("shotgunCount");

        
    }

    public override int getId()
    {
        return id;
    }
    
  }