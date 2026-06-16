using UnityEngine;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using com.gamestudio.cs;

public class ConfigManager
{
    private static ConfigManager instance;

    private static GlobalCfg globalCfg;

    public static ConfigManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ConfigManager();
                // instance.InitDB();
            }
            return instance;
        }
    }

    public static GlobalCfg GlobalCfg
    {
        get
        {
            return globalCfg;
        }
        set
        {
            globalCfg = value;
        }
    }

    private DbAccess db;

    public void InitDB()
    {
		string appDBPath = Application.streamingAssetsPath + "/MyDB.db";
        db = new DbAccess();
        db.OpenDB(@"Data Source=" + appDBPath);
    }

    public List<T> GetAllConfig<T>() where T : BaseConfig, new()
    {
        List<T> configs = new List<T>();
        
        SqliteDataReader sdr = db.ReadFullTable(new T().GetType().Name);
        while (sdr.Read())
        {
            T config = new T();
            config.InitConfig(sdr);
            configs.Add(config);
        }

        return configs;
    }

    public T GetConfigByID<T>(int id) where T : BaseConfig, new()
    {
        T config = new T();
        SqliteDataReader sdr = db.SelectWhere(config.GetType().Name, new[] { "*" }, new[] { "Id" }, new[] { "=" }, new[] { id.ToString() });
        if(sdr.Read())
        {
			config.InitConfig(sdr);
            return config;
        }
        else
        {
            return null;
        }
    }

    public List<T> GetConfigsByID<T>(int id) where T : BaseConfig, new()
    {
        List<T> configs = new List<T>();
        SqliteDataReader sdr = db.SelectWhere(new T().GetType().Name, new[] { "*" }, new[] { "Id" }, new[] { "=" }, new[] { id.ToString() });
        while (sdr.Read())
        {
            T config = new T();
			config.InitConfig(sdr);
            configs.Add(config);
        }

        return configs;
    }

	/// <summary>
	/// 双主键查询
	/// </summary>
	/// <returns>The config by double key.</returns>
	/// <param name="key1">Key1.</param>
	/// <param name="value1">Value1.</param>
	/// <param name="id2">Id2.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public T GetConfigByDoubleKey<T>(string key1, string value1 , string key2 , string value2) where T : BaseConfig, new()
	{
		T config = new T ();
		SqliteDataReader sdr = db.SelectWhere(config.GetType().Name , new[]{"*"},new[] {key1 , key2},new[]{"=","="},new []{value1,value2});
		if(sdr.Read())
		{
			config.InitConfig(sdr);
		}
		return config;
	}

	/// <summary>
	/// 多条件查询
	/// </summary>
	/// <returns>The config by multi condition.</returns>
	/// <param name="colAndVal">列名, 操作符 , 值.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	/// Example:GetConfigByMultiCondition<WeaponConfig>("id","=" ,"1004001","type","=" ,"3");
	public T GetConfigByMultiCondition<T>(params string[] colAndVal) where T : BaseConfig, new()
	{
		T config = new T ();
		if(colAndVal.Length %3 != 0)
		{
			Debug.LogError ("查询条件和值不对等.");
			return null;
		}
		int len = colAndVal.Length / 3;
		string [] col = new string[len];
		string [] opr = new string[len];
		string [] value = new string[len];
		for(int i = 0 ; i < len ; i++)
		{
			col [i] = colAndVal [i*3].ToString();
			opr [i] = colAndVal [i * 3 + 1].ToString();
			value [i] = colAndVal [i * 3 + 2].ToString();
		}
		SqliteDataReader sdr = db.SelectWhere(config.GetType().Name , new[]{"*"}, col , opr , value);
		if(sdr.Read())
		{
			config.InitConfig(sdr);
		}
		return config;
	}

    protected void OnApplicationQuit()
    {
        db.CloseSqlConnection();
    }

	public void CloseSql()
	{
		db.CloseSqlConnection ();
	}
}
