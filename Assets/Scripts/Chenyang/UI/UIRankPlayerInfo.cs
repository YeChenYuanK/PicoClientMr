using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankPlayerInfo : MonoBehaviour
{
    public Text NameT;
    public Text KillT;
    public Text DeadT;
    public Image Hp;
    public Image BackGround;
    public Image CampSp;
    public Image WeapOn;
    
    public PlayerInfo _info;
    public void SetInfo(PlayerInfo info)
    {
        _info = info;
        NameT.text = _info.PlayerName;
        KillT.text = (_info.Kills+_info.HeadShots).ToString();
        DeadT.text = _info.Deads.ToString();
        WeapOn.sprite = GameMng.Instance._prefebMng.GetWeaponInfosById(_info.Weaponid).sprite;
        SetBackGround(info.Hp);

    }
    public void SetBackGround(int hp)
    {
        Hp.fillAmount = _info.Hp / SystemData.MAXHP;
        int index = hp > 0 ? _info.Camp: 2;
        BackGround.sprite = GameMng.Instance._prefebMng.BackGound[index];
        int Campindex = hp > 0 ? _info.Camp : _info.Camp+2;
        CampSp.sprite = GameMng.Instance._prefebMng.CampBG[Campindex];
    }
}
