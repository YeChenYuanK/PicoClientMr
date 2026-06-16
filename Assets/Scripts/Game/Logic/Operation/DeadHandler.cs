using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class DeadHandler : OperationHandler {

	public DeadHandler()
    {
        this.GameOperCmd = (int)GameOper.UNIT_DEAD;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        DeadInfo deadInfo = operation.GetData<DeadInfo>();
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(Dead), deadInfo);
        return true;
    }

    private object Dead(object par)
    {
        DeadInfo deadInfo = par as DeadInfo;
        FightUnit fightUnit = ClientLogic.Instance.FightManager.GetFightUnit(deadInfo.unitId);
        if(fightUnit != null)
        {
            fightUnit.Health = 0;
            fightUnit.DeadTime = deadInfo.deadTime;
            fightUnit.RebirthTime = deadInfo.rebirthTime;
        }

        BaseCharacter harmChar = PlayerManager.Instance.GetCharacter(fightUnit.UnitId);
        if(harmChar != null)
        {
            harmChar.Dead(fightUnit);
        }
        
        return null;
    }
}
