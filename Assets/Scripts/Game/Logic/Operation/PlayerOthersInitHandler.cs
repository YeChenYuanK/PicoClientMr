using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using System.Collections.Generic;
using LekeNet.Room;

public class PlayerOthersInitHandler : OperationHandler {

	public PlayerOthersInitHandler()
    {
        this.GameOperCmd = (int)GameOper.PLAER_INFO_OTHER_INIT;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        // 其他所有玩家集合
        MFightRoom fightRoom = operation.GetData<MFightRoom>();
        List<FightUnit> units = new List<FightUnit>();
        foreach (MFightUnit mfu in fightRoom.fightUnits)
        {
            FightUnit fu = FightUnit.FromProto(mfu);
            units.Add(fu);
            ClientLogic.Instance.FightManager.AddUnit(fu);
        }

        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(CreateOthers), units);
        return true;
    }

    private object CreateOthers(object par)
    {
        List<FightUnit> fus = par as List<FightUnit>;
        PlayerManager.Instance.CreateOthers(fus);
        return null;
    }
}
