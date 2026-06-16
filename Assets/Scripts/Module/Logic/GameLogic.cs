using com.leke;
using com.leke.redSea;

using LinNet;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static DataManager;

namespace Game
{
    public class GameLogic : MonoBehaviour
    {
        private static GameLogic instance;

        public static GameLogic Instance
        {
            get
            {
                return instance;
            }
        }
      

        private bool Init = false;
        public string playerPrefabPath = "Prefabs/Player/CSPlayer";
        public string playerPrefabPath1 = "Prefabs/Player/CSPlayer";
        public string playerPrefabPath2 = "Prefabs/Player/CSPlayer";
        public string watchPlayerPrefabPath = "Prefabs/Player/WatchPlayer";
        public List<Transform> spwanPosList;
        public Transform birthCenter;
        public GameObject preloadCamera;
        private int updateTick = 0;
        public GameObject boundsNotice;

        public UnityEngine.Playables.PlayableDirector director;
        public double directorPlayTime;
        public GameObject HideRoomObj;
        public GameObject MapInfoObj;
        public Volume deadVolume;

        public static double ServerStartTime = -1;
        private bool isInit;

        private long lastFixedTime;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            GameMng.Instance.isFire = true;
            if (deadVolume != null) deadVolume.enabled = false;
            MapInfoObj.SetActive(false);
        }

        public void ShowMapInfoObj()
        {
            MapInfoObj.SetActive(true);
            if (deadVolume != null) deadVolume.enabled = true;
            GameObject notpostCameraObj = GameObject.Find("Camera-nopost");
            if (notpostCameraObj != null)
            {
                notpostCameraObj.GetComponent<Camera>().enabled = true;
            }
        }

        public void HideMapInfoObj()
        {
            MapInfoObj.SetActive(false);
            if (deadVolume != null) deadVolume.enabled = false;
            GameObject notpostCameraObj = GameObject.Find("Camera-nopost");
            if (notpostCameraObj != null)
            {
                notpostCameraObj.GetComponent<Camera>().enabled = false;
            }
        }



        void Update()
        {

        }

        public bool IsAllSceneReady()
        {
            for (int i = 0; GameMng.Instance._playerInfoMng.PlayersCount > i; i++)
            {
                CSPlayer player = GameMng.Instance._playerInfoMng.GetPlayerById(i);
                if (player.PlayerState==0)
                {
                    return false;
                }
            }
            return true;
        }

        private void FixedUpdate()
        {
            if ((updateTick++) % 20 == 0)
            {
              
                    if (!Init)
                    {
                    Init = true;
                    int camp = -1;
                    if (!GameMng.Instance.isServerHost)
                    {
                        camp = GameMng.Instance._playerInfoMng.MySelfCamp;
                    }
                   
                    BattleContext.Instance.CurBattleField.ShowCamp(camp);


                }
               
            }

            // check glass position
            // 部分场景未接 boundsNotice(Inspector 拖的越界警告物体),为 null 时跳过,避免 FixedUpdate 每帧抛 NPE。
            if (boundsNotice == null) return;
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
            else
            {
                boundsNotice.SetActive(false);
            }

            //long curtime = DateUtil.NowMllSec;
            //// float curtime = Time.time;
            //if (this.director != null && this.director.isActiveAndEnabled && this.director.time > 0 && this.directorPlayTime > 0)
            //{
            //    if (curtime - lastFixedTime > 1000)
            //    {
            //        this.director.time = NetworkTime.time - this.directorPlayTime;
            //        Debug.Log("暂停之后继续播放：" + this.director.time);
            //    }
            //}

            //lastFixedTime = curtime;
        }
      
        public GameObject Spawn()
        {
            //spawned = true;
            //int index = 0;
            //if (InitManager.Instance.IsControlCenter)
            //{
            //    index = PrepareData.Instance.SelfAllocateIndex;
            //}
            //else
            //{
            //    // index = (int)PhotonNetwork.player.AllProperties["SpawnPosition"];
            //}

            //int camp = 0;
            //if (InitManager.Instance.IsControlCenter)
            //{
            //    camp = PrepareData.Instance.Camp;
            //}
            //else
            //{
            //    camp = (int)GameMng.Instance._playerInfoMng.GetCamp();
            //}
            GameObject temp=null;
            //if (camp == (int)PlayerCamp.Red)
            //{
            //    temp = NetworkManager.Instantiate(playerPrefabPath, birthCenter.position, birthCenter.rotation, 0) as GameObject;
            //}
            //else if (camp == (int)(PunTeams.Team.blue))
            //{
            //    temp = PhotonNetwork.Instantiate(playerPrefabPath, birthCenter.position, birthCenter.rotation, 0) as GameObject;
            //}
            //else
            //{
            //    temp = GameObject.Instantiate<GameObject>(Resources.Load(watchPlayerPrefabPath) as GameObject);//, birthCenter.position, birthCenter.rotation);
            //    this.DestoryPreLoadCamera();
            //    return null;
            //    //temp = g.Instantiate(watchPlayerPrefabPath, birthCenter.position, birthCenter.rotation, 0) as GameObject;
            //}
            //CSPlayer player = temp.GetComponent<CSPlayer>();
            //CSPlayerInfo playerInfo = temp.GetComponent<CSPlayerInfo>();
            ////player.playerId = PrepareData.Instance.SelfAllocateIndex;//PhotonNetwork.player.ID;
            ////player.Camp = camp;
            ////player.AllocateIndex = PrepareData.Instance.SelfAllocateIndex;
            ////player.PlayerName = PrepareData.Instance.PlayerName;
            ////player.weaponId = PrepareData.Instance.WeaponId;
            ////player.gunHandType = PrepareData.Instance.GunHandType;


            //player.selfCharacter.unitId = player.playerId;
            //if (InitManager.Instance.IsControlCenter)
            //{
            //    EnterGame enterGame = new EnterGame();
            //    enterGame.index = index;

            //}

            //// 隐藏别的阵营的出生点
            
            
            //// 玩家切换至准备状态，需要提示玩家走向出生区域
            //player.ChangeState(typeof(CharacterPrepareState));
            //this.DestoryPreLoadCamera();

            //if (Env.Instance != null)
            //{
            //    Env.Instance.PlayGameStart();
            //}

            return temp;
        }

        public void Record(int playerId, RecordType recordType)
        {
            CSPlayer player = CSPlayerManager.Instance.GetCSPlayer(playerId);
            if (player == null)
            {
                // GameMng.Instance.gameMngPV.RPC("RecordRpc", PhotonTargets.All, playerId, (int)recordType, "");
            }
            else
            {
                //  GameMng.Instance.gameMngPV.RPC("RecordRpc", PhotonTargets.All, playerId, (int)recordType, player.PlayerInfo.PlayerName);
            }

        }

        public void DestoryPreLoadCamera()
        {
            if (this.preloadCamera != null)
            {
                this.preloadCamera.SetActive(false);
                GameObject.Destroy(this.preloadCamera);
                this.preloadCamera = null;
            }
        }

    }

}

