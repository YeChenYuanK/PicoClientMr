using System;
using System.Collections.Generic;
using com.gamestudio.cs;
using LekeNet.Room;

public class GunSynHandler : OperationHandler
{
	public GunSynHandler ()
	{
		this.GameOperCmd = (int)GameOper.CHANGE_WEAPON;
	}


	public override bool OnOperation(RoomOperation operation)
	{
		MChangeWeapon weapone = operation.GetData<MChangeWeapon>();
		MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(SyncGun), weapone);
		return true;
	}

	public object SyncGun(object par)
	{
		MChangeWeapon weapone = par as MChangeWeapon;
		List<BaseCharacter> allPlayers = PlayerManager.Instance.AllPlayers;
		foreach (BaseCharacter basePlayer in allPlayers) 
		{
			if(basePlayer.unitId == weapone.unitid)
			{
				basePlayer.ChangeWeapon (weapone);
			}
		}
		return null;
	}
}

