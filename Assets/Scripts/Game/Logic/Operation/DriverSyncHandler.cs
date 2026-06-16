using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class DriverSyncHandler : OperationHandler {

	public DriverSyncHandler()
    {
        this.GameOperCmd = (int)GameOper.DRIVER_INFO_SYNC;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        DriverInfo driverInfo = operation.GetData<DriverInfo>();
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(SyncDriverInfo), driverInfo);
        return true;
    }

    private object SyncDriverInfo(object par)
    {
        DriverInfo driverInfo = par as DriverInfo;
        BaseCharacter baseChar = PlayerManager.Instance.GetCharacter(driverInfo.unitId);
        if(baseChar is OtherCharactor)
        {
            OtherCharactor otherChar = baseChar as OtherCharactor;
            //otherChar.UpdateDriver(ProtoHelper.ConvertFromProto(driverInfo.headPos),
            //    ProtoHelper.ConvertFromProto(driverInfo.headRotation),
            //    ProtoHelper.ConvertFromProto(driverInfo.leftPos),
            //    ProtoHelper.ConvertFromProto(driverInfo.leftRotation),
            //    ProtoHelper.ConvertFromProto(driverInfo.rightPos),
            //    ProtoHelper.ConvertFromProto(driverInfo.rightRotation),
            //    driverInfo.posY);
        }
        return null;
    }
}
