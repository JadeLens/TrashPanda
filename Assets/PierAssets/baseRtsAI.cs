using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class baseRtsAI : aiBehavior
{
    public Seeker m_seeker;
    public Pier_Unit m_unit;
    public unitStats_ForAiTest stats;
    public command MainWeapon;
	public AItype typeToChase;
	public AItype typeToAvoid;
    private aiBehaviorNode CreateRabbit()
    {
        

      //  movementNode.SetArriveRadius(SeekarriveRadius);
        return new Node_Repeat
        (
            new Node_Selector
            (
                new aiBehaviorNode[] 
                { 
			//	new Node_Flee(agent,detectionRange,SeekarriveRadius,typeToAvoid),
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
                  
                    new Node_Sequence
                    (
                        new  aiBehaviorNode[] 
                        {
                           
                            new Node_Seek_Modular
                            (
                                (IMoveToNode)(new Node_MoveTo_With_Astar(this.gameObject, this.m_seeker, ref this.m_unit.del,m_unit)),
								detectionRange,SeekarriveRadius,typeToChase
                            ),
                            //new Node_Align(agent),
                           // new Node_AlignToTarget(agent,detectionRange,SeekarriveRadius,AItype.player),
                            new Node_Attack_Activate_Weapon(MainWeapon,stats),
                            new Node_Delay(1f)
                        }
                    ),
                      new Node_Delay(0.1f)

                }
            )
        );

    }
 
    void Start()
    {

      //  Init();

        Orders = new Queue<aiBehaviorNode>();
        switch (type)
        {
            case AItype.lamb:
                routine = CreateRabbit();
                break;
            case AItype.wolf:
                routine = CreateAttackDrone();
                Debug.Log("attack Drone");

                break;
            default:
                routine = CreateDrone();
                Debug.Log(" Drone");
                break;

        }
        if (autoStart)
        {
            routine.Run();
        }
    }
}



