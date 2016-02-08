using UnityEngine;
using System.Collections;

public class ResourceCollisionHandling : MonoBehaviour {

    public PlayerResources myResources; //the player controlling this animal
    public int IncrementationTrash = 0;
    public int IncrementationWater = 0;

    //put this script on the animal to handle the resource collecting when collision
    //also link the PlayerResources object in the game to the owner of the animal.
    void OnTriggerEnter(Collider other)
    {
        if (myResources != null)
        {
            CollectResources Resource = (CollectResources)other.gameObject.GetComponent(typeof(CollectResources));
            IncrementationTrash = Resource.getIncTrash();
            IncrementationWater = Resource.getIncWater();

            if (other.gameObject.CompareTag("Trash"))
            {
                myResources.IncrementTrash(IncrementationTrash);
                if (Resource.getDestroyable() == true)
                    Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Water"))
            {
                myResources.IncrementWater(IncrementationWater);
                if (Resource.getDestroyable())
                    Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Resource"))
            {
                myResources.IncrementTrash(IncrementationTrash);
                myResources.IncrementWater(IncrementationWater);
                if (Resource.getDestroyable())
                    Destroy(other.gameObject);
            }
        }
    }
}
