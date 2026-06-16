using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using LekeNet.Room;

/// <summary>
/// 同步
/// </summary>
public class ServerTimeSyncHandler : OperationHandler
{
    public ServerTimeSyncHandler()
	{
        this.GameOperCmd = (int)SysOper.SERVER_TIME_SYNC;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        TimeSync timesync = operation.GetData<TimeSync>();
        long clientTime = DateUtil.NowMllSec;
        DateUtil.ServerTimeDiff = clientTime - timesync.timestamp;
        return true;
    }
}
