using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerSpcThing : MonoBehaviour
{
    public CSPlayer Player;
    public GameObject DieT;
   
    public void OnTriggerEnter(Collider other)
    {
        if (GameMng.Instance.isGameState(0)) return;
        if (Player.isDie) return;
        Player.spcDie();
        ShowT();
    }
    public void ShowT()
    {
        CancelInvoke("HideT");
        DieT.SetActive(true);
        Invoke("HideT", 3f);
    }
    public void HideT()
    {
        DieT.SetActive(false);
    }
}
