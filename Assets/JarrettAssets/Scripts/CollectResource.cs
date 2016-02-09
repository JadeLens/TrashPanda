using UnityEngine;
using System.Collections;

//use this script on objects to collect resources
public class CollectResource : MonoBehaviour
{
    public bool bDestroyOnCollision = true; //Destroy the resource once collected
   
    public int incWater = 0; //incrementation of Water to player
    public int incTrash = 0; //incrementation of Trash to player
    public int MaxTrash = 0; //Max Trash a Base Provides
    public int MaxWater = 0; //Max Water a Base Provides

    public bool getDestroyable()
    {
        return bDestroyOnCollision;
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
