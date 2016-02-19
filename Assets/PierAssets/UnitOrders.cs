﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnitOrders : MonoBehaviour {
	public enum OrderType  {move,attackMove,capture};
	public static void giveOrders(aiBehavior[] Selection,OrderType type,Vector3 location){

		foreach (baseRtsAI rabbit in Selection)
		{
			//aiBehaviorNode commande = UnitOrders.moveComand(rabbit,hit.point);
			aiBehaviorNode commande;
			switch(type){
			case OrderType.move:

				 commande = UnitOrders.moveComand(rabbit,location);
				break;
			case OrderType.capture:

				commande = UnitOrders.CapturePoint(rabbit);
				break;
			case OrderType.attackMove:

				 commande = UnitOrders.attackMove(rabbit,location);
				break;
				default:
				
				commande = UnitOrders.moveComand(rabbit,location);

				break;
			}
			rabbit.Orders.Clear();
			rabbit.Orders.Enqueue(commande);
		}
	}

	public static aiBehaviorNode CapturePoint(baseRtsAI rabbit)
	{
		aiBehaviorNode commande = new Node_Call_Delegate(printDebug);
		return commande;
	}
	 static void printDebug(){
		Debug.Log("dellegate called");
	}
	public static aiBehaviorNode moveComand(baseRtsAI rabbit,Vector3 loc)
	{
		IMoveToNode commande = new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_seeker, ref rabbit.m_unit.del, rabbit.m_unit,loc);

		commande.SetArriveRadius(2.5f);
		return (aiBehaviorNode)commande;
	}

	public static aiBehaviorNode attackMove(baseRtsAI rabbit,Vector3 loc)
	{	
		return new Node_Selector
			(
				new  aiBehaviorNode[] 
				{
					new Node_Sequence
					(
						new  aiBehaviorNode[] 
						{

							new Node_Seek_Modular
							(
								(IMoveToNode)(new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_seeker, ref rabbit.m_unit.del,rabbit.m_unit)),
								rabbit.detectionRange,rabbit.SeekarriveRadius,rabbit.typeToChase
							),
							new Node_Attack_Activate_Weapon(rabbit.MainWeapon,rabbit.stats),
							new Node_Delay(1f)
						}
					),
					new Node_MoveTo_With_Astar(rabbit.gameObject, rabbit.m_seeker, ref rabbit.m_unit.del, rabbit.m_unit,loc)

				}

			);
	}
}
