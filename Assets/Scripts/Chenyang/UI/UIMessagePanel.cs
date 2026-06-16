using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessagePanel : UIBasePanel
{
    public PlayerCameraViewCtrl viewCtrl;
    public Dropdown LanguageDropdown;
    public override void OnEnter()
    {
        LanguageDropdown.onValueChanged.AddListener(OnLanguageChange);
        LanguageDropdown.value = GameMng.Instance.LanguageState;
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        LanguageDropdown.onValueChanged.RemoveListener(OnLanguageChange);
        gameObject.SetActive(false);
    }
    public void OnClickChangeUIBtn()
    {
      
            GameMng.Instance._uiMng.OnChangePanel();
        
    }
    public void OnClickChangeCamera()
    {
   
            viewCtrl.ChangeCamera();
        
    }
    public void OnClickChangePlayer()
    {
        viewCtrl.ChangeNextView();
    }
    public void OnLanguageChange(int selectindex)
    {
        GameMng.Instance.ChangeLan(selectindex);
    }

  
}
