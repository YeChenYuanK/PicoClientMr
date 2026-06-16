using UnityEngine;
using System.Collections;

public class NormalState : State {

	BaseGun weapon;

	public NormalState(BaseGun weapon) : base(weapon)
	{
		this.weapon = weapon;
	}

	public override void Enter ()
	{
		base.Enter ();
        /*
        if(weapon.animator != null)
        {
            weapon.animator.CrossFade("Idle", 0);
        }
        */
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void Exit()
	{
		base.Exit();
	}
}
