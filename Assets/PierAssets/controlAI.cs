using UnityEngine;
using System.Collections;

public class controlAI : MonoBehaviour 
{
    public aiBehavior[] Selection;
	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        Vector3 pos = transform.position;
		if (Input.GetButton("Fire2"))
		{
			RaycastHit hit;
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				// Debug.Log(hit.point);
		
				UnitOrders.giveOrders(Selection,UnitOrders.OrderType.attackMove,hit.point);
			}
		}
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
				UnitOrders.giveOrders(Selection,UnitOrders.OrderType.move,hit.point);
            }
        }
		if (Input.GetKeyDown(KeyCode.A))
		{
			RaycastHit hit;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				PointOfInterest poi = hit.collider.gameObject.GetComponent<PointOfInterest>();
				if(poi)
				{

					aiBehaviorNode commande = UnitOrders.CapturePoint((baseRtsAI)Selection[0],poi);//new Node_Call_Delegate(poi.toggleMat);
					Selection[0].Orders.Clear();
					Selection[0].Orders.Enqueue(commande);
					//UnitOrders.giveOrders(Selection,UnitOrders.OrderType.capture,hit.point);
				}
				

			}
		}
	}
}


    
