using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using LekeNet.Room;

/// <summary>
/// 创建机器人
/// </summary>
public class CreateRobotHandler : OperationHandler {

	public CreateRobotHandler()
    {
        this.GameOperCmd = (int)SysOper.CREATE_AI_ROBOT;
    }

    public override bool OnOperation(RoomOperation operation)
    {

        return true;
    }
}
