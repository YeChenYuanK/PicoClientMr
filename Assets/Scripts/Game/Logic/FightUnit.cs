using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.gamestudio.cs;
using UnityEngine;

/// <summary>
/// 作战单元
/// </summary>
public class FightUnit
{
    private int unitId ;
    private int birthIndex ;
    private int curIndex ;
    private string name ;
    private Vector3 position;
    private float height;
    private int camp;
    private int health;

    private long deadTime;
    private long rebirthTime;

    public MFightUnit ToProto()
    {
        MFightUnit mfu = new MFightUnit();
        mfu.birthIndex = this.birthIndex;
        mfu.unitId = this.unitId;
        mfu.curIndex = this.curIndex;
        mfu.name = this.name;
        mfu.height = (int)(this.height * 100);
        mfu.camp = this.camp;
        return mfu;
    }

    public static FightUnit FromProto(MFightUnit mfu)
    {
        FightUnit fu = new FightUnit();
        fu.unitId = mfu.unitId;
        fu.name = mfu.name;
        fu.curIndex = mfu.curIndex;
        fu.birthIndex = mfu.birthIndex;
        fu.height = mfu.height / 100.0f;
        fu.camp = mfu.camp;
        fu.height = mfu.height;
        fu.deadTime = mfu.deadTime;
        fu.rebirthTime = mfu.rebirthTime;
        return fu;
    }

    public int UnitId
    {
        get
        {
            return unitId;
        }

        set
        {
            unitId = value;
        }
    }

    public int BirthIndex
    {
        get
        {
            return birthIndex;
        }

        set
        {
            birthIndex = value;
        }
    }

    public int CurIndex
    {
        get
        {
            return curIndex;
        }

        set
        {
            curIndex = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public float Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public int Camp
    {
        get
        {
            return camp;
        }

        set
        {
            camp = value;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public long DeadTime
    {
        get
        {
            return deadTime;
        }

        set
        {
            deadTime = value;
        }
    }

    public long RebirthTime
    {
        get
        {
            return rebirthTime;
        }

        set
        {
            rebirthTime = value;
        }
    }
}
