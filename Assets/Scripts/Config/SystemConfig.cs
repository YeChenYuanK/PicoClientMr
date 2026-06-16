using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class SystemConfig : BaseConfig
{
	public	int	id;
	public	int	FootSoldierRefreshCd;
	public	int	FootSoldierRefreshNum;
	public	string	FootSoldierInitNum;
	public	string	SoldierNumMax;
	public	int	ProEnterSoldierNumMax;
	public	int	RebirthTime;
	public	int	ProtectTime;
	public	int	BossRefreshTime;

    
    public SystemConfig()
    {
    }
    
    public override void InitConfig(SqliteDataReader sdr)
    {
        base.InitConfig(sdr);
        
        	id	=	ReadInt("id");
	FootSoldierRefreshCd	=	ReadInt("FootSoldierRefreshCd");
	FootSoldierRefreshNum	=	ReadInt("FootSoldierRefreshNum");
	FootSoldierInitNum	=	ReadString("FootSoldierInitNum");
	SoldierNumMax	=	ReadString("SoldierNumMax");
	ProEnterSoldierNumMax	=	ReadInt("ProEnterSoldierNumMax");
	RebirthTime	=	ReadInt("RebirthTime");
	ProtectTime	=	ReadInt("ProtectTime");
	BossRefreshTime	=	ReadInt("BossRefreshTime");

        
    }

    public override int getId()
    {
        return id;
    }
    
  }