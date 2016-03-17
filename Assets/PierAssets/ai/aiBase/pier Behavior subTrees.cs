               using UnityEngine;
using System.Collections;

public class pierBehaviorsubTrees : MonoBehaviour {
    /// <summary>
    /// creates a sub tree that seeks and destroy enemies
    /// will only run one 
    /// use ether a repeat until fail or as a last branch in a tree 
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="attackDelay"></param>
    /// <returns></returns>
	public static aiBehaviorNode attackSequence(baseRtsAI unit,float attackDelay)
    {

        return new Node_Sequence(
            new aiBehaviorNode[]
                {
               // new Node_Find_Closest_Target_BB(unit.blackBoard, "Target", unit.detectionRange,unit.typeToChase),
                new Node_Get_Closest_Enemy(unit.blackBoard, "Target", unit.detectionRange,unit.UnitFaction),
                new Node_Invert(new Node_IsNull(unit.blackBoard, "Target")),
                new Node_Seek_Modular_BB(unit.blackBoard, "Target",
                    (IMoveToNode)(new Node_MoveTo_With_Astar(unit.gameObject, unit.m_seeker, ref unit.m_unit.del,unit.m_unit,unit.SeekarriveRadius))),
                new Node_Attack_Activate_Weapon(unit.MainWeapon,unit.stats),
                new Node_Delay(attackDelay)
                }
        );

    }
  
}
