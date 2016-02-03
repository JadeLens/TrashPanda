using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Node_MoveTo_With_Astar : aiBehaviorNode, IMoveToNode
{
    private Vector3 m_target;
    private float m_radius;
    private GameObject m_owner;
    private Seeker m_seeker;
    private OnPathDelegate m_onPathMade;
    public Node_MoveTo_With_Astar(GameObject owner,Seeker seeker,OnPathDelegate onPath  )
    {
        m_owner = owner;
        m_seeker = seeker;
        m_onPathMade = onPath;

    }
    //temp-----------------------------
    public Node_MoveTo_With_Astar(GameObject owner, Seeker seeker, Vector3 loc)
    {
        m_owner = owner;
        m_seeker = seeker;
        SetDestination(loc);
    }
    //temp------------------------------

    public override void Run()
    {
        base.Run();
             Debug.Log("started seeker " +m_target);
             m_seeker.StartPath(m_owner.transform.position, m_target, m_onPathMade);
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
            Debug.Log("at destination");
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