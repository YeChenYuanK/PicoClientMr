using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCharacterState : State {
    protected BaseCharacter Player;

    public CSCharacterState(BaseCharacter target) : base(target)
    {
        Player = target;
    }
}
