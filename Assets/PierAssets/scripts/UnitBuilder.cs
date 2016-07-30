using UnityEngine;
using System.Collections;

public class UnitBuilder : MonoBehaviour {
    public basePlayer owner;
    public bool Mute = false;
   public Transform rabbitPrefab;
    public Transform RacoonPrefab;
    public Transform spawnLoc;

    public Transform rallyPoint;

    public int rabbitTrashCost = 50;
    public int rabbitWaterCost = 50;

    public int racoonTrashCost = 50;
    public int racoonWaterCost = 50;
    public AudioClip spawnSound = null;
    public AudioClip wrong;
    // Use this for initialization

        //later change to use unit interface
    public static void cloneUnit(baseRtsAI unit)
    {
        Transform rabbit = Instantiate(unit.transform, unit.transform.position, unit.transform.rotation) as Transform;
        baseRtsAI aiComponent = rabbit.gameObject.GetComponent<baseRtsAI>();

        aiComponent.stats.setFaction(unit.stats.getFaction());
        UnitOrders.giveOrder(aiComponent, UnitOrders.OrderType.move, unit.transform.position);


    }
    /// <summary>
    /// because Ui button cant use a return type of bool 
    /// </summary>
    public void spawnForUiButton()
    {
        SpawnRabbit();
    }
    public bool SpawnRabbit()
    {

        if (owner.myResources.getTrash()>= rabbitTrashCost && owner.myResources.getWater() >= rabbitWaterCost) {
            if(!Mute)
                AudioManager.PlaySoundClip(spawnSound);
            owner.myResources.IncrementTrash(-rabbitTrashCost);
            owner.myResources.IncrementWater(-rabbitWaterCost);
            Transform rabbit = Instantiate(rabbitPrefab, spawnLoc.position, spawnLoc.rotation) as Transform;
            baseRtsAI aiComponent = rabbit.gameObject.GetComponent<baseRtsAI>();

            aiComponent.stats.setFaction(owner.UnitFaction);
            UnitOrders.giveOrder(aiComponent, UnitOrders.OrderType.move, rallyPoint.position);
            return true;
        }
        else
        {
            if (!Mute)
                AudioManager.PlaySoundClip(wrong);
            return false;
        }
    }
    public void spawnRacoonForUiButton()
    {
        SpawnRacoon();
    }
    public bool SpawnRacoon()
    {

        if (owner.myResources.getTrash() >= racoonTrashCost && owner.myResources.getWater() >= racoonWaterCost)
        {
            if (!Mute)
                AudioManager.PlaySoundClip(spawnSound);
            owner.myResources.IncrementTrash(-racoonTrashCost);
            owner.myResources.IncrementWater(-racoonWaterCost);
            Transform rabbit = Instantiate(RacoonPrefab, spawnLoc.position, spawnLoc.rotation) as Transform;
            baseRtsAI aiComponent = rabbit.gameObject.GetComponent<baseRtsAI>();

            aiComponent.stats.setFaction(owner.UnitFaction);
            UnitOrders.giveOrder(aiComponent, UnitOrders.OrderType.move, rallyPoint.position);
            return true;
        }
        else
        {
            if (!Mute)
                AudioManager.PlaySoundClip(wrong);
            return false;
        }
    }
}
