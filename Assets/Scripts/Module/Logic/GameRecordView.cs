using com.leke.redSea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DataManager;

public enum BattleResult{
    NONE,
    WIN,
    LOSE,
    DRAW,
}

public class GameRecordView : MonoBehaviour {

    public List<GameRecordItem> recordItems;

    public NumberShow totalScoreTxt;

    public GameObject winFlag;
    public GameObject loseFlag;
    public GameObject drawFlag;

    private bool hasShow;

    private float exitTime;

    private int mvpIndex;

    public List<GameObject> mvpList;

    private bool isOver;

	void Start () {
        ShowWinORLose(BattleResult.NONE);
    }
	
	void Update () {
        if(!hasShow)
        { 
            if (PrepareData.Instance.GameRecord != null)
            {
                hasShow = true;
                Show();
                exitTime = Time.time + 10;
            }
        }
        
        if (exitTime > 0 && Time.time >= exitTime && !isOver)
        {
            if (InitManager.Instance.IsControlCenter)
            {
                // 重新Init
                // GameMng.Instance.ChangeState(typeof(PrepareState));
                //ControlClient.Instance.DisConnect();
                SceneManager.LoadScene("Begin");
            }
            else
            {
                SceneLoader.Instance.StartScene("StartScene");
            }
        }
	}

    public void HideAll()
    {
        for(int i=0;i< this.recordItems.Count;i++)
        {
            this.recordItems[i].Hide();
        }
    }

    public void ShowMvp()
    {
        this.mvpList[mvpIndex].SetActive(true);
    }

    public void Show()
    {
        HideAll();
        int redCampScore = 0;
        int blueCampScore = 0;
        int maxScoreValue = 0;
        GameRecord record = PrepareData.Instance.GameRecord;
        for (int i = 0; i < record.gameRecordInfo.playerRecords.Count; i++)
        {
            GameRecordItem item = this.recordItems[record.gameRecordInfo.playerRecords[i].Index - 1];
            PlayerRecord playerRecord = record.gameRecordInfo.playerRecords[i];
            int score = GameUtil.ClacScore(playerRecord);
            item.Show(record.gameRecordInfo.playerRecords[i], score);
            if (playerRecord.Camp ==(int)PlayerCamp.Red)
            {
                redCampScore += score;
            } else if(playerRecord.Camp == (int)PlayerCamp.Blue)
            {
                blueCampScore += score;
            }
            
            if(score > maxScoreValue)
            {
                maxScoreValue = score;
                mvpIndex = i;
            }
        }
        
        ShowMvp();
        if(record.gameRecordInfo.playerRecords.Count <= 1)
        {
            return;
        }
        PlayerRecord selfRecord = record.GetPlayerRecord(PrepareData.Instance.SelfAllocateIndex);
        if(selfRecord.Camp == (int)(int)PlayerCamp.Red)
        {
            totalScoreTxt.ShowNumber(redCampScore);
            if(redCampScore > blueCampScore)
            {
                ShowWinORLose(BattleResult.WIN);
            } else if(redCampScore < blueCampScore)
            {
                ShowWinORLose(BattleResult.LOSE);
            } else
            {
                ShowWinORLose(BattleResult.DRAW);
            }
        } else if(selfRecord.Camp == (int)(int)PlayerCamp.Blue)
        {
            totalScoreTxt.ShowNumber(blueCampScore);
            if (redCampScore > blueCampScore)
            {
                ShowWinORLose(BattleResult.LOSE);
            }
            else if (redCampScore < blueCampScore)
            {
                ShowWinORLose(BattleResult.WIN);
            }
            else
            {
                ShowWinORLose(BattleResult.DRAW);
            }
        }
    }

    public void ShowWinORLose(BattleResult result)
    {
        winFlag.gameObject.SetActive(false);
        loseFlag.gameObject.SetActive(false);
        drawFlag.gameObject.SetActive(false);
        switch(result)
        {
            case BattleResult.WIN:
                winFlag.gameObject.SetActive(true);
                break;
            case BattleResult.LOSE:
                loseFlag.gameObject.SetActive(true);
                break;
            case BattleResult.DRAW:
                drawFlag.gameObject.SetActive(true);
                break;
        }
    }
}
