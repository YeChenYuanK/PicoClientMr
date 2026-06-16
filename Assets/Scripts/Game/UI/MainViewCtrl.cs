using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainViewCtrl : MonoBehaviour {

    private void Awake()
    {
        /*
        Debug.Log("start load main view ");
        AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetsBundle/mainview");
        Debug.Log("start load main suc");
        Object[] obj = ab.LoadAllAssets<GameObject>();//加载出来放入数组中
        //创建Mainview
        GameObject mainViewObj = (GameObject)GameObject.Instantiate(obj[0]);
        mainViewObj.transform.parent = this.transform;
        mainViewObj.transform.localPosition = Vector3.zero;
        mainViewObj.transform.localScale = new Vector3(1,1,1);
        */
    }


    public void ShowOrHide()
    {
        if (MainView.Instance != null)
        {
            MainView.Instance.ShowOrHide();
        }
    }

    public void ChangeCameraView()
    {
        if (PlayerCameraViewCtrl.Instance != null)
        {
            PlayerCameraViewCtrl.Instance.ChangeNextView();
        }
    }
}
