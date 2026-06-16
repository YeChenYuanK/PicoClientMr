using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class FireHandler : OperationHandler {

	public FireHandler()
    {
        this.GameOperCmd = (int)GameOper.FIRE;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        FireInfo fireInfo = operation.GetData<FireInfo>();
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(Fire), fireInfo);
        return true;
    }

    private object Fire(object par)
    {
        FireInfo fireInfo = par as FireInfo;
        BaseCharacter shooter = PlayerManager.Instance.GetCharacter(fireInfo.shooterId);
        if(shooter != null)
        {
            shooter.Fire(fireInfo);
        }
        return null;
    }
}
