using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class PlayerSelfInitHandler : OperationHandler {

	public PlayerSelfInitHandler()
    {
        this.GameOperCmd = (int)GameOper.PLAYER_INFO_SELF_INIT;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        MFightUnit mFightUnit = operation.GetData<MFightUnit>();
        FightUnit selfUnit = FightUnit.FromProto(mFightUnit);
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(CreateSelf), selfUnit);
        ClientLogic.Instance.FightManager.AddUnit(selfUnit);
        //PlayerInfo.RoomUnitId = selfUnit.UnitId;
        //PlayerInfo.Name = selfUnit.Name;
        return true;
    }

    private object CreateSelf(object par)
    {
        FightUnit fu = par as FightUnit;
        PlayerManager.Instance.CreateSelf(fu);
        return null;
    }
}
