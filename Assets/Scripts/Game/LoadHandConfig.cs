using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PPGunItem
{
    public float px;
    public float py;
    public float pz;

    public float rx;
    public float ry;
    public float rz;
}
public class PPGunConfig
{
    public PPGunItem HandConfig;
    public PPGunItem PPGunHandConfig;
}

public class LoadHandConfig : MonoBehaviour
{

    public static PPGunConfig GunConfig = null;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Debug.Log("开始加载枪模的偏移配置文件");
        // 从网络加载配置文件
        string url = "http://192.168.16.150/ppgunconfig.json";
        UnityWebRequest request = UnityWebRequest.Get(url);
        this.StartCoroutine(GetConfig(request));
    }

    private IEnumerator GetConfig(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            // 处理成功的响应
            string configData = request.downloadHandler.text;
            Debug.Log($"加载枪模的偏移配置文件 {configData}");
            // 这里可以对解析得到的配置数据进行进一步操作，比如反序列化为对象
            GunConfig = JsonConvert.DeserializeObject<PPGunConfig>(configData);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
