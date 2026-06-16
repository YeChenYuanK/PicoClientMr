using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForStartState : State
{
    bool IsLoad = false;
    public WaitForStartState(BaseGameEntity gameEntity) : base(gameEntity)
    {
    }

    public override void Enter()
    {
        base.Enter();
        /*
        if(PrepareData.Instance.MapId == 0)
        {
            SceneLoader.Instance.StartScene(SceneDefine.SCENE_GAME_00_ROOM); 
        } else 
        */
        GameProcessData.Instance.IncrGamePlayCount();
        if (PrepareData.Instance.MapId == 0)
        {
            SceneLoader.Instance.StartScene(SceneDefine.SCENE_GAME_01_STORAGE);
        }
        else if (PrepareData.Instance.MapId == 1)
        {
            SceneLoader.Instance.StartScene(SceneDefine.SCENE_GAME_02_FACTORY);
        }
        else if (PrepareData.Instance.MapId == 2)
        {
            SceneLoader.Instance.StartScene(SceneDefine.SCENE_GAME_03_RUINS);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

}
