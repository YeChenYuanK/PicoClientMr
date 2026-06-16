using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DataManager 
{
    [Serializable]
    public class Mapinfo 
    {
        public Sprite MapSprite;
        public string MapName;
        public string MapScene;
    }
    [Serializable]
    public class LanguageStringList
    {
        public string Lan_key;
        public string Lan_China;
        public string Lan_English;
        public string Lan_French;
        public string Lan_Arab;
        public string Lan_Spain;
    }
    [Serializable]
    public class LanguageSpriteList
    {
        public string Lan_Key;
        public Sprite Lan_China;
        public Sprite Lan_English;
        public Sprite Lan_French;
        public Sprite Lan_Arab;
        public Sprite Lan_Spain;

    }
    [Serializable]
    public class Weaponinfos
    {
        public int key;
        public Vector3 Pos;
        public Vector3 Rot;
        public int index;
        public Sprite sprite;
    }
    public enum PanelState
    {    
        UIServerRoomPanel,
        UIServerRankPanel,
        UIMessagePanel,
        UIServerOverPanel,
        None,
    }
    public enum BodyState
    {
      Body_Head,
      Body_Left,
      Body_Right,
    }
    public enum PlayerCamp
    {
        Red,
        Blue,
        None,

    }
    public enum GameState
    {
        GAME_PREPARE,
        GAME_BATTLE,
        GAME_MIDDLE,
        GAME_OVER,
    }
}
