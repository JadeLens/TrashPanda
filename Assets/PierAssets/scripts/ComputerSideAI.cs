using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ComputerSideAI : basePlayer
{
    // public aiBehavior[] Selection;
    //public List<aiBehavior> selected;

    //behaviortree Related start

    public Dictionary<string, System.Object> blackBoard;
    public bool autoStart = false;
    public aiBehaviorNode routine;

    //behaviortree Related end

    public UnitBuilder myBuilding;
    Dictionary<CtrlGroupsName, List<baseRtsAI>> controlGroups;
    //public faction UnitFaction;
   // public List<PointOfInterest> points;
	public bool gettingAPoint = false;
  
	public Transform Location;
//	public Transform centroid;
	public 	List<aiBehavior> OutToCapturePt;
    public enum CtrlGroupsName {all,capture,offence, defence };
    public float actionDelay = 30;

    int total;
    int numCapturer;
    int numOffence;

    // Use this for initialization
    void Start ()
    {
		OutToCapturePt = new List<aiBehavior>();
        controlGroups = new Dictionary<CtrlGroupsName, List<baseRtsAI>>();
        blackBoard = new Dictionary<string, System.Object>();

        controlGroups[CtrlGroupsName.all] = new List<baseRtsAI>();
        controlGroups[CtrlGroupsName.capture] = new List<baseRtsAI>();
        controlGroups[CtrlGroupsName.offence] = new List<baseRtsAI>();

        //decide ratio of capturer to offence 
        //add defender later

        routine = createAdvancedComputerAI();
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
                 new Node_Call_Delegate(countUnits),///also sorts them into groups
                 new Node_Call_Delegate(sendToCapPoint),
                 new Node_Check_Condition(isCapingDone),
                 new Node_Call_Delegate(sendAttackOrder),//attack order
                 new Node_Delay(actionDelay)

                }

            )
         );

    }
    public aiBehaviorNode createAdvancedComputerAI()
    {
        return new Node_Repeat
        (
            new Node_Sequence
            (
                new aiBehaviorNode[]
                {
                new Node_Call_Delegate(countUnits),///also sorts them into groups
                new Node_PrioritySelector
                (
                    new aiBehaviorNode[]
                    {
                        new Node_Sequence //attack sequence
                        (
                            new aiBehaviorNode[]
                            {
                            new Node_Check_Condition(haveEnoughUnitsToAttack),
                            new Node_Call_Delegate(sendAttackOrder)
                            }
                        ),
                        new Node_Check_Condition(makeUnits), //returns false if not enought resources
                        new Node_Sequence //capPoint sequence
                        (
                            new aiBehaviorNode[]
                            {
                                new Node_Check_Condition(isCapingDone),//add check to see if any unCaptured points
                                new Node_Call_Delegate(sendToCapPoint)
                                
                            }
                        )
                    }
                 ),
               
                 new Node_Delay(actionDelay)

                }

            )
         );

    }

    bool makeUnits()
    {


        return myBuilding.SpawnRabbit();
    }
    bool haveEnoughUnitsToAttack()
    {

        return false;
    }
    bool haveEnoughUnits()
    {

        return false;
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
      
        total = controlGroups[CtrlGroupsName.all].Count;
        numCapturer = Mathf.CeilToInt(total / 3);
        numOffence = total - numCapturer *2;
        for (int i = 0; i < total; i++)
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
       // blackBoard.Add("unitCount", total);
       Node_SetVariable.SetBBVar(blackBoard, "unitCount", total);

    }
    //to add prioritise closer cap pt and neutral ones
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

    bool isCapingDone()
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
        else
        {
            return true;
         //   Node_SetVariable.SetBBVar(blackBoard, "isCapDone", true);
        }
        return false;
    }
    void sendAttackOrder()
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
