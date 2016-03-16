using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ComputerSideAI : MonoBehaviour {
    public aiBehavior[] Selection;
	public List<aiBehavior> selected;
	public faction myFaction;
	public List<PointOfInterest> points;
	public bool gettingAPoint = false;
    public bool sendOrder = false;
	public Transform Location;
	public Transform centroid;
	public 	List<aiBehavior> OutToCapturePt;
    // Use this for initialization
    void Start () {
		OutToCapturePt = new List<aiBehavior>();
	}

	PointOfInterest getPointToAttack(){
		foreach(PointOfInterest poi in points){
			/*if(poi.owningFaction != myFaction){


				return poi;
			}*/
			if(poi.curentmat != 2){


				return poi;
			}
		}
		return null;
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
		if(gettingAPoint == false){
			PointOfInterest poi = getPointToAttack();
			if(poi != null){
				aiBehaviorNode commande = UnitOrders.CapturePoint((baseRtsAI)Selection[0],poi);//new Node_Call_Delegate(poi.toggleMat);
				Selection[0].Orders.Clear();
				Selection[0].Orders.Enqueue(commande);
				OutToCapturePt.Add(Selection[0]);
				gettingAPoint = true;
			}

		}

		if(OutToCapturePt.Count >0){
			foreach(aiBehavior unit in OutToCapturePt){
				if(unit.Orders.Count == 0){
					gettingAPoint = false;
					OutToCapturePt.Remove(unit);
					break;
				}
			}
		}
	}
}
