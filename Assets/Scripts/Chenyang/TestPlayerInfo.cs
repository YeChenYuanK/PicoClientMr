using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
[Serializable]
public class TestPlayerInfo : NetworkBehaviour
{
        [SyncVar]
        public string PlayerName;
        [SyncVar]
        public int playerId;
        [SyncVar]
        public int weaponId;
        [SyncVar]
        public int Camp;
        [SyncVar]
        public int PlayerState;

       private TestPlayer testPlayer;
    public void Init(TestPlayer player)
    {
        testPlayer = player;
    }

  }
