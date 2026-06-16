using UnityEngine;
using System.Collections;

public class JumpState : State {

	BaseCharacter character;
	private float orgY;
	private const float Gravity = 9.8f;

	private float JumpSpeed = 2.8f;

	private long RiseTime = 2000;
	private float JumpHight = 3.8f;
	private bool DicState = true;

	public JumpState(BaseCharacter character) : base(character)
	{
		this.character = character;
	}

	public override void Enter ()
	{
		base.Enter ();
		orgY = character.transform.position.y;
		//加速度3 
	}

	public override void Update ()
	{
		base.Update ();
		//当前位移速度 = 当前速度 - 每帧减速度
		float currY ;
        //自由落体算法
		/*currY = character.transform.position.y + (JumpSpeed * Time.deltaTime);
		JumpSpeed -= Gravity * Time.deltaTime;
		if (currY < orgY) 
		{
			currY = orgY;
			character.ChangeState (new BattleState(character));
		} 
        */
        if(DicState)
        {
            currY = character.transform.position.y + (JumpHight * Time.deltaTime / (RiseTime / 1000));
            if(currY > JumpHight)
            {
                DicState = false;
                currY = JumpHight;
            }
        }else
        {
            //currY = character.transform.position.y - (JumpHight * Time.deltaTime / ((float)RiseTime / 1000.0f));

            currY = character.transform.position.y - (JumpSpeed * Time.deltaTime);
            if (currY < orgY)
            {
                currY = orgY;
                character.ChangeState(new BattleState(character));
            }
        }

		character.transform.position = new Vector3 (character.transform.position.x , currY , character.transform.position.z);
		//(Time.deltaTime * Gravity)
	}


	public override void Exit()
	{
		base.Exit();
	}

}
 
 