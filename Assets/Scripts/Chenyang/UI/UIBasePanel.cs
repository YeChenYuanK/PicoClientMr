using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;

public abstract class UIBasePanel : MonoBehaviour
{
   
    public PanelState _PanelState;
    public abstract void OnEnter();

    public abstract void OnExit();
    

}
