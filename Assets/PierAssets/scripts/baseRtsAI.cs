﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum faction { neutral, faction1, faction2 }
public class baseRtsAI : aiBehavior
{
    public faction UnitFaction;
    public Seeker m_seeker;
    public Pier_Unit m_unit;
    //  public unitStats_ForAiTest stats;
    public command MainWeapon;
    public AItype typeToChase;
    public AItype typeToAvoid;
    private aiBehaviorNode CreateRabbit()
    {


        //  movementNode.SetArriveRadius(SeekarriveRadius);
        return new Node_Repeat
        (
            new Node_PrioritySelector
            (
                new aiBehaviorNode[]
                { 
			//	new Node_Flee(agent,detectionRange,SeekarriveRadius,typeToAvoid),
                    new Node_Wander_Modular(
                        new Node_MoveTo_With_Astar(this.gameObject, this.m_seeker, ref this.m_unit.del,m_unit,SeekarriveRadius),
                        anchorRange),

                }
            )
        );
    }
    private aiBehaviorNode CreateDrone()
    {
        return new Node_Repeat
        (
            new Node_PrioritySelector(
                new aiBehaviorNode[]
                {
                    new Node_FollowOrders(this),
                    new Node_Delay(0.5f)

                }
            )
        );

    }
    private aiBehaviorNode CreateAttackDrone()
    {
        return new Node_Repeat//main repeat node
        (
            new Node_PrioritySelector(

                new aiBehaviorNode[]
                {
                    new Node_FollowOrders(this),

                      pierBehaviorsubTrees.attackSequence(this,0.5f)//this gets reapeated by main repeat node if we have no orders
                   

                }
            )
        );

    }

    void Start()
    {

        Init();

        Orders = new Queue<aiBehaviorNode>();
        switch (type)
        {
            case AItype.lamb:
                routine = CreateRabbit();
                break;
            case AItype.wolf:
                routine = CreateAttackDrone();
                //    Debug.Log("attack Drone");

                break;
            default:
                routine = CreateDrone();
                //      Debug.Log(" Drone");
                break;

        }
        if (autoStart)
        {
            routine.Run();
        }
    }


    void OnEnable()
    {
        RTSUnitManager.Register(this);
    }

    void OnDisable()
    {
        RTSUnitManager.Unregister(this);

    }

}

