using UnityEngine;
using System.Collections;

//use this script on objects to collect resources
public class CollectResources : MonoBehaviour
{
    public bool DestroyOnCollision = true;
    public int incWater = 0;
    public int incTrash = 0;

    public bool getDestroyable()
    {
        return DestroyOnCollision;
    }

    public int getIncWater()
    {
        return incWater;
    }
    public int getIncTrash()
    {
        return incTrash;
    }
}
