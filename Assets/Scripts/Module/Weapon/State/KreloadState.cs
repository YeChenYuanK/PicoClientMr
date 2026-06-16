using UnityEngine;
using System.Collections;

public class KreloadState : State {

	BaseGun weapon;
	long kreloadOvetTime;

	public KreloadState(BaseGun weapon) : base(weapon)
	{
		this.weapon = weapon;
	}

	public override void Enter ()
	{
		base.Enter ();
        kreloadOvetTime = DateUtil.NowMllSec;// weapon.KreloadTime + DateUtil.NowMllSec;
		// weapon.animator.CrossFade ("Change" , 0);
		//播放换弹夹的声音
	}

	public override void Update ()
	{
		base.Update ();
		if(kreloadOvetTime < DateUtil.NowMllSec)
		{
			weapon.KreloadOver ();
			weapon.ChangeState (new NormalState(weapon));
		}
	}

	public override void Exit()
	{
		base.Exit();
	}
}
