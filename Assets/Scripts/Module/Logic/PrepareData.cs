using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareData {

    private static PrepareData instance;
    public static PrepareData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PrepareData();
            }
            return instance;
        }
    }

    public int SelfAllocateIndex = 0;

    public int MapId = 1;

    public string RoomName = "prename";

    public string PlayerName = "";

    public string AllocatePhotonAddress { get; set; }

    public int Camp = 1;

    public int GameTime = 600;

    public int WeaponId = 1001001;

    public int GunHandType = 1; //1 是手柄 2 是PPGUN

    public GameRecord GameRecord { get; set; }

    public float LastKillTime;
    public int KillNum;

    
   

}
