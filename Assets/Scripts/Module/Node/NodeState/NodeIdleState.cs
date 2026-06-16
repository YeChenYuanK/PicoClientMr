using UnityEngine;
using System.Collections;

public class NodeIdleState : State {

	MoveTeleporter teleport;
	public NodeIdleState(MoveTeleporter teleport) : base(teleport)
	{
		this.teleport = teleport;

	}

	long endDate;
	public override void Enter ()
	{
		base.Enter ();
		endDate = MoveTeleporter.IDLE_TIME + DateUtil.NowMllSec;
	}

	public override void Update ()
	{
		base.Update ();
		if(endDate < DateUtil.NowMllSec)
		{
			
		}
	}

	public override void Exit ()
	{
		base.Exit ();
	}

}
