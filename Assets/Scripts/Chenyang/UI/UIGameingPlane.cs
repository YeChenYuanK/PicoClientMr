using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameingPlane : UIBasePanel
{
    public Text TimeT;
    public Text AllPlayerRedScore;
    public Text AllPlayerBlueScore;
    public List<UIRankPlayerInfo> RedRankInfos = new List<UIRankPlayerInfo>();
    public List<UIRankPlayerInfo> BlueRankInfos = new List<UIRankPlayerInfo>();

    public override void OnEnter()
    {
        GameMng.Instance.RefreshUI += RefreshUI;
        GameMng.Instance.RefreshHpUI += RefreshHpUI;
        RefreshUI();
        gameObject.SetActive(true);
    }
  
    public void RefreshUI()
    {
       
        List<PlayerInfo> Red = GameMng.Instance._playerInfoMng.GetPlayInfoListByCamp(0);
        List<PlayerInfo> Blue = GameMng.Instance._playerInfoMng.GetPlayInfoListByCamp(1);
       string red= SetUIRank(RedRankInfos, Red).ToString();            
        string blue= SetUIRank(BlueRankInfos, Blue).ToString();
        AllPlayerRedScore.text = red;
        AllPlayerBlueScore.text = blue;


    }
    public void Update()
    {
        TimeT.text = ConvertSecondsToMMSS(GameMng.Instance.GameTime);
    }
    public static string ConvertSecondsToMMSS(int seconds)
    {
        // 计算分钟和秒数
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        // 格式化为两位数，不足补零
        return string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
    }
    public int SetUIRank(List<UIRankPlayerInfo> rank, List<PlayerInfo> infos)
    {
        int score = 0;
        
        for (int i = 0; i < rank.Count; i++)
        {
            if (i < infos.Count)
            {
                rank[i].SetInfo(infos[i]);
                score+= GameUtil.CountScore(infos[i]);
                rank[i].gameObject.SetActive(true);
            }
            else
            {
                rank[i].gameObject.SetActive(false);
            }
        }

        return score;

    }
    public void RefreshHpUI(int playerid, int hp)
    {
        PlayerInfo info = GameMng.Instance._playerInfoMng.GetPlayerInfoById(playerid);
        if (info != null)
        {
            if (info.Camp == 0)
                GetUIRank(RedRankInfos, info.Playerid,hp);
            else
                GetUIRank(BlueRankInfos, info.Playerid, hp);
        }
    }
    public void GetUIRank(List<UIRankPlayerInfo> rank, int id,int hp)
    {
        for (int i = 0; i < rank.Count; i++)
        {
            if (rank[i]._info.Playerid == id && rank[i].gameObject.activeInHierarchy)
            {
                rank[i].SetBackGround(hp);
                return;
            }
        }
       
    }
    public override void OnExit()
    {
        GameMng.Instance.RefreshUI -= RefreshUI;
        GameMng.Instance.RefreshHpUI -= RefreshHpUI;
        gameObject.SetActive(false);
    }

   
}
