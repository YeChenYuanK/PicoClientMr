using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareState : State
{
    public PrepareState(BaseGameEntity gameEntity) : base(gameEntity)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // SceneManager.LoadScene("PrepareScene");
        SceneLoader.Instance.StartScene(SceneDefine.SCENE_PREPARE);
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
