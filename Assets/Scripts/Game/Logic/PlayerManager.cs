using UnityEngine;
using System.Collections;
using com.gamestudio.cs;
using System.Collections.Generic;
using System;

public class PlayerManager : MonoBehaviour {

    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get { return instance; }
    }

    private List<BaseCharacter> allPlayers;
	public List<BaseCharacter> AllPlayers
	{
		get
		{
			return allPlayers;
		}
	}
	private SelfCharacter selfCharacter;
	public SelfCharacter GetSelfCharacter
	{
		get 
		{
			return selfCharacter;
		}
	}
    private UnityEngine.Object otherCharacter;
    public PlayerManager()
    {
        instance = this;
        allPlayers = new List<BaseCharacter>();
    }

	// Use this for initialization
	void Start () {
        otherCharacter = Resources.Load("Prefabs/Humanoid/OtherCharacter");
    }
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public void CreateSelf(FightUnit unit)
    {
        BasePoint point = TeleporterManager.Instance.FindBasePoint(unit.BirthIndex);
        if(point == null)
        {
            Debug.Log("point can not found : " + unit.BirthIndex);
            return;
        }

        UnityEngine.Object obj = Resources.Load("Prefabs/Humanoid/SelfCharacter");
        GameObject selfObj = GameObject.Instantiate(obj) as GameObject;

        selfObj.transform.SetParent(point.telePorter.transform);
        selfCharacter = selfObj.GetComponent(typeof(SelfCharacter)) as SelfCharacter;
        selfCharacter.unitId = unit.UnitId;

        allPlayers.Add(selfCharacter);
    }

    public void CreateOthers(List<FightUnit> units)
    {
		UnityEngine.Object otherCharacter = Resources.Load("Prefabs/Humanoid/OtherCharacter"); 
        foreach (FightUnit fightUnit in units)
        {
            CreateOther(fightUnit);
        }
    }
    
    public void CreateOther(FightUnit fu)
    {
  //      GameObject otherObj = GameObject.Instantiate(otherCharacter) as GameObject;
  //      BasePoint point = TeleporterManager.Instance.FindBasePoint(fu.CurIndex);
  //      if (point == null)
  //      {
  //          Debug.Log("point can not found : " + fu.CurIndex);
  //          return;
  //      }
  //      otherObj.transform.SetParent(point.telePorter.transform);
  //      otherObj.transform.localPosition = Vector3.zero;
  //      OtherCharactor otherCharactor = otherObj.GetComponent<OtherCharactor>() as OtherCharactor;
		//otherObj.GetComponent<CharacterSkin> ().Init((Camp)fu.Camp);
  //      otherCharactor.unitId = fu.UnitId;
		//otherCharactor.camp = fu.Camp;
  //      allPlayers.Add(otherCharactor);
    }

    public void SyncFightUnit(FightUnit fu)
    {
    }

    public BaseCharacter GetCharacter(int unitId)
    {
        foreach(BaseCharacter bc in this.allPlayers)
        {
            if(bc.unitId == unitId)
            {
                return bc;
            }
        }
        return null;
    }

    public void RemovePlayer(int playerId)
    {
        BaseCharacter player = this.GetCharacter(playerId);
        player.Destory();
        this.allPlayers.Remove(player);
    }
}
