using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Random = UnityEngine.Random;

public class TestPlayer : NetworkBehaviour
{
    public TestPlayerInfo PlayerInfo;

    public override void OnStartLocalPlayer()
    {
      
    }
    //[Command]
    //public void CmdAddPlayer(CSPlayer player)
    //{
    //    Debug.LogError("CHENYANG");
    //    GameMng.Instance._playerInfoMng.AddPlayer(player);
    //    GameMng.Instance.RefreshUI?.Invoke();
    //    RpcAddPlayer(player);
    //}

    [Command]
    public void CmdChangeName()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
