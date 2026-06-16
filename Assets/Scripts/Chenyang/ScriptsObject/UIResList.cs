using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;

[CreateAssetMenu(fileName = "UIResList", menuName = "ListMenu/UIResList")]

public class UIResList : ScriptableObject
{
   
    public List<Mapinfo> MapInfos = new List<Mapinfo>();
    public List<string> CampName = new List<string>();
    public List<Weaponinfos> Weaponinfos = new List<Weaponinfos>();

}
