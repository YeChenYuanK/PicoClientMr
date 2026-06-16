using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class UIServerGamePanel : UIBasePanel
{
    public List<UIClientPlayerInfo> PlayerInfos = new List<UIClientPlayerInfo>();

    private int MapIndex;
    public Image MapSp;
    public Text MapT;
    public Text Message;
    public GameObject StartButton;
   
    public void UpdatePlayerUIList()
    {
        int index = 0;
        foreach (var player in GameMng.Instance._playerInfoMng.PlayersInfos)
        {
            Debug.Log("UI====ID=====" + player.Key);
            Debug.Log("UI====名字======" + player.Value.PlayerName);
            PlayerInfos[index].SetInfo(player.Value);
            PlayerInfos[index].gameObject.SetActive(true);
            index++;
        }
        for (int i = index; i < PlayerInfos.Count; i++)
        {
            PlayerInfos[i].gameObject.SetActive(false);
        }
        
    }
    public void OnClickChangeMap(int vaule)
    {
      
        MapIndex += vaule;
        if (MapIndex > GameMng.Instance._prefebMng._UIResList.MapInfos.Count-1)
            MapIndex = 0;
        else if (MapIndex < 0)
            MapIndex = GameMng.Instance._prefebMng._UIResList.MapInfos.Count - 1;

        var mapinfo = GameMng.Instance._prefebMng.GetMapInfoByIndex(MapIndex);
       
        MapT.text = GameMng.Instance._prefebMng.GetLanguageString(mapinfo.MapName);
        MapSp.sprite = mapinfo.MapSprite;
    }
    public void OnClickStartGame()
    {
        if (GameMng.Instance._playerInfoMng.Players.Count > 0)
        {

            if (GameMng.Instance._playerInfoMng.IsAllSceneReady())
            {
                GameMng.Instance.GameStart(MapIndex);
                StartButton.SetActive(false);
            }
            else
            {
                Message.text = GameMng.Instance._prefebMng.GetLanguageString("1029");
                Message.enabled = true;
                Invoke("OnEnableMessage", 2);
            }
        }
        else
        {
            Message.text = GameMng.Instance._prefebMng.GetLanguageString("1030");
            Message.enabled = true;
            Invoke("OnEnableMessage", 2);
        }
    }
    public void OnEnableMessage()
    {
        Message.enabled = false;
    }
    public void RefreshTeamUI()
    {
        OnClickChangeMap(0);
        UpdatePlayerUIList();
    }
    public override void OnEnter()
    {
        GameMng.Instance.RefreshUI += RefreshTeamUI;
        GameMng.Instance.ChangeLanguage += RefreshTeamUI;
        OnClickChangeMap(0);
        gameObject.SetActive(true);
    }
    
    public override void OnExit()
    {
        GameMng.Instance.RefreshUI -= RefreshTeamUI;
        GameMng.Instance.ChangeLanguage -= RefreshTeamUI;
        gameObject.SetActive(false);
    }
}
