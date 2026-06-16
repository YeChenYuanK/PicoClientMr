using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParametersManager : BaseManager
{
    public Parameters _parameters;
    public ParametersManager(GameMng mng) : base(mng)
    {
    }
    
    public override void OnInit()
    {
        if (!Mng.isServerHost) return;

        string filePath = Application.streamingAssetsPath + "/Parameters" + ".txt";
        _parameters = JsonConvert.DeserializeObject<Parameters>(File.ReadAllText(filePath));
        GameMng.Instance.LanguageState = _parameters.LanguageState;
       

        //Debug.LogError(_parameters.sceneSize);
    }
    public GunInfo GetGunInfo(string key, int type)
    {
        if (type == 0)
        {
            for (int i = 0; i < _parameters.HandType.Count; i++)
            {
                if (_parameters.HandType[i].Key == key)
                {
                    return _parameters.HandType[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < _parameters.PPType.Count; i++)
            {
                if (_parameters.PPType[i].Key == key)
                {
                    return _parameters.PPType[i];
                }
            }
        }
        return null;
    }
    public void SaveByJosn()
    {
        Parameters p = new Parameters();
        for (int i = 0; i < 3; i++)
        {
            GunInfo info = new GunInfo();
            info.Key = (1001000 + i).ToString();
            info.Posx = 0.007f;
            info.Posy = -0.079f;
            info.Posz = 0.262f;
            info.Rotx = 60F;
            info.Roty = 180F;
            info.Rotz = 0F;
            p.HandType.Add(info);

            p.PPType.Add(info);
        }
        string str = JsonConvert.SerializeObject(p);

        string filePath = Application.streamingAssetsPath + "/Parameters" + ".txt";

        StreamWriter sw = new StreamWriter(filePath);

        sw.Write(str);

        //关闭StreamWriter

        sw.Close();
    }
    [System.Serializable]
    public class Parameters
    {
        public string sceneSize;
        public int LanguageState;//0中文 1英文 2法语 3阿拉伯语 4西班牙语
        public int GameTime;
        public int Type;
        public int BGMvolume;
        public int BloothCloth;//0是不启动  1是启动
        public List<GunInfo> HandType = new List<GunInfo>();
        public List<GunInfo> PPType = new List<GunInfo>();
        public List<BloothConnectInfo> BloothConnectInfos = new List<BloothConnectInfo>();
    }
    [System.Serializable]
    public class GunInfo
    {
        public string Key;
        public float Posx;
        public float Posy;
        public float Posz;
        public float Rotx;
        public float Roty;
        public float Rotz;
    }
    [System.Serializable]
    public class BloothConnectInfo
    {
        public string sn;
        public string bloothName;
        public int index;
    }

}
