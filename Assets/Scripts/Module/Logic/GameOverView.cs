using com.leke.redSea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DataManager;

public class GameOverView : MonoBehaviour {

    public List<GameOverItem> Reditems=new List<GameOverItem>();
    public List<GameOverItem> Blueitems=new List<GameOverItem>();



    public NumberShow team1Score;
    public NumberShow team2Score;

    public int TeamPlayerCount = 3;
    int red;
    int blue;
    void Start ()
    {
      
        Show();
    }
    public void Show()
    {
        red = 0;
        blue = 0;
        List<PlayerInfo> Red = GameMng.Instance._playerInfoMng.GetPlayInfoListByCamp(0);
        List<PlayerInfo> Blue = GameMng.Instance._playerInfoMng.GetPlayInfoListByCamp(1);
        int mvpid = GameMng.Instance._playerInfoMng.Getmvp();
        for (int i = 0; i < Reditems.Count; i++)
        {
            if (i < Red.Count)
            {
                if (Red[i].Playerid == mvpid)
                    Reditems[i].Show(Red[i], true);
                else
                    Reditems[i].Show(Red[i]);

                red += GameUtil.CountScore(Red[i]);

                Reditems[i].gameObject.SetActive(true);
            }
            else
            {
                Reditems[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < Blueitems.Count; i++)
        {
            if (i < Blue.Count)
            {
                if (Blue[i].Playerid == mvpid)
                    Blueitems[i].Show(Blue[i], true);
                else
                    Blueitems[i].Show(Blue[i]);
                blue += GameUtil.CountScore(Blue[i]);
                Blueitems[i].gameObject.SetActive(true);
            }
            else
            {
                Blueitems[i].gameObject.SetActive(false);
            }
        }
        team1Score.ShowNumber(red);
        team2Score.ShowNumber(blue);
    }
        
}
