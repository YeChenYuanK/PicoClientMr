using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class MonsterConfig : BaseConfig
{
	public	int	id;
	public	string	Name;
	public	int	resource;
	public	int	AIType;
	public	int	MaxHp;
	public	float	MoveSpeed;
	public	int	Atk;
	public	float	AtkSpeed;
	public	float	AtkRange;
	public	int	integral;

    
    public MonsterConfig()
    {
    }
    
    public override void InitConfig(SqliteDataReader sdr)
    {
        base.InitConfig(sdr);
        
        	id	=	ReadInt("id");
	Name	=	ReadString("Name");
	resource	=	ReadInt("resource");
	AIType	=	ReadInt("AIType");
	MaxHp	=	ReadInt("MaxHp");
	MoveSpeed	=	ReadFloat("MoveSpeed");
	Atk	=	ReadInt("Atk");
	AtkSpeed	=	ReadFloat("AtkSpeed");
	AtkRange	=	ReadFloat("AtkRange");
	integral	=	ReadInt("integral");

        
    }

    public override int getId()
    {
        return id;
    }
    
  }