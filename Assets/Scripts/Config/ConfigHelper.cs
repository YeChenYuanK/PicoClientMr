using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ConfigHelper {

    private static SystemConfig systemConfig;
    private static UserConfig userCfg;
    private static List<WeaponConfig> weaponList = null;
    /// <summary>
    /// 系统配置文件
    /// </summary>
    public static SystemConfig SysCfg
    {
        get
        {
            if(systemConfig == null)
            {
                systemConfig = new SystemConfig()
                {
                    id = 0,
                    FootSoldierInitNum = "1_0;2_0;3_0",
                    FootSoldierRefreshCd = 2,
                    FootSoldierRefreshNum = 1,
                    SoldierNumMax = "180_80|300_100|480_120",
                    ProEnterSoldierNumMax = 80,
                    RebirthTime = 1,
                    ProtectTime = 2,
                    BossRefreshTime = 420

                };
            }
            return systemConfig;
        }
    }
    public static void LoadUserConfig()
    {
        /*
        string fileContent = ReadTextFromFile("sys.cfg");
        */
        string fileContent = null;
        if (fileContent != null && fileContent != "")
        {
            Debug.Log("Unity Game Load SysCfg content : " + fileContent);
            userCfg = JsonConvert.DeserializeObject<UserConfig>(fileContent);
        } else
        {
            userCfg = new UserConfig()
            {
                DoorOpenTime = 1.0f,
                DoorOpenKeepTime = 10f,
                DoorCloseTime = 1.0f,
                gunType = 2,// 1 是ppgun 2 是手柄枪
                xRotation = 60,
                zRotation = 0f,
                ServerIp = "192.168.50.41",
                ServerPort = 9596
            };
        }
    }

    private static string ReadTextFromFile(string filename)
    {
        string path = Application.streamingAssetsPath + "/" + filename;
#if UNITY_ANDROID
        WWW www = new WWW(path);
        while (!www.isDone) { }
        return www.text;
#else
        FileStream fs = File.Open(Application.streamingAssetsPath + "/sys.cfg", FileMode.Open);
        string content = "";
        using (fs)
        {
            StreamReader sr = new StreamReader(fs);
            string temp = null;
            while ((temp = sr.ReadLine()) != null)
            {
                content += temp;
            }
        }
        return content;
#endif
    }


    public static UserConfig UserCfg
    {
        get
        {
            if (userCfg == null)
            {
                LoadUserConfig();
            }
            return userCfg;
        }
    }


    /// <summary>
    /// 获得所有武器的配置列表
    /// </summary>
    /// <returns></returns>
    public static List<WeaponConfig> AllWeaponConfigs()
    {
        InitWeaponsData();
        return new List<WeaponConfig>();
    }

    private static void InitWeaponsData()
    {
        if (weaponList == null)
        {
            weaponList = new List<WeaponConfig> {
                new WeaponConfig()
                {
                    id = 1001001,
                    name = "SCAR",
                    price = 1000,
                    shopIndex = 101,
                    normalHit = 100,
                    cirt = 400,
                    firingRate = 10,
                    recoil = 0,
                    recive = 300,
                    magazine = 30,
                    kreloadTime = 2000,
                    repeating = true,
                    shotgunCount = 1
                },
                new WeaponConfig()
                {

                    id = 1001002,
                    name = "SG",
                    price = 1000,
                    shopIndex = 102,
                    normalHit = 200,
                    cirt = 800,
                    firingRate = 1,
                    recoil = 0,
                    recive = 300,
                    magazine = 5,
                    kreloadTime = 3000,
                    repeating = false,
                    shotgunCount = 5
                },
                new WeaponConfig()
                {
                    id = 1001003,
                    name = "AK",
                    price = 1000,
                    shopIndex = 101,
                    normalHit = 150,
                    cirt = 800,
                    firingRate = 6,
                    recoil = 0,
                    recive = 300,
                    magazine = 30,
                    kreloadTime = 2000,
                    repeating = true,
                    shotgunCount = 1
                }
            };
        }
    }

    public static WeaponConfig GetWeaponCfg(int gunId)
    {
        InitWeaponsData();

        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i].id == gunId)
            {
                return weaponList[i];
            }
        }

        return null;
    }

}
