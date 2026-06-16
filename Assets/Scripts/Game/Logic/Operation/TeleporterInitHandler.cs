using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class TeleporterInitHandler : OperationHandler {

	public TeleporterInitHandler()
    {
        this.GameOperCmd = (int)GameOper.TELEPORTER_INIT;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        TeleporterSync teleSync = operation.GetData<TeleporterSync>();
        foreach (MTeleporter mt in teleSync.teleporters)
        {
            MapManager.Instance.AddTeleportInfo(mt);
        }
        // MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(SyncTelePortInfo), teleSync);
        return true;
    }

    private object SyncTelePortInfo(object par)
    {
        TeleporterSync teleSync = par as TeleporterSync;
        foreach (MTeleporter mt in teleSync.teleporters)
        {
            MTeleporter origin = MapManager.Instance.GetTeleporter(mt.index);

            MapManager.Instance.AddTeleportInfo(mt);
            BasePoint point = TeleporterManager.Instance.FindBasePoint(mt.index);
            point.status = mt.status;

            point.Change(origin, mt);
        }
        return null;
    }
}
