using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ComputerSideAI : MonoBehaviour {
    public aiBehavior[] Selection;
	public List<aiBehavior> selected;
    public bool sendOrder = false;
	public Transform Location;
	public Transform centroid;
    // Use this for initialization
    void Start () {
		
	}
	// Update is called once per frame
	void Update () {
			selected = new List<aiBehavior>();
		
		Vector3 temp  = Vector3.zero;
		foreach(aiBehavior u in Selection){
			temp += u.gameObject.transform.position;

		}
		temp /= Selection.Length;
		float averageDist =0;
		foreach(aiBehavior u in Selection){
			averageDist += Vector3.Distance( u.gameObject.transform.position,temp);

		}
		averageDist /= Selection.Length;

		foreach(aiBehavior u in Selection){
			if(Vector3.Distance( u.gameObject.transform.position,temp) < averageDist +2){
				selected.Add(u);
			}

		}
		if(centroid)
			centroid.position = temp;
		
        if (sendOrder)
        {
			//UnitOrders.giveOrders(Selection,UnitOrders.OrderType.move,Location.position);

			foreach (baseRtsAI rabbit in selected)
			{				

				aiBehaviorNode commande = UnitOrders.moveComand(rabbit,Location.position);
					

				rabbit.Orders.Clear();
				rabbit.Orders.Enqueue(commande);
			}
			sendOrder = false;

        }
	}
}
