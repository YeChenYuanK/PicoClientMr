using UnityEngine;
using System.Collections;
using LekeNet;

public class MapInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ClientLogic.Instance.NoticeSceneLoadComplete();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnApplicationQuit()
    {
        TcpContext.Instance.Destory();
        ClientLogic.Instance.Destory();
    }
}
