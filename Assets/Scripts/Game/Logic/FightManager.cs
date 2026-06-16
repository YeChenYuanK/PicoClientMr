using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.gamestudio.cs;

public class FightManager
{
    private List<FightUnit> fightUnits;

    public FightManager()
    {
        this.fightUnits = new List<FightUnit>();
    }

    public FightUnit AddUnit(FightUnit fu)
    {
        FightUnit existUnit = GetFightUnit(fu.UnitId);
        if (existUnit != null) {
            RemoveUnit(existUnit);
        }
        this.fightUnits.Add(fu);
        return fu;
    }

    public void AddUnit(MFightUnit fu)
    {
        this.AddUnit(FightUnit.FromProto(fu));
    }

    public void RemoveUnit(FightUnit fu)
    {
        if(fu != null)
        {
            this.fightUnits.Remove(fu);
        }
    }

    public FightUnit GetFightUnit(int id)
    {
        foreach(FightUnit fu in this.fightUnits)
        {
            if(fu.UnitId == id)
            {
                return fu;
            }
        }
        return null;
    }

    public FightUnit RemoveUnit(int playerId)
    {
        FightUnit existUnit = this.GetFightUnit(playerId);
        this.RemoveUnit(existUnit);
        return existUnit;
    }

    public List<FightUnit> FightUnits
    {
        get
        {
            return fightUnits;
        }
    }

}

