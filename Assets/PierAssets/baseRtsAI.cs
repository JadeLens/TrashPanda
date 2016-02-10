using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class baseRtsAI : aiBehavior
{
    public Seeker m_seeker;
    public Pier_Unit m_unit;
    public unitStats_ForAiTest stats;
    public command MainWeapon;
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
      private aiBehaviorNode CreateAttackDrone()
    {
        return new Node_Repeat
        (
            new Node_PrioritySelector(
                new aiBehaviorNode[] 
                { 
                    new Node_FollowOrders(this),
                    new Node_Delay(0.5f),
                    new Node_Sequence
                    (
                        new  aiBehaviorNode[] 
                        {
                           
                            new Node_Seek(agent,detectionRange,SeekarriveRadius,AItype.player),
                            //new Node_Align(agent),
                            new Node_AlignToTarget(agent,detectionRange,SeekarriveRadius,AItype.player),
                            new Node_Attack_Activate_Weapon(MainWeapon,stats),
                            new Node_Delay(1f)
                        }
                    )

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



