using System;
using System.Collections.Generic;
using com.gamestudio.cs;
using LekeNet.Room;
using UnityEngine.SceneManagement;

public class BattleSettleOverHandler : OperationHandler
{
	public BattleSettleOverHandler()
	{
		this.GameOperCmd = (int)GameOper.SETTLE_INFO;
	}


	public override bool OnOperation(RoomOperation operation)
	{
        MBattleSettle battleSettle = operation.GetData<MBattleSettle>();
		MainFrameCall.Instance.AddCall(OnBattleSettleOver, battleSettle);
		return true;
	}

	public object OnBattleSettleOver(object par)
	{
        MBattleSettle battleSettle = par as MBattleSettle;

        CloseNet();
        //SceneManager.LoadScene("Scene/MainSence");
		//SelfCharacter.Instance.GameOver(battleSettle);
        return null;
	}

    public void CloseNet()
    {
        ClientLogic.Instance.Destory();
    }
}

