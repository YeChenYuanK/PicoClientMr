using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class SmokeConfig : BaseConfig
{
	public	int	id;
	public	int	RefreshTime;
	public	string	SmokeRefresh;
	public	int	SmokeSoldierNum;
	public	float	SoldierRefreshCD;
	public	float	SmokeTime;
	public	int	PlayerCount;

    
    public SmokeConfig()
    {
    }
    
    public override void InitConfig(SqliteDataReader sdr)
    {
        base.InitConfig(sdr);
        
        	id	=	ReadInt("id");
	RefreshTime	=	ReadInt("RefreshTime");
	SmokeRefresh	=	ReadString("SmokeRefresh");
	SmokeSoldierNum	=	ReadInt("SmokeSoldierNum");
	SoldierRefreshCD	=	ReadFloat("SoldierRefreshCD");
	SmokeTime	=	ReadFloat("SmokeTime");
	PlayerCount	=	ReadInt("PlayerCount");

        
    }

    public override int getId()
    {
        return id;
    }
    
  }