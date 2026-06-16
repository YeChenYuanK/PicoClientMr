using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class RebirthHandler : OperationHandler {

	public RebirthHandler()
    {
        this.GameOperCmd = (int)GameOper.UNIT_REBIRTH;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        MFightUnit fightUnit = operation.GetData<MFightUnit>();
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(Rebirth), FightUnit.FromProto(fightUnit));
        return true;
    }

    private object Rebirth(object par)
    {
        FightUnit rebirthUnit = par as FightUnit;
        FightUnit fightUnit = ClientLogic.Instance.FightManager.AddUnit(rebirthUnit);

        BaseCharacter harmChar = PlayerManager.Instance.GetCharacter(fightUnit.UnitId);
        if(harmChar != null)
        {
            harmChar.Rebirth(fightUnit);
        }
        
        return null;
    }
}
