using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Mirror;

public class StepAction : MonoBehaviour
{
    public long ActionTime = 0;
    [SerializeField]
    protected List<StepParam> EventList;
    public StepAction NextAction = null;

    public Action OvertimeCallback;

    public enum StepState
    {
        begin = 0,
        exe = 1,
        over = 2
    }
    public double serverStartTime = 0;
    protected StepState state = StepState.begin;
    public StepState State
    {
        get
        {
            return state;
        }
    }
    protected long startTime = 0;
    public virtual double PassTime
    {
        get
        {
            return (NetworkTime.time - serverStartTime);
        }
    }

    Animator anim;
    // Use this for initialization
    protected virtual void Start ()
    {
    }

    // Update is called once per frame
    protected virtual void Update ()
    {

        //if(serverStartTime == 0 )
        //{
        //    if(PhotonNetwork.room.CustomProperties.ContainsKey("ServerStartTime"))
        //    {
        //        serverStartTime = double.Parse(PhotonNetwork.room.CustomProperties["ServerStartTime"].ToString());

        //    }
        //    return;
        //}
        ////Debug.Log(anim.clip.name);
        //if (state == StepState.exe)
        //{
        //    Exe();
        //}
    }

    protected virtual void FixedUpdate()
	{
        if (serverStartTime == 0)
        {
            InitStartTime();
            return;
        }
        if (state == StepState.exe)
        {
            Exe();
        }
    }

    public virtual void Init()
    {
        state = StepState.exe;
    }

    public virtual void Exe()
    {
        if(EventList.Count <= 0)
        {
            EndCallback();
            state = StepState.over;
        }else
        {
            for(int i = 0; i < EventList.Count; i++)
            {
                if(EventList[i].ExeTime + ActionTime <= PassTime)
                {
                    StepParam param = EventList[i];
                    param.Exe();
                    if (param.IsUpdateAnimParam)
                    {
                        if (param.target != null)
                        {
                            AnimatorParam animParam = param.target.AddComponent<AnimatorParam>();
                            animParam.serverStartTime = serverStartTime;
                            animParam.ActionTime = EventList[i].ExeTime + ActionTime;
                        }
                        else
                        {
                            Debug.Log("StepAction:" + gameObject.name + "的Target为空");
                        }
                    }
                    EventList.Remove(EventList[i]);
                    i--;
                }
            }
        }
    }

    public virtual void InitStartTime()
    {
       
        
    }

    public void Exit()
    {
        Debug.Log("Exit~");
    }

    protected void EndCallback()
	{
        //Debug.Log ("EndCallback");
        //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Black");
        if(OvertimeCallback != null)
            OvertimeCallback();

    }

    void EventCallback(StepParam aevent)
    {
        Debug.Log("Event:" + EventList.IndexOf(aevent));
        //StepList[StepList.IndexOf(aevent)].EventCallback(aevent);
    }
}
