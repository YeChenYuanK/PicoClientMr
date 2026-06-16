using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class UIServerOverPanel: UIBasePanel
{
    [SerializeField] private List<UICheckoutItem> redItems = new List<UICheckoutItem>();
    [SerializeField] private List<UICheckoutItem> blueItems = new List<UICheckoutItem>();
   
    public Transform redTeamScore;
    public Transform blueTeamScore;
    public Transform redTeamParent;
    public Transform blueTeamParent;
    public UICheckoutItem itemPrefab;
    public int TeamPlayerCount = 10;
    public Sprite[] numImages;
    public override void OnEnter()
    {
        CacheInitData();
        
        var playerInfoMng = GameMng.Instance._playerInfoMng;
        if (playerInfoMng == null) return;
        int mvpId = playerInfoMng.Getmvp();
        SetItemData(playerInfoMng.GetPlayInfoListByCamp(0), playerInfoMng.GetPlayInfoListByCamp(1), mvpId);
        
        //TestPlayerInfo();
        
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
    
    public void OnCloseBtnClick()
    {
        gameObject.SetActive(false);
    }

    void CacheInitData()
    {
        if (redItems.Count == TeamPlayerCount && blueItems.Count == TeamPlayerCount)
            return;
        
        for (int i = 0; i < TeamPlayerCount; i++)
        {
            UICheckoutItem item = Instantiate(itemPrefab, redTeamParent);
            item.gameObject.SetActive(false);
            item.transform.localScale = Vector3.one;
            redItems.Add(item);
        }

        for (int i = 0; i < TeamPlayerCount; i++)
        {
            UICheckoutItem item = Instantiate(itemPrefab, blueTeamParent);
            item.gameObject.SetActive(false);
            item.transform.localScale = Vector3.one;
            blueItems.Add(item);
        }
    }
    
    private void SetItemData(List<PlayerInfo> redPlayers, List<PlayerInfo> bluePlayers, int mvpId)
    {
        int redTotalScore = UpdateTeamItems(redItems, redPlayers, mvpId);
        int blueTotalScore = UpdateTeamItems(blueItems, bluePlayers, mvpId);

        Debug.Log("redTotalScore:" + redTotalScore+" blueTotalScore:" + blueTotalScore);
        ShowNumber(redTotalScore, redTeamScore.transform);
        ShowNumber(blueTotalScore, blueTeamScore.transform);
    }

    private int UpdateTeamItems(List<UICheckoutItem> items, List<PlayerInfo> datas, int mvpId)
    {
        int totalScore = 0;
        if (items == null) return totalScore;

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item == null) continue;

            if (datas != null && i < datas.Count)
            {
                var data = datas[i];
                bool isMvp = (data.Playerid == mvpId);
                
                item.SetData(data, isMvp);
                totalScore += GameUtil.CountScore(data);
                item.gameObject.SetActive(true);
            }
            else
            {
                //item.gameObject.SetActive(false);
                item.ClearData();
            }
        }
        return totalScore;
    }
    
    #region 测试代码
    void TestPlayerInfo()
    {
        List<PlayerInfo> redPlayers = new List<PlayerInfo>();
        List<PlayerInfo> bluePlayers = new List<PlayerInfo>();
        CreatePlayerInfo(redPlayers, 0, 1, "吧啦吧啦吧啦吧啦", 1, 1, 1, 1);
        CreatePlayerInfo(redPlayers, 0, 2, "玩家2", 12, 2, 12, 12);
        CreatePlayerInfo(redPlayers, 0, 3, "阿啦啦啦啦", 123, 3, 123, 123);
        CreatePlayerInfo(redPlayers, 0, 4, "玩家4", 1234, 4, 1234, 1234);
        CreatePlayerInfo(redPlayers, 0, 5, "玛丽丽丽丽丽", 12345, 5, 12345, 12345);

        CreatePlayerInfo(bluePlayers, 1, 6, "玩家6", 1, 1, 1, 1);
        CreatePlayerInfo(bluePlayers, 1, 7, "玩家7", 1, 12, 2, 2);
        CreatePlayerInfo(bluePlayers, 1, 8, "阿中重中之重", 1, 123, 3, 3);
        CreatePlayerInfo(bluePlayers, 1, 9, "玩家9", 1, 1234, 4, 1);
        CreatePlayerInfo(bluePlayers, 1, 10, "玩家10", 1, 1235, 10, 2);
        
        SetItemData(redPlayers, bluePlayers, 7);
    }
    void CreatePlayerInfo(List<PlayerInfo> players, int camp, int playerId, string playerName, int weaponId, int kills, int deads, int headShots)
    {
        PlayerInfo info = new PlayerInfo();
        info.Playerid = playerId;
        info.PlayerName = playerName;
        info.Camp = camp;
        info.Weaponid = weaponId;
        info.Kills = kills;
        info.Deads = deads;
        info.HeadShots = headShots;
        info.State = 0;
        info.Hp = 100;
        info.HandType = 0;
        players.Add(info);
    }
    #endregion
    
    void ShowNumber(int num, Transform parent)
    {
        if (parent == null || numImages == null || numImages.Length == 0)
            return;

        string numStr = num.ToString();
        int length = numStr.Length;
        int childCount = parent.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child == null) continue;

            Image image = child.GetComponent<Image>();
            if (image == null) continue;

            if (i < length)
            {
                int digit = numStr[i] - '0';
                if (digit >= 0 && digit < numImages.Length)
                {
                    image.sprite = numImages[digit];
                    image.gameObject.SetActive(true);
                }
                else
                {
                    image.gameObject.SetActive(false);
                }
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }

}
