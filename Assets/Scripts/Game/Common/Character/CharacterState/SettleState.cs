using UnityEngine;
using System.Collections;

public class SettleState : State 
{

	BaseCharacter character;

	public SettleState(BaseCharacter character) : base(character)
	{
		this.character = character;
	}

	public override void Enter ()
	{
		base.Enter ();
	}

	public override void Update ()
	{
	}
	

	public override void Exit()
	{
	}
}
