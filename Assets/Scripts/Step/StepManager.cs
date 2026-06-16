using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StepManager : MonoBehaviour {

    public StepAction FristAction;

    private StepAction CurrAction;

    private StepAction[] actions;

	// Use this for initialization
	void Start () {
        //GameInputManager.Instance.OKPress += OKPress;
        OKPress();
        actions = GetComponentsInChildren<StepAction>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(actions != null && actions.Length > 0)
        {
            for(int i = 0; i < actions.Length; i++)
            {
                if(actions[i].serverStartTime == 0)
                {
                    continue;
                }
                //判断未处理的action是否需要处理
                if(actions[i].State == StepAction.StepState.begin)
                {
                    actions[i].Init();
                }else if(actions[i].State == StepAction.StepState.exe)
                {
                    actions[i].Exe();
                }

            }
        }
    }
    private void OKPress()
    {
        //Debug.Log("111");
        if (CurrAction == null)
        {
            //开始
            CurrAction = FristAction;
            CurrAction.Init();
            CurrAction.OvertimeCallback = OvertimeCallback;
        }
        else if(CurrAction.NextAction != null)
        {
            if (CurrAction.State == StepAction.StepState.over)
            {
                //下一步
                CurrAction.Exit();
                CurrAction = CurrAction.NextAction;
                CurrAction.Init();
                CurrAction.OvertimeCallback = OvertimeCallback;
            }
        }else
        {
            //结束
            Debug.Log("已结束，显示结束界面");
        }
    }

    private void OvertimeCallback()
    {
        //Debug.Log("OvertimeCallback:" + CurrAction);
    }
}
