using UnityEngine;
using System.Collections;
using System;
using LekeNet;

public class BasePlayer : MonoBehaviour {

    /// <summary>
    /// 玩家Id
    /// </summary>
    public int unitId;

    private int tick = 0;

    private SelfCharacter selfCharacter ;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixUpdate()
    {
        
    }

    public void SyncFightUnit(FightUnit fu)
    {
        BasePoint point = TeleporterManager.Instance.FindBasePoint(fu.CurIndex);
        if(point == null)
        {
            return;
        }
        this.transform.SetParent(point.telePorter.transform);
        if(fu.Position != null)
        {
            transform.position = fu.Position;
        }
    }

   
}
