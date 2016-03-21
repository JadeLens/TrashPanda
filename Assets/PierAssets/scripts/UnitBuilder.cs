using UnityEngine;
using System.Collections;

public class UnitBuilder : MonoBehaviour {
    public faction playerFaction;
   public Transform rabbitPrefab;

    public Transform spawnLoc;

    public Transform rallyPoint;
	// Use this for initialization
	public void SpawnRabbit()
    {

        Transform rabbit =Instantiate(rabbitPrefab, spawnLoc.position, spawnLoc.rotation)as Transform;
        baseRtsAI aiComponent = rabbit.gameObject.GetComponent<baseRtsAI>();

        aiComponent.UnitFaction = playerFaction;
        UnitOrders.giveOrder(aiComponent, UnitOrders.OrderType.move, rallyPoint.position);
    }
}
