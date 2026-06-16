using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;

[CreateAssetMenu(fileName = "LanguageList", menuName = "ListMenu/LanguageList")]

public class LanguageList : ScriptableObject
{
    public List<LanguageStringList> LanStrLists = new List<LanguageStringList>();
    public List<LanguageSpriteList> LanSpLists = new List<LanguageSpriteList>();
}
