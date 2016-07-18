using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum faction { neutral, faction1, faction2 }
public class baseRtsAI : aiBehavior
{
    public Seeker m_seeker;
    public Pier_Unit m_unit;
    //  public unitStats_ForAiTest stats;
    public command MainWeapon;
   
 
    public new IRtsUnit stats;
  
   // private aiBehaviorNode CreateRabbit()
   // {


   //     //  movementNode.SetArriveRadius(SeekarriveRadius);
   //     return new Node_Repeat
   //     (
   //         new Node_PrioritySelector
   //         (
   //             new aiBehaviorNode[]
   //             { 
			////	new Node_Flee(agent,detectionRange,SeekarriveRadius,typeToAvoid),
   //                 new Node_Wander_Modular(
   //                     new Node_MoveTo_With_Astar(this.gameObject,m_unit,SeekarriveRadius),
   //                     anchorRange),

   //             }
   //         )
   //     );
   // }
   // private aiBehaviorNode CreateDrone()
   // {
   //     return new Node_Repeat
   //     (
   //         new Node_PrioritySelector(
   //             new aiBehaviorNode[]
   //             {
   //                 new Node_FollowOrders(this),
   //                 new Node_Delay(0.5f)

   //             }
   //         )
   //     );

   // }


    private aiBehaviorNode CreateAttackDrone()
    {
        return new Node_Repeat//main repeat node
        (
            new Node_Selector(

                new aiBehaviorNode[]
                {
                    new Node_FollowOrders(this),

                     pierBehaviorsubTrees.attackSequence(this)//this gets reapeated by main repeat node if we have no orders
                   
                     
                }
            )
        );

    }

    void Start()
    {

        Init();
   
        routine = CreateAttackDrone();

        if (autoStart)
        {
            routine.Run();
        }
    }

    void OnEnable()
    {
   
        stats = this.gameObject.GetComponent<IRtsUnit>();
        RTSUnitManager.Register(this);
    }

    void OnDisable()
    {
        RTSUnitManager.Unregister(this);

    }

}

