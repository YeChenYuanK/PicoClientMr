using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;

// 将调用push到主循环调用
public class MainFrameCall : MonoBehaviour {

    public delegate System.Object FrameCall(System.Object obj);

    private static MainFrameCall instance;

    public static MainFrameCall Instance
    {
        get { return instance; }
    }

    private Queue<FrameCallPair> queue;

    private List<FrameCallPair> waitingList;

    void Awake()
    {
        this.queue = new Queue<FrameCallPair>();
        this.waitingList = new List<FrameCallPair>();
        instance = this;
    }

    // Use this for initialization
    void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        while(this.queue.Count > 0)
        {
            FrameCallPair pair = this.queue.Dequeue();
            if(pair != null)
            {
                System.Object result = null;
                try
                {
                    result = pair.frameCall(pair.paras);
                }catch(Exception e)
                {
					Debug.LogError("main frame call fail " + e.Message + ", statck:" + e.StackTrace);
                }

                lock(pair)
                {
                    pair.isSuc = true;
                    pair.result = result;
                    Monitor.PulseAll(pair);
                }
            }
        }
    }

    void FixedUpdate()
    {
        RobotAIManager.Instance.OnTick();
    }

    public void AddCall(FrameCall frameCall, System.Object para)
    {
        FrameCallPair pair = new FrameCallPair();
        pair.paras = para;
        pair.frameCall = frameCall;
        this.queue.Enqueue(pair);
    }

    public void AddCallSync(FrameCall frameCall, System.Object para, out System.Object result)
    {
        FrameCallPair pair = new FrameCallPair();
        pair.paras = para;
        pair.frameCall = frameCall;
        this.queue.Enqueue(pair);
        this.waitingList.Add(pair);
        lock(pair) {
            while (!pair.isSuc)
            {
                Monitor.Wait(pair);
            }
            this.waitingList.Remove(pair);
            result = pair.result;
        }
    }

    void OnApplicationQuit()
    {
        foreach (FrameCallPair pair in this.waitingList)
        {
            lock (pair)
            {
                pair.isSuc = true;
                Monitor.PulseAll(pair);
            }
        }
    }
}

public class MainFrameCallName
{
    public const string GET_ALL_STATE = "GET_ALL_STATE";
}

public class FrameCallPair
{
    public System.Object paras;

    public MainFrameCall.FrameCall frameCall;

    public volatile System.Object result;

    public volatile bool isSuc = false;


}
