using com.leke;
using LinNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using static DataManager;

public class PrepareLogic : MonoBehaviour {

    private static PrepareLogic instance;
    public static PrepareLogic Instance
    {
        get
        {
            return instance;
        }
    }

   // public Transform birthCenter;
    //public string playerPrefabPath = "Prefabs/Player/CSPlayer";
    //public string playerPrefabPath1 = "Prefabs/Player/CSPlayer1";
    //public string playerPrefabPath2 = "Prefabs/Player/CSPlayer2";
  //  public string watchPlayerPrefabPath = "Prefabs/Player/PreWatchPlayer";
   
  
    public List<BirthPoint> birthPoints=new List<BirthPoint>();
    public GameObject boundsNotice;
    private int updateTick = 0;
    public bool Init;
   
   // public GameObject sceneRig;
#if OCULUS
    public OVROverlay cubemapOverlay;
    public OVROverlay loadingTextQuadOverlay;
#endif
    private void Awake()
    {
        instance = this;
        
    }
    
    public void HideFocusModelShow()
    {
        GameObject leftbeam = GameObject.Find("LeftBeam");
        if(leftbeam != null)
        {
            leftbeam.GetComponent<MeshRenderer>().enabled = false;
        }
        GameObject DominantBeam = GameObject.Find("DominantBeam");
        if (DominantBeam != null)
        {
            DominantBeam.GetComponent<MeshRenderer>().enabled = false;
        }
        GameObject DominantControllerModel = GameObject.Find("DominantControllerModel");
        if (DominantControllerModel != null)
        {
            DominantControllerModel.SetActive(false);
        }
        GameObject NonDominantControllerModel = GameObject.Find("NonDominantControllerModel");
        if (NonDominantControllerModel != null)
        {
            NonDominantControllerModel.SetActive(false);
        }
        
        
    }

    void Start () {

        HideFocusModelShow();
        Debug.Log("进入prepare logic");
        // 重置一下Bleed的特效
        BleedBehavior.BloodAmount = 0.0f;
        if (GameMng.Instance.isServerHost)
            ShowCamp(-1);

        GameMng.Instance.isFire = true;

    }

    private void FixedUpdate()
    {
        //if ((updateTick++) % 20 == 0)
        //{
        //    if (!Init)
        //    {
        //        Init = true;
        //        int camp = -1;
        //        PlayerInfo Player = GameMng.Instance._playerInfoMng.GetMySelfInfo();
        //        if (Player != null)
        //            camp = Player.Camp;
        //        ShowCamp(camp);
        //    }
        //}
            // check glass position
            if (!GameMng.Instance.isServerHost)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null &&
                (mainCamera.transform.position.x < -SceneDefine.SAFE_HOR ||
                    mainCamera.transform.position.x > SceneDefine.SAFE_HOR ||
                    mainCamera.transform.position.z < -SceneDefine.SAFE_VER ||
                    mainCamera.transform.position.z > SceneDefine.SAFE_VER))
            {
                boundsNotice.SetActive(true);
                float distance = SceneDefine.SAFE_HOR;
                if (Mathf.Abs(mainCamera.transform.position.x) > SceneDefine.SAFE_HOR)
                {
                    distance = Mathf.Abs(mainCamera.transform.position.x) - SceneDefine.SAFE_HOR;
                }
                if (Mathf.Abs(mainCamera.transform.position.z) > SceneDefine.SAFE_VER)
                {
                    distance = Mathf.Min(distance, Mathf.Abs(mainCamera.transform.position.z) - SceneDefine.SAFE_VER);
                }
                Color newColor = Color.red;
                newColor.a = Mathf.Lerp(0, 1, Mathf.Clamp(distance / 0.5f, 0, 1));
                boundsNotice.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", newColor);

            }
            else
            {
                boundsNotice.SetActive(false);
            }
        }
        
    }






    public void ShowCamp(int Camp)
    {
      
        for (int i = 0; i < birthPoints.Count; i++)
        {
        
                if (birthPoints[i].Camp == Camp||Camp==-1)
                    birthPoints[i].gameObject.SetActive(true);
                else
                    birthPoints[i].gameObject.SetActive(false);
            
        }
    }

}
