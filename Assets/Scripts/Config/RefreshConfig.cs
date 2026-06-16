using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class RefreshConfig : BaseConfig
{
	public	int	id;
	public	string	PointId;
	public	int	enemyID;
	public	int	startDate;
	public	float	delay;
	public	int	totalNum;
	public	int	PlayerCount;
	public	int	Voice;

    
    public RefreshConfig()
    {
    }
    
    public override void InitConfig(SqliteDataReader sdr)
    {
        base.InitConfig(sdr);
        
        	id	=	ReadInt("id");
	PointId	=	ReadString("PointId");
	enemyID	=	ReadInt("enemyID");
	startDate	=	ReadInt("startDate");
	delay	=	ReadFloat("delay");
	totalNum	=	ReadInt("totalNum");
	PlayerCount	=	ReadInt("PlayerCount");
	Voice	=	ReadInt("Voice");

        
    }

    public override int getId()
    {
        return id;
    }
    
  }