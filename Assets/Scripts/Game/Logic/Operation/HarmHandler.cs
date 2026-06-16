using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class HarmHandler : OperationHandler {

	public HarmHandler()
    {
        this.GameOperCmd = (int)GameOper.UNIT_HARMED;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        HarmInfo harmInfo = operation.GetData<HarmInfo>();
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(Harm), harmInfo);
        return true;
    }

    private object Harm(object par)
    {
        HarmInfo harmInfo = par as HarmInfo;
        FightUnit fightUnit = ClientLogic.Instance.FightManager.GetFightUnit(harmInfo.unitId);
        if(fightUnit != null)
        {
            fightUnit.Health = harmInfo.curHealth;
        }

        BaseCharacter harmChar = PlayerManager.Instance.GetCharacter(fightUnit.UnitId);
        if(harmChar != null)
        {
            harmChar.Hurt(harmInfo.shooterId, ProtoHelper.ConvertFromProto(harmInfo.hurtPos), ProtoHelper.ConvertFromProto(harmInfo.hurtQuaternion), harmInfo.triggerPart);
        }
        
        return null;
    }
}
