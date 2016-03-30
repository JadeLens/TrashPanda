using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ComputerSideAI : basePlayer
{
    // public aiBehavior[] Selection;
    //public List<aiBehavior> selected;


    Dictionary<CtrlGroupsName, List<baseRtsAI>> controlGroups;
    //public faction UnitFaction;
   // public List<PointOfInterest> points;
	public bool gettingAPoint = false;
    public bool sendOrder = false;
	public Transform Location;
	public Transform centroid;
	public 	List<aiBehavior> OutToCapturePt;
    public enum CtrlGroupsName {all,capture,offence, defence };
    // Use this for initialization
    void Start ()
    {
		OutToCapturePt = new List<aiBehavior>();
        controlGroups = new Dictionary<CtrlGroupsName, List<baseRtsAI>>();

        controlGroups[CtrlGroupsName.all] = new List<baseRtsAI>();
        controlGroups[CtrlGroupsName.capture] = new List<baseRtsAI>();
        controlGroups[CtrlGroupsName.offence] = new List<baseRtsAI>();
        foreach (IRtsUnit unit in RTSUnitManager.GetUnitList())
        {
                if (unit.getFaction() == UnitFaction && unit.getAIcomponent()!= null)
                {
                    controlGroups[CtrlGroupsName.all].Add(unit.getAIcomponent());
                }  
        }
        //decide ratio of capturer to offence 
        //add defender later
        int total = controlGroups[CtrlGroupsName.all].Count;
        int numCapturer  = Mathf.CeilToInt( total / 6);
        int numOffence = total  - numCapturer;
        for(int i = 0; i < total; i++)
        {
            if (i <= numCapturer)
            {
                controlGroups[CtrlGroupsName.capture].Add(controlGroups[CtrlGroupsName.all][i]);
            }
            else
            {
                controlGroups[CtrlGroupsName.offence].Add(controlGroups[CtrlGroupsName.all][i]);
            }
        }
    }

	PointOfInterest getPointToAttack()
    {
		foreach(PointOfInterest poi in HouseManager.GetUnitList())
        {
			if(poi.owningFaction != UnitFaction)
            {
				return poi;
			}
		}
		return null;
	}
	// Update is called once per frame
	void Update ()
    {
		//	selected = new List<aiBehavior>();
	
		//Vector3 temp  = Vector3.zero;
		//foreach(aiBehavior u in Selection){
  //          if(u!=null)
		//	    temp += u.gameObject.transform.position;

		//}
		//temp /= Selection.Length;
		//float averageDist =0;
		//foreach(aiBehavior u in Selection){
  //          if (u != null)
  //          {
  //              averageDist += Vector3.Distance(u.gameObject.transform.position, temp); 
  //          }

		//}
		//averageDist /= Selection.Length;

  //      foreach (aiBehavior u in Selection) {
  //          if (u != null) { 
  //              if (Vector3.Distance(u.gameObject.transform.position, temp) < averageDist + 2)
  //              {
  //                  selected.Add(u);
  //              }
  //          }
		//}
	/*	if(centroid)
			centroid.position = temp;
		*/
        if (sendOrder)
        {
			//UnitOrders.giveOrders(Selection,UnitOrders.OrderType.move,Location.position);
			foreach (baseRtsAI rabbit in controlGroups[CtrlGroupsName.offence])
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
                int index = getAvailableUnit(controlGroups[CtrlGroupsName.capture]);
                if (controlGroups[CtrlGroupsName.capture][index] != null)
                {
                    aiBehaviorNode commande = UnitOrders.CapturePoint(controlGroups[CtrlGroupsName.capture][index], poi);//new Node_Call_Delegate(poi.toggleMat);
                    controlGroups[CtrlGroupsName.capture][index].Orders.Clear();
                    controlGroups[CtrlGroupsName.capture][index].Orders.Enqueue(commande);
                    OutToCapturePt.Add(controlGroups[CtrlGroupsName.capture][index]); 
                }
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
    int getAvailableUnit(List<baseRtsAI> listOfUnits)
    {
        for(int i = 0; i < listOfUnits.Count; i++)
        {
            if (listOfUnits[i].Orders.Count == 0)
            {

                return i;
            }

        }
        return -1;
    }
}
