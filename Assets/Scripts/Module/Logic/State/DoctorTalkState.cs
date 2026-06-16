using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorTalkState : State {

    private float enterTime;

    public DoctorTalkState(BaseGameEntity gameEntity) : base(gameEntity)
    {

    }

    public override void Enter()
    {
        base.Enter();
        // 这里就是女博士对话的实现的地方
        this.enterTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();
        if (this.enterTime == 0) return;
        if(Time.time > (this.enterTime + GameMng.Instance.DoctorTalkTime))
        {
            //GameMng.Instance.ChangeState(typeof(BattleStartState));
        }
    }

}
