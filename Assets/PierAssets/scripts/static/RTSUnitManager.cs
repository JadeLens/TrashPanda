using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct OnAttackedInfo
{
 public   Vector3 location;

}
public interface IRtsUnit: IGameUnit, IObsevable<OnAttackedInfo>
{
    faction getFaction();
    void setFaction(faction newFaction);

    float getSightRange();
    float getAttackRange();
    float getAttackSpeed();
    baseRtsAI getAIcomponent();
}

public class RTSUnitManager {
    static List<IRtsUnit> list = new List<IRtsUnit>();
 
    public static void Register(baseRtsAI unit)
    {
        if (!list.Contains(unit.stats))
        {
           // Debug.Log(unit.gameObject.name);
           list.Add(unit.stats);
        }
    }
    public static void Unregister(baseRtsAI unit)
    {
        if (list.Contains(unit.stats))
        {
            list.Remove(unit.stats);
        }
    }

    public static void Register(IRtsUnit unit)
    {
        if (!list.Contains(unit))
        {
            list.Add(unit);
            
        }
    }
    public static void Unregister(IRtsUnit unit)
    {
        if (list.Contains(unit))
        {
            list.Remove(unit);
        }
    }

    public static  List<IRtsUnit> GetUnitList()
    {

        return list;
    }
}
