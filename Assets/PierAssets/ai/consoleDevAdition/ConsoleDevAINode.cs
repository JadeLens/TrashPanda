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
public class Node_MoveTo_With_Astar : aiBehaviorNode, IMoveToNode
{
    private Vector3 m_target;
    private float m_radius;
    private GameObject m_owner;
    private Seeker m_seeker;
    private OnPathDelegate m_onPathMade;
    Pier_Unit m_unit;
	public Node_MoveTo_With_Astar(GameObject owner, Seeker seeker, ref OnPathDelegate onPath, Pier_Unit unit,Vector3 loc = default(Vector3))
    {
        m_owner = owner;
        m_seeker = seeker;
        m_onPathMade = onPath;
        m_unit = unit;
		SetDestination(loc);
        ///Debug.Log("created move Node" + onPath);
    }
   
    public override void Run()
    {
        base.Run();
//             Debug.Log("started seeker " +m_target);
             Path p =  m_seeker.StartPath(m_owner.transform.position, m_target, m_onPathMade);
             m_unit.path = p;
    }
    public Vector3 GetDestination()
    {
        return m_target;
    }
    public void SetDestination(float x, float z)
    {
        m_target = (new Vector3(x, 0, z));
    }

    public void SetDestination(float x, float y, float z)
    {
        m_target = (new Vector3(x, y, z));
    }

    public void SetDestination(Vector3 v)
    {
        m_target = (v);
    }

    public void SetArriveRadius(float r)
    {
        m_radius = r;

    }

    public override void Reset()
    {
        //m_steering.Reset();

        MakeReady();
    }

    public override void Act(GameObject ob)
    {
        

        Debug.DrawRay(ob.transform.position, m_target - ob.transform.position, Color.blue, 0.10f);

      
        if (isAtDestination(ob))
        {
            Succeed();
           // Debug.Log("at destination");
           // m_agent.SetDestination(ob.transform.position);

        }

    }

    private bool isAtDestination(GameObject ob)
    {
        //  Debug.Log(Vector3.Distance(m_target, ob.transform.position));
        if (Vector3.Distance(ob.transform.position, new Vector3(m_target.x, ob.transform.position.y, m_target.z)) <= m_radius)
        {
            return true;
        }
        else
        {
            //  Debug.Log("moveTo distance is " + Vector3.Distance(ob.transform.position, m_target));
            //  Debug.Log("arrive radius is " + m_radius);
            return false;
        }
        //if (m_target - m_startPos.sqrMagnitude <= 0.01) { return true; } else { return false; }
    }
}