               using UnityEngine;
using System.Collections;

public class pierBehaviorsubTrees : MonoBehaviour {
    /// <summary>
    /// creates a sub tree that seeks and destroy enemies
    /// will only run one 
    /// use ether a repeat until fail or as a last branch in a tree 
    /// </summary>
    /// <param name="unit"></param>
    /// 
    /// <returns></returns>
	public static aiBehaviorNode attackSequence(baseRtsAI unit)
    {

        return new Node_Sequence(
            new aiBehaviorNode[]
                {
               // new Node_Find_Closest_Target_BB(unit.blackBoard, "Target", unit.detectionRange,unit.typeToChase),
                new Node_Get_Closest_Enemy(unit.blackBoard, "Target", unit.stats.getSightRange(),unit.stats.getFaction()),
                new Node_Invert(new Node_IsNull(unit.blackBoard, "Target")),
                killTarget(unit, "Target")
                }
        );

    }
    public static aiBehaviorNode killTarget(baseRtsAI unit,string TargetKey)
    {
    return new Node_Sequence
            (
                new aiBehaviorNode[]
                {
                  //  new Node_Timer(
                        new Node_Seek_Modular_BB(unit.blackBoard, TargetKey,
                            (IMoveToNode)(new Node_MoveTo_With_Astar(unit.gameObject, unit.m_unit, unit.SeekarriveRadius))),
                        //1),
                    new Node_Attack_Activate_Weapon(unit.MainWeapon, unit.stats),
                    new Node_Delay(unit.stats.getAttackSpeed())
                }
            );
    }
  
}
