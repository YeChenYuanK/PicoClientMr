using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mono.Data.Sqlite;

public class BaseConfig
{
	private SqliteDataReader sdr;

    public BaseConfig()
    {
    }
	
	public virtual void InitConfig(SqliteDataReader sdr)
	{
		this.sdr = sdr;
	}



    protected int ReadInt(string name)
    {
        try
        {
			return sdr.GetInt32(sdr.GetOrdinal(name));
        }
        catch (Exception e)
        {
            Debug.LogError(name + ":" + e.ToString());
            return 0;
        }
    }

	protected string ReadString(string name)
	{
		try
		{
			return sdr.GetString(sdr.GetOrdinal(name));
		}
		catch (Exception e)
		{
			Debug.LogError(name + ":" + e.ToString());
			return "";
		}
	}
    protected byte ReadByte(string name)
    {
		try
		{
			return sdr.GetByte(sdr.GetOrdinal(name));
		}
		catch (Exception e)
		{
			Debug.LogError(name + ":" + e.ToString());
			return 0;
		}
    }

    protected short ReadShort(string name)
    {
		try
		{
			return sdr.GetInt16(sdr.GetOrdinal(name));
		}
		catch (Exception e)
		{
			Debug.LogError(name + ":" + e.ToString());
			return 0;
		}
    }

    protected long ReadLong(string name)
    {
		try
		{
			return sdr.GetInt64(sdr.GetOrdinal (name));
		}
		catch (Exception e)
		{
			Debug.LogError(name + ":" + e.ToString());
			return 0;
		}
    }

    protected bool ReadBool(string name)
    {
		try
		{
			return ReadInt (name) >= 1 ? true : false ;
		}
		catch (Exception e)
		{
			Debug.LogError(name + ":" + e.ToString());
			return false;
		}
    }

    protected float ReadFloat(string name)
    {
		try
		{
			return sdr.GetFloat(sdr.GetOrdinal(name));
		}
		catch (Exception e)
		{
			Debug.LogError(name + ":" + e.ToString());
			return 0;
		}
    }

    protected string[] ReadArray(string name)
    {
        try
        {
            return sdr.GetString(sdr.GetOrdinal(name)).Split("_"[0]);
        }catch(Exception e)
        {
            Debug.LogError(name + ":" + e.ToString());
            return null;
        }
    }

    public virtual int getId()
    {
        throw new System.NotImplementedException();
    }
}