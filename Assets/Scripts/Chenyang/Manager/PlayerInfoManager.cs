using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : BaseManager
{

    public List<CSPlayer> Players = new List<CSPlayer>();
    public CSPlayer _mySelfPlayer;
    public int MySelfCamp;
    public Dictionary<int, PlayerInfo> PlayersInfos = new Dictionary<int, PlayerInfo>();
    public Dictionary<int, PlayerInfo> PlayersRecordsInfos = new Dictionary<int, PlayerInfo>();
    public List<int> WinScores = new List<int>();
    public int Kill = 0;
    public bool isFristblood()
    {
        if (Kill == 0)
        {
            Kill += 1;
            return true;
        }
        return false;
    }
    public int Getmvp()
    {
        int id = -1;
        int score = -1;
        foreach (var item in PlayersInfos)
        {
            if (score == -1)
            {
                score = GameUtil.CountScore(item.Value);
                id = item.Key;
            }
            else
            {
                if (score < GameUtil.CountScore(item.Value))
                {
                    score = GameUtil.CountScore(item.Value);
                    id = item.Key;
                }
            }
        }
        return id;
    }
    public int ClinetEndCountRecords()
    {
        PlayersInfos.Clear();
       // Debug.LogError(Players.Count);
        for (int i = 0; i < Players.Count; i++)
        {
            PlayerInfo Info = new PlayerInfo();
            Info.Playerid = Players[i].playerId;
            Info.Weaponid = Players[i].WeaponId;
            Info.Camp = Players[i].Camp;
            Info.PlayerName = Players[i].PlayerName;
            Info.Kills = Players[i].kills;
            Info.HeadShots = Players[i].shotHead;
            Info.Deads = Players[i].Dead;
           // Debug.Log("tianjia======"+Players[i].playerId);
            PlayersInfos.Add(Players[i].playerId,Info);
           
        }
        return EndCountRecords();
    }
    public int EndCountRecords()
    {
        int red = 0;
        int Blue = 0;
        foreach (var item in PlayersInfos)
        {
            if (item.Value.Camp == 0)
            {
                red += GameUtil.CountScore(item.Value);
            }
            else
            {
                Blue += GameUtil.CountScore(item.Value);

            }
            if (PlayersRecordsInfos.ContainsKey(item.Key))
            {
                PlayerInfo Info = PlayersRecordsInfos[item.Key];
                Info.Kills += item.Value.Kills;
                Info.Deads += item.Value.Deads;
                Info.HeadShots += item.Value.HeadShots;
                Info.Weaponid = item.Value.Weaponid;
                Info.Camp = item.Value.Camp;
                Info.PlayerName = item.Value.PlayerName;
            }
            else
            {
                PlayerInfo Info = new PlayerInfo();
                Info.Playerid = item.Value.Playerid;
                Info.Weaponid = item.Value.Weaponid;
                Info.Camp = item.Value.Camp;
                Info.PlayerName = item.Value.PlayerName;
                Info.Kills = item.Value.Kills;
                Info.HeadShots = item.Value.HeadShots;
                Info.Deads = item.Value.Deads;
                PlayersRecordsInfos.Add(Info.Playerid, Info);

            }
        }
        if (red > Blue)
        {
           
            WinScores[0] += 1;
            return 0;
        }
        else if (red < Blue)
        {
            WinScores[1] += 1;
            return 1;
        }
        else
        {
            return 2;
            //WinScores[0] += 1;
            //WinScores[1] += 1;
        }
        
    }
   
    public List<PlayerInfo> GetPlayInfoListByCamp(int camp)
    {
        List<PlayerInfo> red=new List<PlayerInfo>();
        foreach (var item in PlayersInfos)
        {
            if (item.Value.Camp == camp)
                red.Add(item.Value);
        }
        return red;
    }
    public void Clear()
    {
        Kill = 0;
        Players.Clear();
        if (PlayersInfos.Count <= 0) return;
        foreach (var item in PlayersInfos)
        {
            item.Value.Kills = 0;
            item.Value.HeadShots = 0;
            item.Value.Deads = 0;
        }
    }

    public void ChangeCamp(int camp)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].otherCharacter.ShowName(true);
        }
    }
    public void AddInfos(int id, PlayerInfo info)
    {
        if (!PlayersInfos.ContainsKey(id))
            PlayersInfos.Add(id, info);
    }
    public PlayerInfo GetMySelfInfo()
    {
        if (PlayersInfos.ContainsKey(mySelfId))
            return PlayersInfos[mySelfId];
        return null;
    }
   
    public PlayerInfo GetPlayerInfoById(int id)
    {
       // Debug.LogError("Playerinfo数量=====" + PlayersInfos.Count);
        if (PlayersInfos.ContainsKey(id))
            return PlayersInfos[id];
        return null;
    }
    public int mySelfId=-1;
  
    public PlayerInfoManager(GameMng mng) : base(mng)
    {
        for (int i = 0; i < 2; i++)
        {
            WinScores.Add(0);
        }
       
}
   
    public int PlayersCount
    {
        get { return Players.Count; }
    }
    public CSPlayer GetPlayerById(int index)
    {
        for (int i = 0; i < Players.Count; i++)
        {
           
            if (Players[i].playerId == index)
            {
             
                return Players[i];
            }
        }
       
        return null;
    }
    public bool ChectkPlayerDie(int Camp)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].Camp == Camp)
            {
                if (!Players[i].isDie)
                    return false;
            }
        }
        return true;
    }
    public CSPlayer GetPlayerByindex(int index)
    {

            if(index<Players.Count)
            {
                return Players[index];
            }
        

        return null;
    }
    public void AddPlayer(CSPlayer Player)
    {
        if (!Players.Contains(Player))
        {
            Players.Add(Player);
        }
    }
    public void ChangeCamp(int Playerid, int newCamp)
    {
        CSPlayer p= GetPlayerById(Playerid);
        
        if (p != null)
        {
         
            if (PlayersInfos.ContainsKey(Playerid))
            {
                PlayersInfos[Playerid].Camp = newCamp;
            }
            p.Camp=newCamp;
           p.otherCharacter.OnChangeCamp(newCamp);
        }
    }
    public bool IsAllSceneReady()
    {
        foreach (var item in Players)
        {
            if (item.PlayerState == 1)
            {
                return false;
            }
        }
        return true;
          
    }
    public void ChnageState(int Playerid, int state)
    {
        CSPlayer p = GetPlayerById(Playerid);
        if (p != null)
        {
            if (PlayersInfos.ContainsKey(Playerid))
            {
                PlayersInfos[Playerid].State = state;
            }
        }
    }
    public void ChangeName(int Playerid, string name)
    {
        CSPlayer p = GetPlayerById(Playerid);
        if (p != null)
        {
            if (PlayersInfos.ContainsKey(Playerid))
            {
                PlayersInfos[Playerid].PlayerName = name;
            }
            p.PlayerName=name;
            p.otherCharacter.OnPlayerNameChange(name);
        }
    }
    public void ChangeHandType(int Playerid, int HandType)
    {
        CSPlayer p = GetPlayerById(Playerid);
        if (p != null)
        {
            if (PlayersInfos.ContainsKey(Playerid))
            {
                PlayersInfos[Playerid].HandType = HandType;
            }
            p.HandType = HandType;
          
        }
    }
    public void ServerChangeWeaponId(int Playerid, int weaponid)
    {
    
            if (PlayersInfos.ContainsKey(Playerid))
            {
                PlayersInfos[Playerid].Weaponid = weaponid;
            }
        
    }
    public void ChangeWeaponid(int Playerid, int weaponid)
    {
        CSPlayer p = GetPlayerById(Playerid);
        if (p != null)
        {
            p.WeaponId=weaponid;
           p.gunController.ChangeGunById(weaponid);
        }
    }
    public void SetHp(int playerid, int hp)
    {
       PlayerInfo player= GetPlayerInfoById(playerid);
        if (player != null)
            player.Hp = hp;

        Mng.RefreshHpUI?.Invoke(playerid,hp);
    }
}
public class PlayerInfo
{
    public int Playerid;
    public string PlayerName;
    public int Camp;
    public int Weaponid;
    public int State;
    public int Kills;
    public int Deads;
    public int HeadShots;
    public int Hp;
    public int HandType;
    
}
