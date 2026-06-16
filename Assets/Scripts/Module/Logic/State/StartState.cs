using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartState : State
{

    public StartState(BaseGameEntity gameEntity) : base(gameEntity)
    {

    }

    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene("GameScene_BigSpace_2_Factory");
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
