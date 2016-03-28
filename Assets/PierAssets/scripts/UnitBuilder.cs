using UnityEngine;
using System.Collections;

public class UnitBuilder : MonoBehaviour {
    public basePlayer owner;
    public bool Mute = false;
   public Transform rabbitPrefab;

    public Transform spawnLoc;

    public Transform rallyPoint;

    public int rabbitTrashCost = 50;
    public int rabbitWaterCost = 50;
    public AudioClip spawnSound = null;
    public AudioClip wrong;
    // Use this for initialization
    public void SpawnRabbit()
    {

        if (owner.myResources.getTrash()>= rabbitTrashCost && owner.myResources.getWater() >= rabbitWaterCost) {
            if(!Mute)
                AudioManager.PlaySoundClip(spawnSound);
            owner.myResources.IncrementTrash(-rabbitTrashCost);
            owner.myResources.IncrementWater(-rabbitWaterCost);
            Transform rabbit = Instantiate(rabbitPrefab, spawnLoc.position, spawnLoc.rotation) as Transform;
            baseRtsAI aiComponent = rabbit.gameObject.GetComponent<baseRtsAI>();

            aiComponent.UnitFaction = owner.UnitFaction;
            UnitOrders.giveOrder(aiComponent, UnitOrders.OrderType.move, rallyPoint.position);
        }
        else
        {
            if (!Mute)
                AudioManager.PlaySoundClip(wrong);
        }
    }
}
