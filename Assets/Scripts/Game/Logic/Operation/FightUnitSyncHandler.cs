using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class FightUnitSyncHandler : OperationHandler {

	public FightUnitSyncHandler()
    {
        this.GameOperCmd = (int)GameOper.FIGHT_UNIT_SYNC;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        // 其他玩家信息同步
        MFightUnit mFightUnit = operation.GetData<MFightUnit>();
        FightUnit originUnit = ClientLogic.Instance.FightManager.GetFightUnit(mFightUnit.unitId);
        FightUnit curUnit = FightUnit.FromProto(mFightUnit);
        ClientLogic.Instance.FightManager.AddUnit(curUnit);
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(SyncFightUnit), curUnit);
        return true;
    }

    private object SyncFightUnit(object par)
    {
        FightUnit fu = par as FightUnit;
        BaseCharacter baseChar = PlayerManager.Instance.GetCharacter(fu.UnitId);
        if (baseChar is OtherCharactor)
        {
            OtherCharactor otherChar = baseChar as OtherCharactor;
           // otherChar.SyncFightUnit(fu);
        }
        return null;
    }
}
