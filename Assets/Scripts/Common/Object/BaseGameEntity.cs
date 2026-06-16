using System;
using UnityEngine;


//----------------------------------------------
// 所有需要状态管理的游戏对象继承此基类
// author lisongcheng
//----------------------------------------------
public class BaseGameEntity : BaseEventBehaviour
{
	//指向一个状态实例的指针
	protected StateMachine stateMachine;
    public static string ITWEEN_COMPLETE = "BaseGameEntity.ITWEEN_COMPLETE";

	public BaseGameEntity()
	{
		this.stateMachine = new StateMachine(this);
	}

	public StateMachine GetFSM ()
	{
		//返回状态管理机
		return stateMachine;
	}

    public void ChangeState(State state)
    {
        stateMachine.ChangeState(state);
    }

    public void ChangeState(System.Type type)
    {
        System.Object[] pram = new System.Object[] { this };
        State state = Activator.CreateInstance(type, pram) as State;
        stateMachine.ChangeState(state);
        Debug.Log("游戏进入状态:" + type.ToString());
    }

    public System.Type GetCurrStateType()
    {
        if (stateMachine.CurrentState() != null)
        {
            return stateMachine.CurrentState().GetType();
        }
        else
        {
            return null;
        }
    }

    public virtual void TweenComplete()
    {
        Dispach(ITWEEN_COMPLETE);
    }

    public virtual void Update()
    {

    }

}
