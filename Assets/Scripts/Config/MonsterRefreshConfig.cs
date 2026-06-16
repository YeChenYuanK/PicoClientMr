using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class MonsterRefreshConfig : BaseConfig
{
	public	int	id;
	public	int	TimePoint;
	public	string	FreshGroup;
	public	string	MonsterGroup;
	public	int	FreshNum;
	public	int	Wave;

    
    public MonsterRefreshConfig()
    {
    }
    
    public override void InitConfig(SqliteDataReader sdr)
    {
        base.InitConfig(sdr);
        
        	id	=	ReadInt("id");
	TimePoint	=	ReadInt("TimePoint");
	FreshGroup	=	ReadString("FreshGroup");
	MonsterGroup	=	ReadString("MonsterGroup");
	FreshNum	=	ReadInt("FreshNum");
	Wave	=	ReadInt("Wave");

        
    }

    public override int getId()
    {
        return id;
    }
    
  }