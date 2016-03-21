using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RTSUnitManager {
    static List<baseRtsAI> list = new List<baseRtsAI>();
 
    public static void Register(baseRtsAI unit)
    {
        if (!list.Contains(unit))
        {
            list.Add(unit);
        }
    }

    public static void Unregister(baseRtsAI unit)
    {
        if (list.Contains(unit))
        {
            list.Remove(unit);
        }
    }
    public static  List<baseRtsAI> GetUnitList()
    {

        return list;
    }
}
