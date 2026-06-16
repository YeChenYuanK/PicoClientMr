using UnityEngine;
using System.Collections;

public class StateMachine
{
	//entity 实体
	private BaseGameEntity owner;
		
	private State currentState;
	private State previousState;
	private State globalState;
		
	public StateMachine (BaseGameEntity owner)
	{
		this.owner = owner;
		this.currentState = null;
		this.previousState = null;
		this.globalState = null;
	}

	public void GlobalStateEnter()
	{
		this.globalState.Enter();
	}
		
	public void SetGlobalStateState(State GlobalState)
	{
		this.globalState = GlobalState;
		this.globalState.Enter();
	}
		
	public void SetCurrentState(State CurrentState)
	{
		this.currentState = CurrentState;
		this.currentState.Enter();
	}

	public void SMUpdate ()
	{
		//全局状态的运行
		if (this.globalState != null)
			this.globalState.Update ();

		//一般当前状态的运行
		if (this.currentState != null)
		{
			this.currentState.Update ();
		}
	}
		
	public void ChangeState (State newState)
	{
		if (newState == null) {
			Debug.LogError ("该状态不存在");
		}

		if (this.currentState == null)
		{
			this.SetCurrentState(newState);
			return;
		}
        //不重复执行状态
        if (newState.GetType() == currentState.GetType())
        {
            return;
        }
        

		//退出之前状态
		this.currentState.Exit();
		//保存之前状态
		this.previousState = currentState;	
			
		//设置当前状态
		this.currentState = newState;
		//进入当前状态
		this.currentState.Enter ();
	}
		
	public State CurrentState ()
	{
		return this.currentState;
	}

    public bool IsEquip(System.Type type)
    {
        if (currentState != null && CurrentState().GetType() == type)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public State GlobalState ()
	{
		return this.globalState;
	}
	public State PreviousState ()
	{
		return this.previousState;
	}

    public BaseGameEntity GetOwnerEntity()
    {
        return owner;
    }

    public void CloseMachine()
    {
        if (currentState != null)
        {
            currentState.Exit();
            currentState = null;
        }
        if (globalState != null)
        {
            globalState.Exit();
            globalState = null;
        }
    }
}