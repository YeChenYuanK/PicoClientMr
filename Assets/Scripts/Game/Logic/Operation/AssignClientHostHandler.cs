using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class AssignClientHostHandler : OperationHandler {

	public AssignClientHostHandler()
    {
        this.GameOperCmd = (int)SysOper.ASSIGN_CLIENT_HOST;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        // 指定主机模式
        RobotAIManager.Instance.IsHost = true;
        return true;
    }
}
