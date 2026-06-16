using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.gamestudio.cs;
using LekeNet.Room;

public class MovebleNodeSyncHandler : OperationHandler 
{

	public MovebleNodeSyncHandler()
	{
		this.GameOperCmd = (int)GameOper.MOVABLE_TELEPORTER_BEGIN_TIME;
	}

	public override bool OnOperation(RoomOperation operation)
	{
		MTeleporterMoveTime teleSync = operation.GetData<MTeleporterMoveTime>();
		if (teleSync != null) {
			Debug.Log ("aaaaaaaaaaaa" + teleSync.beginMoveTime);
		}
		MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(SyncTelePortInfo), teleSync);
		return true;
	}

	private object SyncTelePortInfo(object par)
	{
		MTeleporterMoveTime teleSync = par as MTeleporterMoveTime;
		List<BasePoint> points = TeleporterManager.Instance.GetMovePoints ();
		MoveTeleporter teleporter;
		foreach(BasePoint point in points)
		{
			teleporter = point.telePorter.parent.GetComponent<MoveTeleporter> ();
			teleporter.Init (teleSync.beginMoveTime);
		}
		Debug.Log(">>>>>>>>>>>>>>>>>>>>>MovebleNodeSyncHandler:" + teleSync.beginMoveTime);
		return null;
	}
}
