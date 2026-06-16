using UnityEngine;
using System.Collections;

public class NodeMoveState : State {

	MoveTeleporter teleport;
	public NodeMoveState(MoveTeleporter teleport) : base(teleport)
	{
		this.teleport = teleport;
	}

	public override void Enter ()
	{
		base.Enter ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void Exit ()
	{
		base.Exit ();
	}

}
