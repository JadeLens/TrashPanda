using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pathfinding;

public class Node_Call_Delegate:aiBehaviorNode{
	public delegate void NodeFunction();
	protected NodeFunction m_function;
	public Node_Call_Delegate(NodeFunction func){

		m_function = func;
	}
	public override void Run()
	{
		
		base.Run ();
		m_function();
	}
	public override void Reset()
	{

		MakeReady();
	}
	public override void Act(GameObject ob)
	{
		Succeed();

	}

}

/// <summary>
/// wander in range around a point
/// </summary>
public class Node_Wander_Modular : aiBehaviorNode
{
    private IMoveToNode m_child;
    float m_range;

    private bool isStartLocationStored = false;

    /// <summary>
    /// wander in range around a point
    /// </summary>
    public Node_Wander_Modular(IMoveToNode MoveNode, float range )
    {
        m_range = range;

        m_child = MoveNode;
    }

    public override void Reset()
    {
        isStartLocationStored = false;
        m_child.Reset();
        MakeReady();
    }

    public override void Run()
    {
        base.Run();
        //m_child.Run();
    }

    private void InitializeChild(GameObject ob)
    {
        Vector2 rDir = Random.insideUnitCircle;
        rDir *= m_range;
        rDir += new Vector2(ob.transform.position.x, ob.transform.position.z);
        m_child.SetDestination(rDir.x, ob.transform.position.y, rDir.y);
        //1.5 is cube height to the ground
        m_child.SetArriveRadius(1 + 1.5f);
        //  Debug.Log(rDir);
        // Debug.Log(rDir);
        //  m_child.SetDestination(Random.Range(-m_range + m_anchorPoint.x, m_range + m_anchorPoint.x), Random.Range(-m_range + m_anchorPoint.z, m_range + m_anchorPoint.z));
    }

    public override void Act(GameObject ob)
    {
        if (isStartLocationStored == false)
        {
            //m_anchorPoint = ob.transform.position;
            InitializeChild(ob);
            // Debug.Log(ob.transform.position);
            // m_child.SetStartPosition(ob.transform.position);
            isStartLocationStored = true;
        }
        switch (m_child.GetState())
        {
            case NodeState.Running:
                m_child.Act(ob);
                break;
            case NodeState.Failure:
                //  Debug.Log("wander fail");
                Fail();
                break;
            case NodeState.Success:
                //  Debug.Log("wander sucess");
                Succeed();
                break;
            case NodeState.Ready:
                m_child.Run();
                break;
        }

    }
}


/// <summary>
/// set a variable in the black board
/// </summary>
public class Node_SetVariable : aiBehaviorNode
{
    Dictionary<string, Object> m_dict;
    string m_keyToSet;
    Object m_value;
    //put constructor here
    public Node_SetVariable(Dictionary<string, Object> dict, string key,Object value)
    {
        m_dict = dict;
        m_keyToSet = key;
        m_value = value;


    }
    public override void Run()
    {

        base.Run();
        if(m_dict == null)
        {
            Debug.Log("npc ai blackBoard not initialized ");
            Debug.Break();

        }
    }
    public override void Reset()
    {

        MakeReady();
    }
    public override void Act(GameObject ob)
    {
          SetBBVar(m_dict, m_keyToSet, m_value);
          Succeed();
    }
    static public void SetBBVar(Dictionary<string, Object> dict,string key,Object value)
    {
        if (dict.ContainsKey(key) == false)
        {

            dict.Add(key, value);
        }
        else
        {
            dict[key] = value;
        }

    }

}

/// <summary>
/// checks if a blackBoard Variable is Null
/// </summary>
public class Node_IsNull : aiBehaviorNode
{
    Dictionary<string, Object> m_dict;
    string m_keyToCheck;
    
    /// <summary>
    /// checks if a blackBoard Variable is Null
    /// </summary>
    public Node_IsNull(Dictionary<string, Object> dict, string key)
    {
        m_dict = dict;
        m_keyToCheck = key;

    }
    public override void Run()
    {

        base.Run();
        if (m_dict == null)
        {
            Debug.Log("npc ai blackBoard not initialized ");
            Debug.Break();

        }
    }
    public override void Reset()
    {

        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        Object value = GetBBVar(m_dict, m_keyToCheck);

        if (value == null)
        {
            Succeed();
        }
        else
        {
            Fail();
        }
    }
    static public Object GetBBVar(Dictionary<string, Object> dict, string key)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];

        }
        else
        {
            return null;
        }

    }
}


/// <summary>
/// finds a target of specific type 
/// then it moves toward it until it is close enought
/// fails if no units in detection range
/// </summary>
public class Node_Seek_Modular : aiBehaviorNode
{
    private IMoveToNode m_child1;
    private Node_FindClosestTarget m_child2;
    private float m_range;

