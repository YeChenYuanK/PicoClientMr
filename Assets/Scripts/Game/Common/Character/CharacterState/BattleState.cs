using UnityEngine;
using System.Collections;

public class BattleState : State {

	BaseCharacter character;

	public BattleState(BaseCharacter character) :base (character)
	{
		this.character = character;
	}
	public override void Enter ()
	{
		base.Enter ();
	}
}
