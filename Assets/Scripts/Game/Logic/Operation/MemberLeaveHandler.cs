using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class MemberLeaveHandler : OperationHandler {

	public MemberLeaveHandler()
    {
        this.GameOperCmd = (int)GameOper.MEMBER_LEAVE;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        MemberLeave ml = operation.GetData<MemberLeave>();
        FightUnit unit = ClientLogic.Instance.FightManager.RemoveUnit(ml.playerId);
        if(unit != null)
        {
            MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(DestoryPlayer), ml.playerId);
        }
        return true;
    }

    private object DestoryPlayer(object par)
    {
        PlayerManager.Instance.RemovePlayer((int)par);
        return null;
    }
}