    /// <summary>
    /// finds a target of specific type 
    /// then it moves toward it until it is close enought
    /// fails if no units in detection range
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="_range"></param>
    /// <param name="ArriveRadius"></param>
    /// <param name="_type"></param>
    public Node_Seek_Modular(IMoveToNode MoveNode, float _range, float ArriveRadius, AItype _type = AItype.none)
    {
        m_range = _range;
        m_child1 = MoveNode;
        m_child2 = new Node_FindClosestTarget(m_range, _type);
        m_child1.SetArriveRadius(ArriveRadius);
    }
    public override void Reset()
    {
        m_child1.Reset();
        m_child2.Reset();
        MakeReady();
    }
    public override void Run()
    {
        base.Run();
        m_child2.Run();
    }
    public override void Act(GameObject ob)
    {
        // Debug.Log("seek node act");
        //we set a new destination every frame
        // if (m_child1.isReady())//means we havent started to move
        // {
        // add some code to lose the target
        switch (m_child2.GetState())
        {
            case NodeState.Running:
                //Debug.Log("searching");
                m_child2.Act(ob);
                return;
            case NodeState.Failure:
                Fail();
                //  Debug.Log(m_child1.GetDestination());
                return;
            //break;
            case NodeState.Success:
                if (m_child2.GetTarget() != null)
                {
                    Vector3 Direction = m_child2.GetTarget().transform.position - ob.transform.position;
                    //add a check to see if we are close enought for detection
                    m_child1.SetDestination(m_child2.GetTarget().transform.position);
//                       Debug.Log("dest set");
//				Debug.Log(m_child2.GetTarget().transform.position);
                    // m_child1.Run();
                }
                else
                {
                    Fail();
                }
                break;
        }
        switch (m_child1.GetState())
        {
            case NodeState.Ready:
                m_child1.Run();
                break;
            case NodeState.Running:
                m_child1.Act(ob);
                break;
            case NodeState.Failure:
                Fail();
                break;
            case NodeState.Success:
                // Debug.Log("seek worked");
                Succeed();
                break;

        }

    }
}


/// <summary>
/// finds a target of specific type 
/// then it moves toward it until it is close enought
/// </summary>
public class Node_Seek_Modular_BB : aiBehaviorNode
{
    private IMoveToNode m_child1;
 
    private string m_key;
    private Dictionary<string, Object> m_dict;
    public Node_Seek_Modular_BB(Dictionary<string, Object> blackBoard, string bb_key,IMoveToNode MoveNode)
    {
        m_dict = blackBoard;
        m_key = bb_key;
        m_child1 = MoveNode;
      //  m_child2 = new Node_FindClosestTarget(m_range, _type);
    }
    public override void Reset()
    {
        m_child1.Reset();
        MakeReady();
    }
    public override void Run()
    {
        base.Run();
  
    }
    public override void Act(GameObject ob)
    {
        // Debug.Log("seek node act");
        //we set a new destination every frame
        GameObject target = Node_IsNull.GetBBVar(m_dict, m_key) as GameObject;
        if (target  != null)
        {
            Vector3 Direction = target.transform.position - ob.transform.position;
            //add a check to see if we are close enought for detection
            m_child1.SetDestination(target.transform.position);
            //                       Debug.Log("dest set");
            //				Debug.Log(m_child2.GetTarget().transform.position);
            // m_child1.Run();
        }
        else
        {
            Debug.LogError("typecast failed wrong data type in bb var  if var can be null use the Is_Null Node");
            Fail();
        }
                
        
        switch (m_child1.GetState())
        {
            case NodeState.Ready:
                m_child1.Run();
                break;
            case NodeState.Running:
                m_child1.Act(ob);
                break;
            case NodeState.Failure:
                Fail();
                break;
            case NodeState.Success:
                // Debug.Log("seek worked");
                Succeed();
                break;

        }

    }
}

/// <summary>
/// does a circle cast to find targets
/// uses a the blackBoard
/// </summary>
public class Node_Find_Closest_Target_BB : aiBehaviorNode
{
    private float m_radius;

    private GameObject m_target;
    private AItype m_typeToLookFor;
    private string m_key;
    private Dictionary<string, Object> m_dict;
    public Node_Find_Closest_Target_BB(Dictionary<string, Object> blackBoard,string bb_key,float radius, AItype type = AItype.none)
    {
        m_radius = radius;
        m_typeToLookFor = type;
        m_dict = blackBoard;
        m_key = bb_key;
    }
    public override void Reset()
    {
        m_target = null;
        MakeReady();
    }
    public GameObject GetTarget()
    {
        if (m_target == null)
        {
            Fail();
        }

        return m_target;
    }

    public override void Act(GameObject ob)
    {
        float closestDist = Mathf.Infinity;
        Collider[] hitColliders = Physics.OverlapSphere(ob.transform.position, m_radius);
        int i = 0;
        aiBehavior temp;

        while (i < hitColliders.Length)
        {

            temp = hitColliders[i].gameObject.GetComponent<aiBehavior>();
            if (temp != null && (temp.type == m_typeToLookFor || m_typeToLookFor == AItype.none))
            {

                float curDist = Vector3.Distance(temp.gameObject.transform.position, ob.transform.position);
                if (curDist < closestDist)
                {
                    closestDist = curDist;
                    m_target = temp.gameObject;
                }
            }

            i++;
        }
        if (m_target == null)
        {
            
            Fail();
        }
        else
        {
            Succeed();
        }
        Node_SetVariable.SetBBVar(m_dict, m_key, m_target);
    }
}

/// <summary>
/// repeats a child node until it fails
/// </summary>
public class Node_Repeat_Until_Fail : aiBehaviorNode
{
    private aiBehaviorNode m_child;
    /// <summary>
    /// repeats a child node until it fails
    /// </summary>
    public Node_Repeat_Until_Fail(aiBehaviorNode Node)
    {
        m_child = Node;
    }
    
    public override void Run()
    {
        base.Run();
        m_child.Run();
    }

    public override void Reset()
    {
        m_child.Reset();
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        switch (m_child.GetState())
        {
            case NodeState.Running:
                m_child.Act(ob);
                break;
            case NodeState.Failure:

                Succeed();
               
                break;
            case NodeState.Success:
                m_child.Reset();
             
         
                break;
            case NodeState.Ready:
                m_child.Run();
                break;
        }
    }
}

