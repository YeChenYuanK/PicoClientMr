using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DataManager;

public class UIClientPlayerInfo : MonoBehaviour
{
  
    public Text IdT;
    public InputField NameT;
    public Image WeaponSp;
    public Text CampT;
    public Text StateT;
    private PlayerInfo Player;
    public Text Type;

    private int WeaponIndex;
    private int Campindex;
    private int Typeindex;
    public void Start()
    {
        
        NameT.onValueChanged.AddListener(OnInputValueChanged);
    }
    public void SetInfo(PlayerInfo player)
    {
        Player = player;
        IdT.text = player.Playerid.ToString()+"#";
        WeaponIndex =GameMng.Instance._prefebMng.GetWeaponInfosById(player.Weaponid).index;
        Campindex = player.Camp;
        
        if (Player.State == 0||Player.State==1)
        {
            StateT.text = GameMng.Instance._prefebMng.GetLanguageString(player.State == 0 ? "1028" : "1029");
           
        }
        else
        {
            StateT.text = GameMng.Instance._prefebMng.GetLanguageString("1031");
        }
        Typeindex = Player.HandType;
        Type.text = Player.HandType == 0 ? "H" : "P";
        NameT.text = player.PlayerName;
        Weaponinfos weapon = GameMng.Instance._prefebMng.GetWeaponSpByIndex(WeaponIndex);
        WeaponSp.sprite = weapon.sprite;

        OnChageCamp();
    }
    public void OnClickChangeCamp(int vaule)
    {
        Campindex += vaule;
        if (Campindex > 1)
            Campindex = 0;
        else if (Campindex < 0)
            Campindex = 1;
        OnChageCamp();
    }
   
    public void OnClickChangeWeapon(int vaule)
    {
        WeaponIndex += vaule;
        if (WeaponIndex > 2)
            WeaponIndex = 0;
        else if (WeaponIndex < 0)
            WeaponIndex = 2;
        OnChageWeapon();
    }
    public void OnInputValueChanged(string value)
    {
     
        if (value.Length > 8)
        {
            string truncatedText = value.Substring(0, 8);
            OnChangeName(truncatedText);
        }
        else
            OnChangeName(value);
    }
    public void OnChageWeapon()
    {
        Weaponinfos weapon= GameMng.Instance._prefebMng.GetWeaponSpByIndex(WeaponIndex);
        WeaponSp.sprite = weapon.sprite;
        GameMng.Instance._playerInfoMng.ChangeWeaponid(Player.Playerid, weapon.key);
    }
    public void OnChangeName(string name)
    {
       
        GameMng.Instance._playerInfoMng.ChangeName(Player.Playerid, name);
    }
    public void OnChageCamp() 
    {
        Color color = Campindex == 0? Color.red : Color.blue;
        CampT.color = color;
        CampT.text = GameMng.Instance._prefebMng.GetLanguageString(Campindex == 0 ? "1002" : "1001");
        GameMng.Instance._playerInfoMng.ChangeCamp(Player.Playerid, Campindex);
    }
    public void OnChangeType()
    {
        Typeindex = Typeindex == 0 ? 1 : 0;
        Type.text = Typeindex == 0 ? "H" : "P";
        GameMng.Instance._playerInfoMng.ChangeHandType(Player.Playerid, Typeindex);
    }
 

}
