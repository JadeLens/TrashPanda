using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitOrders : MonoBehaviour
{
	delegate void OrderDoneCallBack();

	public enum OrderType
    {
        move,attackMove,attackTarget,capture
    };

    public static void giveOrder(baseRtsAI unit, OrderType type, Vector3 location)
    {
        aiBehaviorNode commande;
        switch (type)
        {
            case OrderType.attackMove:

                commande = attackMove(unit, location);
                break;
            default:

                commande = moveComand(unit, location);

                break;
        }
        EnqueueComand(unit, commande);
    }

    public static void giveOrder(baseRtsAI unit, OrderType type, IRtsUnit target)
    {
        if (unit.stats != target)
        {
            EnqueueComand(unit, attackTarget(unit, target));
        }
    }

    public static void giveOrder(baseRtsAI unit, OrderType type, PointOfInterest poi)
    {

        EnqueueComand(unit, CapturePoint(unit, poi));
    }

    public static void EnqueueComand(baseRtsAI unit, aiBehaviorNode cmd)
    {
        if (unit != null)
        {
            unit.Orders.Clear();
            unit.Orders.Enqueue(cmd);
        }

    }

    public static void giveOrders(List<baseRtsAI> Selection, OrderType type, Vector3 location)
    {
        foreach (baseRtsAI rabbit in Selection)
        {
            //aiBehaviorNode commande = UnitOrders.moveComand(rabbit,hit.point);
            if(rabbit != null)
                giveOrder(rabbit, type, location);
        }
    }

    public static void giveOrders(List<baseRtsAI> Selection, OrderType type, IRtsUnit target)
    {
        foreach (baseRtsAI rabbit in Selection)
        {
            //aiBehaviorNode commande = UnitOrders.moveComand(rabbit,hit.point);
            if (rabbit != null)
            { 
                giveOrder(rabbit, type, target);
            }
        }
    }

    public static aiBehaviorNode CapturePoint(baseRtsAI rabbit,PointOfInterest poi)
	{
		aiBehaviorNode commande = new Node_Sequence
			(
				new  aiBehaviorNode[] 
				{
					moveComand(rabbit,poi.gameObject.transform.position),
					new Node_Call_Delegate(poi.CapturePT,rabbit)
				}
			);
		return commande;
	}

	static void printDebug()
    {
		Debug.Log("dellegate called");
	}

	public static aiBehaviorNode moveComand(baseRtsAI rabbit,Vector3 loc)
	{
		IMoveToNode commande = new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_unit, rabbit.SeekarriveRadius,loc);
		commande.SetArriveRadius(2.5f);
		return (aiBehaviorNode)commande;
	}

    public static aiBehaviorNode attackMove(baseRtsAI rabbit, Vector3 loc)
    {
        return new Node_PrioritySelector
        (new aiBehaviorNode[]
            {
            new Node_Invert(new Node_Repeat_Until_Fail
                (pierBehaviorsubTrees.attackSequence(rabbit))),
                new Node_MoveTo_With_Astar(rabbit.gameObject,  rabbit.m_unit,rabbit.SeekarriveRadius,loc)
            }
        );
    }

    public static aiBehaviorNode attackTarget(baseRtsAI rabbit, IRtsUnit target)
    {
        return new Node_PrioritySelector
            (new aiBehaviorNode[]
            {//change to seletor nested ina  sequence
                new Node_Invert(new Node_Succeeder(new Node_SetVariable(rabbit.blackBoard,"Target",target.GetGameObject()))),
                new Node_Invert
                (
                    new Node_Repeat_Until_Fail
                    (
                    pierBehaviorsubTrees.killTarget(rabbit,"Target")
                        
                    )
                ),
                new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_unit,rabbit.SeekarriveRadius,target.GetGameObject().transform.position)

            }
        );
    }
}
