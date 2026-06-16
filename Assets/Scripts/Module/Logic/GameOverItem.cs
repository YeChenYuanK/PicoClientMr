using com.leke.redSea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverItem : MonoBehaviour {

    public NumberShow killsTxt;
    public NumberShow headShotTxt;
    public NumberShow deadsTxt;
    public NumberShow scoreTxt;

    public GameObject mvp;
    public TextMesh nameTxt;
    public void Show(PlayerInfo playerRecord, bool isMvp = false)
    {
        this.nameTxt.text = playerRecord.PlayerName;
        this.killsTxt.ShowNumber(playerRecord.Kills + playerRecord.HeadShots);
        this.headShotTxt.ShowNumber(playerRecord.HeadShots);
        this.deadsTxt.ShowNumber(playerRecord.Deads);
        int score = GameUtil.CountScore(playerRecord);
        this.scoreTxt.ShowNumber(score);
        this.mvp.SetActive(isMvp);
    }
}
