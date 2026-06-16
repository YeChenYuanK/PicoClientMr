using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.leke.redSea;

public class GameRecordItem : MonoBehaviour {

    public NumberShow killsTxt;
    public NumberShow headShotTxt;
    public NumberShow deadsTxt;
    public NumberShow scoreTxt;

    public GameObject titleInfo;
    public GameObject self;
    public GameObject noplayerBg;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void Hide()
    {
        titleInfo.SetActive(false);
        self.SetActive(false);
        noplayerBg.SetActive(true);
        killsTxt.gameObject.SetActive(false);
        headShotTxt.gameObject.SetActive(false);
        deadsTxt.gameObject.SetActive(false);
        scoreTxt.gameObject.SetActive(false);
    }

    public void Show(PlayerRecord playerRecord, int score)
    {
        titleInfo.SetActive(true);
        noplayerBg.SetActive(false);
        if (playerRecord.Index == PrepareData.Instance.SelfAllocateIndex)
        {
            self.SetActive(true);
        } else
        {
            self.SetActive(false);
        }
        killsTxt.gameObject.SetActive(true);
        headShotTxt.gameObject.SetActive(true);
        deadsTxt.gameObject.SetActive(true);
        scoreTxt.gameObject.SetActive(true);
        this.killsTxt.ShowNumber(playerRecord.Kills);
        this.headShotTxt.ShowNumber(playerRecord.HeadShots);
        this.deadsTxt.ShowNumber(playerRecord.Deads);
        this.scoreTxt.ShowNumber(score);
    }
}
