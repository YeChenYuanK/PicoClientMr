using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour {
    private static GameSceneManager instance;

    public static GameSceneManager Instance
    {
        get
        {
            return instance;
        }
    }
    public Transform hidePart;
    public GameObject UIEndCount;
    // Use this for initialization
    void Awake () {
        instance = this;
	}

    private HideWhenDie[] cachedHideObjs;

    // Update is called once per frame
    void Update () {
		
	}
    public bool GameEnd
    {
        set
        {
            if (UIEndCount != null)
            {
                ObjectUtil.UpdateObjectActive(UIEndCount, value);
                if (Game.GameLogic.Instance != null&&Game.GameLogic.Instance.HideRoomObj!=null)
                {
                    Game.GameLogic.Instance.HideRoomObj.SetActive(false);
                }
            }
            this.SetHidePart(!value);
        }
    }
    public bool PartActive
    {
        set
        {
            if (hidePart != null)
            {
                ObjectUtil.UpdateObjectActive(hidePart, value);
            }
            this.SetHidePart(value);
        }
    }
    
    public void SetHidePart(bool isactive)
    {
        if(!isactive)
        {
            HideWhenDie[] objs = FindObjectsOfType<HideWhenDie>();
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].gameObject.SetActive(false);
            }
            cachedHideObjs = objs;
        } else
        {
            if(cachedHideObjs != null)
            {
                for (int i = 0; i < cachedHideObjs.Length; i++)
                {
                    cachedHideObjs[i].gameObject.SetActive(true);
                }
            }
        }
    }

}
