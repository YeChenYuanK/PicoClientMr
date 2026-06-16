using UnityEngine;
using System.Collections;

public class DieState : State {

	BaseCharacter character;

	public DieState(BaseCharacter character) : base(character)
	{
		this.character = character;
	}

	public override void Enter ()
	{
		base.Enter ();
        if(character is SelfCharacter)
        {
            // 如果是主角视角
            SelfCharacter selfCharacter = character as SelfCharacter;
            selfCharacter.ShowDeadCount(ClacLeftSec());
        }

	}

	public override void Update ()
	{
		base.Update ();
        if (character is SelfCharacter)
        {
            // 如果是主角视角
            SelfCharacter selfCharacter = character as SelfCharacter;
            selfCharacter.ShowDeadCount(ClacLeftSec());
        }
    }

    public int ClacLeftSec()
    {
        FightUnit fu = ClientLogic.Instance.FightManager.GetFightUnit(this.character.unitId);
        return (int)((fu.RebirthTime - DateUtil.ServerTime) / 1000);
    }

    public override void Exit()
    {
        base.Exit();
      
    }

}
