using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class PlayerOtherInitHandler : OperationHandler {

	public PlayerOtherInitHandler()
    {
        this.GameOperCmd = (int)GameOper.PLAYER_NEW;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        MFightUnit mFightUnit = operation.GetData<MFightUnit>();
        FightUnit fightUnit = FightUnit.FromProto(mFightUnit);
        ClientLogic.Instance.FightManager.AddUnit(fightUnit);
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(CreateOther), fightUnit);
        return true;
    }

    private object CreateOther(object par)
    {
        FightUnit fu = par as FightUnit;
        PlayerManager.Instance.CreateOther(fu);
        return null;
    }
}
