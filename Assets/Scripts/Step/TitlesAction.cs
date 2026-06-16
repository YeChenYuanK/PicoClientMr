using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlesAction : StepAction
{
    
    public override double PassTime
    {
        get
        {
            return Time.fixedTime - serverStartTime;
        }
    }
    public override void InitStartTime()
    {
        serverStartTime = Time.fixedTime;
    }
}
