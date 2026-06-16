using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;

public class PrefebManager : MonoBehaviour
{
    public LanguageList _LanguageList;
    public string GetLanguageString(string key)
    {
        foreach (var item in _LanguageList.LanStrLists)
        {
            if (item.Lan_key == key)
            {
                string str="";
                switch (GameMng.Instance.LanguageState)
                {
                    case 0:
                        str= item.Lan_China;
                        break;
                    case 1:
                        str = item.Lan_English;
                        break;
                    case 2:
                        str = item.Lan_French;
                        break;
                    case 3:
                        str = item.Lan_Arab;
                        break;
                    case 4:
                        str = item.Lan_Spain;
                        break;
                    default:
                        break;
                }
                return str;
            }
        }
        return null;
    }
    public Sprite GetLanguageSpring(string key)
    {
        foreach (var item in _LanguageList.LanSpLists)
        {
            if (item.Lan_Key == key)
            {
                Sprite sp = null;
                switch (GameMng.Instance.LanguageState)
                {
                    case 0:
                        sp = item.Lan_China;
                        break;
                    case 1:
                        sp = item.Lan_English;
                        break;
                    case 2:
                        sp = item.Lan_French;
                        break;
                    case 3:
                        sp = item.Lan_Arab;
                        break;
                    case 4:
                        sp = item.Lan_Spain;
                        break;
                    default:
                        break;
                }
                return sp;
            }
        }
        return null;
    }

    public UIResList _UIResList;
    public Weaponinfos GetWeaponInfosById(int key)
    {
        foreach (var item in _UIResList.Weaponinfos)
        {
            if (item.key == key)
                return item;
        }
        return null;
    }
    public Weaponinfos GetWeaponSpByIndex(int index)
    {
        foreach (var item in _UIResList.Weaponinfos)
        {
            if (item.index == index)
                return item;
        }
        return null;
    }
    public Mapinfo GetMapInfoByIndex(int index)
    {
        if (index < _UIResList.MapInfos.Count)
            return _UIResList.MapInfos[index];
        return null;
    }
    public string GetCampNameByIndex(int index)
    {
        if (index < _UIResList.CampName.Count)
            return _UIResList.CampName[index];
        return null;
    }

    public List<Sprite> BackGound = new List<Sprite>();
    public List<Sprite> CampBG = new List<Sprite>();
}
