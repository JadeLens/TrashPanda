using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ComputerSideAI : basePlayer
{
    // public aiBehavior[] Selection;
    //public List<aiBehavior> selected;

    //behaviortree Related start

    public Dictionary<string, Object> blackBoard;
    public bool autoStart = false;
    public aiBehaviorNode routine;

    //behaviortree Related end

    Dictionary<CtrlGroupsName, List<baseRtsAI>> controlGroups;
    //public faction UnitFaction;
   // public List<PointOfInterest> points;
	public bool gettingAPoint = false;
  
	public Transform Location;
	public Transform centroid;
	public 	List<aiBehavior> OutToCapturePt;
    public enum CtrlGroupsName {all,capture,offence, defence };
    public float actionDelay = 30;
    // Use this for initialization
    void Start ()
    {
		OutToCapturePt = new List<aiBehavior>();
        controlGroups = new Dictionary<CtrlGroupsName, List<baseRtsAI>>();
        blackBoard = new Dictionary<string, Object>();

        controlGroups[CtrlGroupsName.all] = new List<baseRtsAI>();
        controlGroups[CtrlGroupsName.capture] = new List<baseRtsAI>();
        controlGroups[CtrlGroupsName.offence] = new List<baseRtsAI>();

        //decide ratio of capturer to offence 
        //add defender later

        routine = createComputerAI();
        if (autoStart)
        {
            routine.Run();
        }
    }
    public void FixedUpdate()
    {
        if (routine.isRunning())
        {
            routine.Act(this.gameObject);
        }
    }
    public aiBehaviorNode createComputerAI()
    {

        return new Node_Repeat
        (
            new Node_Sequence
            (
                new aiBehaviorNode[]
                {
                 new Node_Call_Delegate(countUnits)///also sorts them into groups
                ,new Node_Call_Delegate(sendToCapPoint)
                ,new Node_Call_Delegate(isCapingDone),
                 new Node_Call_Delegate(sendOrder),
                new Node_Delay(actionDelay)

                }

            )
         );

    }
    class UnitAmount : Object
    {
        public int total;
        public int numCapturer;
        public int numOffence;
    }

    public void countUnits()
    {
        controlGroups[CtrlGroupsName.all].Clear();
        controlGroups[CtrlGroupsName.capture].Clear();
        controlGroups[CtrlGroupsName.offence].Clear();
        foreach (IRtsUnit unit in RTSUnitManager.GetUnitList())
        {
            if (unit.getFaction() == UnitFaction && unit.getAIcomponent() != null)
            {
                controlGroups[CtrlGroupsName.all].Add(unit.getAIcomponent());
            }
        }
        UnitAmount unitAmount = new UnitAmount();
        unitAmount.total = controlGroups[CtrlGroupsName.all].Count;
        unitAmount.numCapturer = Mathf.CeilToInt(unitAmount.total / 3);
        unitAmount.numOffence = unitAmount.total - unitAmount.numCapturer *2;
        for (int i = 0; i < unitAmount.total; i++)
        {
            if (i <= unitAmount.numCapturer)
            {
                controlGroups[CtrlGroupsName.capture].Add(controlGroups[CtrlGroupsName.all][i]);
            }
            else
            {
                controlGroups[CtrlGroupsName.offence].Add(controlGroups[CtrlGroupsName.all][i]);
            }
        }
        Node_SetVariable.SetBBVar(blackBoard, "unitCount",unitAmount);

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
    

    void sendToCapPoint()
    {
        if (gettingAPoint == false)
        {
            PointOfInterest poi = getPointToAttack();
            if (poi != null)
            {
                int index = 0;// getAvailableUnit(controlGroups[CtrlGroupsName.capture]);
                UnitOrders.giveOrders(controlGroups[CtrlGroupsName.capture], UnitOrders.OrderType.move, poi.gameObject.transform.position);
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
         
       


    }

    void isCapingDone()
    {
        if (OutToCapturePt.Count > 0)
        {
            foreach (aiBehavior unit in OutToCapturePt)
            {
                if (unit.Orders.Count == 0 ||unit == null)
                {

                    gettingAPoint = false;
                    OutToCapturePt.Remove(unit);

                    break;

                }

            }
            //if(OutToCapturePt.Count == 0)
            //{
            //    gettingAPoint = false;
            //}
        }

    }
    void sendOrder()
    {
            //UnitOrders.giveOrders(Selection,UnitOrders.OrderType.move,Location.position);
            foreach (baseRtsAI rabbit in controlGroups[CtrlGroupsName.offence])
            {

            aiBehaviorNode commande = UnitOrders.attackMove(rabbit, Location.position);///UnitOrders.moveComand(rabbit, Location.position);
            rabbit.Orders.Clear();
                rabbit.Orders.Enqueue(commande);
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
