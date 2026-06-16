using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ParametersManager;

[System.Serializable]
public class ScenePrefabDefine
{
    public string SceneSize = "7X12";
    public GameObject PrefabObject;
}

[System.Serializable]
public class SceneObjDefine
{
    public string SceneSize = "7X12";
    [Header("需要展示的物件列表")]
    public List<GameObject> ShowArray;
}

public class LogicLoad : MonoBehaviour
{
    public List<ScenePrefabDefine> scenePrefabDefines;

    public List<SceneObjDefine> sceneObjDefines;
    public GameObject EndUI;
    public GameObject prepareUI;
    private bool InitScene;
    private int UpdateFrame;
    void Start()
    {
      

        if (GameMng.Instance.FightCount > 0 && GameMng.Instance.isGameState(0))
        {
            if (EndUI != null&&prepareUI!=null)
            {
                EndUI.SetActive(true);
                prepareUI.SetActive(false);
            }
        }


        //for(int i=0;i< sceneObjDefines.Count; i++)
        //{
        //    SceneObjDefine sceneObjDefine = sceneObjDefines[i];
        //    if (sceneObjDefine.SceneSize.Trim().ToLower().Equals(sceneSize))
        //    {
        //        for(int j=0;j< sceneObjDefine.ShowArray.Count;j++)
        //        {
        //            sceneObjDefine.ShowArray[j].SetActive(true);
        //        }
        //    } 
        //    /*else
        //    {
        //        for (int j = 0; j < sceneObjDefine.ShowArray.Count; j++)
        //        {
        //            sceneObjDefine.ShowArray[j].SetActive(false);
        //        }
        //    }*/
        //}

        //GameObject cameraDebugObj = GameObject.Find("DebugCamera");
        //if(cameraDebugObj != null)
        //{
        //    GameObject.DestroyImmediate(cameraDebugObj);
        //    Debug.Log("销毁了场景预设相机");
        //}
    }
  

    public void CreatPrefeb()
    {

        string sceneSize = StateManager._instance.size;
        var prefabDefine = GeScenePrefabDefine(sceneSize);
        if (prefabDefine != null)
        {
            GameObject.Instantiate(prefabDefine.PrefabObject, Vector3.zero, Quaternion.identity);
        }
    }

    public ScenePrefabDefine GeScenePrefabDefine(string sceneSize)
    {
        sceneSize = sceneSize.Trim().ToLower();
        foreach (var scenePrefabDefine in scenePrefabDefines)
        {
            if(scenePrefabDefine.SceneSize.Trim().ToLower().Equals(sceneSize))
            {
                return scenePrefabDefine;
            }
        }
        return null;
    }

    void Update()
    {
        if (InitScene) return;
        UpdateFrame++;
        if (UpdateFrame > 30)
        {
            InitScene = true;
            CreatPrefeb();
        }
    }
}
