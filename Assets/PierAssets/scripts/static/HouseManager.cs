using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HouseManager : MonoBehaviour {

    static List<PointOfInterest> list = new List<PointOfInterest>();

    public static void Register(PointOfInterest poi)
    {
        if (!list.Contains(poi))
        {
            list.Add(poi);
        }
    }

    public static void Unregister(PointOfInterest poi)
    {
        if (list.Contains(poi))
        {
            list.Remove(poi);
        }
    }
    public static List<PointOfInterest> GetUnitList()
    {

        return list;
    }
}
