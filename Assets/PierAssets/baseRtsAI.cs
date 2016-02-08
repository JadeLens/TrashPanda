using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class baseRtsAI : aiBehavior
{
    public Seeker m_seeker;
    public Pier_Unit m_unit;

    private aiBehaviorNode CreateRabbit()
    {


      //  movementNode.SetArriveRadius(SeekarriveRadius);
        return new Node_Repeat
        (
            new Node_Selector
            (
                new aiBehaviorNode[] 
                { 
                    new Node_Flee(agent,detectionRange,SeekarriveRadius,AItype.wolf),
                    new Node_Wander_Modular( 
                        new Node_MoveTo_With_Astar(this.gameObject, this.m_seeker, ref this.m_unit.del,m_unit), anchorRange),
                 
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

    void Start()
    {

        Init();

        Orders = new Queue<aiBehaviorNode>();
        switch (type)
        {
            case AItype.lamb:
                routine = CreateRabbit();
                break;
        
            default:
                routine = CreateDrone();
                break;

        }
        if (autoStart)
        {
            routine.Run();
        }
    }
}



