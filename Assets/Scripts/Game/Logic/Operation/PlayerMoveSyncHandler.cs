using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using com.gamestudio.tank;
using LekeNet;
using LekeNet.Room;

public class PlayerMoveSyncHandler : OperationHandler {

	public PlayerMoveSyncHandler()
    {
        this.GameOperCmd = (int)GameOper.PLAYER_INSTANT_MOVE;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        PlayerInstantMove pmove = operation.GetData<PlayerInstantMove>();
        MainFrameCall.Instance.AddCall(new MainFrameCall.FrameCall(SyncTelePortInfo), pmove);
        return true;
    }

    private object SyncTelePortInfo(object par)
    {
        PlayerInstantMove pmove = par as PlayerInstantMove;
		FightUnit originUnit = ClientLogic.Instance.FightManager.GetFightUnit (pmove.playerId);
		BaseCharacter character = PlayerManager.Instance.GetCharacter(pmove.playerId);
		if (character is SelfCharacter) {
		} else {
//			character
			//播放1.传送特效,2.移动特效（获得上个点位置），3.到位置提示特效， 
			//((OtherCharactor)character).PlayerTeleportEffect(ProtoHelper.ConvertFromProto(pmove.currentPos) , ProtoHelper.ConvertFromProto(pmove.targetPoint));
		}
		originUnit.CurIndex = pmove.targetIndex;
        return null;
    }
}
