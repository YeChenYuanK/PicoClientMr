using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using LekeNet.Room;

/// <summary>
/// 同步
/// </summary>
public class ScoreGetHandler : OperationHandler
{
    public ScoreGetHandler()
	{
        this.GameOperCmd = (int)GameOper.SCORE_GET;
    }

    public override bool OnOperation(RoomOperation operation)
    {
        ScoreGet scoreGet = operation.GetData<ScoreGet>();
        ScoreManager.Instance.UpdateScore(scoreGet.campId, scoreGet.campScore);

		System.Object result = null;
//		MainFrameCall.Instance.AddCallSync (UpdateScore ,scoreGet ,out result);
//		TempUIManager.Instance.UpdateScroe (scoreGet.campId, scoreGet.campScore);
        return true;
    }

	private System.Object UpdateScore(System.Object obj)
	{
		ScoreGet scoreGet = obj as ScoreGet;
		UIManager.Instance.UpdateScroe (scoreGet.campId, scoreGet.campScore);
		return null;
	}
}
