using UnityEngine;


public class State
{
	protected BaseGameEntity target;

	public State(BaseGameEntity target)
	{
		if (target == null) {
			Debug.LogError ("游戏对象实体为空");
			return;
		}
		this.target = target;
	}
	

	//进入状态  
	public virtual void Enter ()
	{
			
	}
		
	//状态正常执行
	public virtual void Update ()
	{
			
	}
		
	//退出状态
	public virtual void Exit ()
	{
			
	}

	public BaseGameEntity Target
	{
		get {return this.target;}
	}
}