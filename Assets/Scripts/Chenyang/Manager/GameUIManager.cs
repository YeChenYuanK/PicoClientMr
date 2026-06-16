using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;
public class GameUIManager : MonoBehaviour
{

    private GameMng _GameMng;
    public GameMng Mng
    {
        get
        {
            if (_GameMng == null)
                _GameMng = GameMng.Instance;
            return _GameMng;
        }
    }
    public List<UIBasePanel> UIBasePanel = new List<UIBasePanel>();
    private int m_PanelStete;
    public void OnInit()
    {
        m_PanelStete = (int)PanelState.None;
        if (Mng.isServerHost)
        {
            UIBasePanel[(int)PanelState.UIMessagePanel].OnEnter();
            OnChangePanel(PanelState.UIServerRoomPanel);
            //OpenServerOverPanel();
        }
    }
    public void OnChangePanel(object panelstate)
    {
        var Panelstate = (int)panelstate;

        if (Panelstate != m_PanelStete)
        {
            if (m_PanelStete != (int)PanelState.None)
                UIBasePanel[m_PanelStete].OnExit();
            m_PanelStete = Panelstate;
            if (m_PanelStete != (int)PanelState.None)
                UIBasePanel[m_PanelStete].OnEnter();
        }
    }
    public void OnChangePanel()
    {
        if (m_PanelStete == (int)PanelState.UIServerRoomPanel)
            OnChangePanel(PanelState.UIServerRankPanel);
        else
            OnChangePanel(PanelState.UIServerRoomPanel);

    }

    public void OpenServerOverPanel()
    {
        UIBasePanel[(int)PanelState.UIServerOverPanel].OnEnter();
    }
}
