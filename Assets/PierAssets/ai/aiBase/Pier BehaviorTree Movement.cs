using UnityEngine;
using System.Collections;
using extensions;

public interface IMoveToNode
{
    aiBehaviorNode.NodeState GetState();
    void Run();
    void Reset();
    void Act(GameObject ob);
    Vector3 GetDestination();
    void SetDestination(float x, float z);
    void SetDestination(float x, float y, float z);
    void SetDestination(Vector3 v);
    void SetArriveRadius(float f);
}

/// <summary>
/// base movement for all steering behaviours Node
/// </summary>
public class Node_SteerTo : aiBehaviorNode
{
    /// <summary>
    /// how close do we have to get to it
    /// </summary>

    private Vector3 m_target;
    private float m_moveSpeed = 5f;
    private float m_rotationSpeed = 3f;

    public Node_SteerTo() { }
    public Node_SteerTo(float x, float z)
    {
        SetDirection(x, z);
    }
    public Node_SteerTo(float x, float y, float z)
    {
        SetDirection(x, y, z);
    }
    public override void Run()
    {
        base.Run();
    }

    public void SetDirection(float x, float z)
    {
        m_target = new Vector3(x, 0, z);
    }
    public void SetDirection(float x, float y, float z)
    {
        m_target = new Vector3(x, y, z);
    }
    public void SetDirection(Vector3 to)
    {
        m_target = to;

    }
    /* 
      public void SetStartPosition(Vector3 from)
     {
         m_startPos = from;
     }*/
    public override void Reset()
    {
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        // Vector3 direction = m_target - ob.transform.position ;
        ob.transform.rotation = Quaternion.Slerp(ob.transform.rotation, Quaternion.LookRotation(m_target), m_rotationSpeed * Time.deltaTime);

        //Debug.Log(  direction.magnitude + " >" + m_arriveRadius);
        // Vector3 moveVector = m_target.normalized * m_moveSpeed * Time.deltaTime;
        Vector3 moveVector = ob.transform.forward.normalized * m_moveSpeed * Time.deltaTime;
        ob.transform.position += moveVector;



    }
}

public class Node_Align : aiBehaviorNode
{
    private Vector3 m_target;
    private NavMeshAgent m_agent;
    private float m_rotationSpeed = 3f;
    public Node_Align(float rSpeed=3f)
    {
        m_rotationSpeed = rSpeed;
    }
    public Node_Align(NavMeshAgent agent)
    {
        m_agent = agent;
        SetDirection(m_agent.destination);
    }
    public Node_Align(Vector3 v)
    {
        SetDirection(v);
    }
    public Node_Align(float x, float z)
    {
        SetDirection(x, z);
    }
    public Node_Align(float x, float y, float z)
    {
        SetDirection(x, y, z);
    }
   
    public override void Run()
    {
        base.Run();
    }

    public void SetDirection(float x, float z)
    {
        m_target = new Vector3(x, 0, z);
    }
    public void SetDirection(float x, float y, float z)
    {
        m_target = new Vector3(x, y, z);
    }
    public void SetDirection(Vector3 to)
    {
        m_target = to;

    }
    
    public override void Reset()
    {
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        // Vector3 direction = m_target - ob.transform.position ;
        if (m_target == Vector3.zero)
        {
            Debug.Break();
        }
        else
        {
           
            ob.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation((m_target - ob.transform.position).normalized), ob.transform.rotation, m_rotationSpeed * Time.fixedDeltaTime);
           // if (Vector3.Angle(ob.transform.forward, (m_target - ob.transform.position).normalized) < 2)
            if (Quaternion.Angle(Quaternion.LookRotation((m_target - ob.transform.position).normalized), ob.transform.rotation)<2)
            {
            
                Succeed();
                //  Debug.Log("finished Turning");
            }
            else
            {
              //  Debug.Log(Quaternion.Angle(Quaternion.LookRotation((m_target - ob.transform.position).normalized), ob.transform.rotation));
            
                // Debug.Log(Vector3.Angle(ob.transform.rotation.eulerAngles, Quaternion.LookRotation(m_target).eulerAngles));
            }
        }
    }
}

//temporary will use the command patern 
//so this node would determine wich direction to send to  the move command
//and also determine if we have reached the target pos
public class Node_MoveTo : aiBehaviorNode, IMoveToNode
{
    private bool isStartLocationStored = false;


    private Vector3 m_target;
    public float m_radius;
    private Node_SteerTo m_steering;

    public Node_MoveTo()
    {
        m_steering = new Node_SteerTo();
    }

    public Node_MoveTo(float x, float z)
    {
        m_steering = new Node_SteerTo();
        SetDestination(x, z);
    }

    public Node_MoveTo(float x, float y, float z)
    {
        m_steering = new Node_SteerTo();
        SetDestination(x, y, z);

    }
    public override void Run()
    {
        base.Run();
        m_steering.Run();
    }
    public Vector3 GetDestination()
    {
        return m_target;
    }
    public void SetDestination(float x, float z)
    {
        m_target = new Vector3(x, 0, z);
    }

    public void SetDestination(float x, float y, float z)
    {
        m_target = new Vector3(x, y, z);
    }

    public void SetDestination(Vector3 v)
    {
        m_target = v;
    }

    public void SetArriveRadius(float r)
    {
        m_radius = r;
    }

    public override void Reset()
    {
        m_target = Vector3.zero;
        //m_startPos = null;
        MakeReady();
    }

    public override void Act(GameObject ob)
    {
        if (isAtDestination(ob))
        {
            // Debug.Log("arrived");
            Succeed();
        }
        else
        {
            Vector3 Orientation = m_target - ob.transform.position;
            m_steering.SetDirection(Orientation);
            m_steering.Act(ob);
            Debug.DrawRay(ob.transform.position, m_target - ob.transform.position, Color.blue, 0.10f);
        }
    }

    private bool isAtDestination(GameObject ob)
    {
        //  Debug.Log(Vector3.Distance(m_target, ob.transform.position));
        if (Vector3.Distance(m_target, ob.transform.position) <= m_radius) { return true; } else { return false; }
        //if (m_target - m_startPos.sqrMagnitude <= 0.01) { return true; } else { return false; }
    }
}


public class Node_MoveTo_With_Avoid : aiBehaviorNode, IMoveToNode
{
    private Vector3 m_target;
    private float m_radius;
    private NavMeshAgent m_agent;

    public Node_MoveTo_With_Avoid(NavMeshAgent agent)
    {
        m_agent = agent;
    }
	//temp-----------------------------
	public Node_MoveTo_With_Avoid(NavMeshAgent agent,Vector3 loc)
	{
		m_agent = agent;
		SetDestination (loc);
	}
	//temp------------------------------

    public override void Run()
    {
        base.Run();
   //     Debug.Log(m_target);
       
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

    private Vector3 getNavCoordinate(Vector3 c)
    {
        Vector3 result;
        if (m_agent.RandomPoint(m_target, 0.2f, out result) == false)
        {
            //Fail();
        }

        return result;
    }

    public override void Reset()
    {
        //m_steering.Reset();

        MakeReady();
    }

    public override void Act(GameObject ob)
    {
        m_agent.SetDestination(m_target);
        
        Debug.DrawRay(ob.transform.position, m_target - ob.transform.position, Color.blue, 0.10f);
        
        switch (m_agent.pathStatus)
        {
            case NavMeshPathStatus.PathComplete:

              //  Succeed();
                break;

            case NavMeshPathStatus.PathInvalid:
            case NavMeshPathStatus.PathPartial:
                Fail();
                break;

        }
        if (isAtDestination(ob))
        {
            Succeed();
            m_agent.SetDestination(ob.transform.position);

        }
      
    }

    private bool isAtDestination(GameObject ob)
    {
        //  Debug.Log(Vector3.Distance(m_target, ob.transform.position));
        if (Vector3.Distance(ob.transform.position, new Vector3 (m_target.x,ob.transform.position.y,m_target.z)) <= m_radius) 
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


///now obsolete because of navMesh
/// <summary>
/// used to avoid obstacle
/// has a function to get an avoid vector
/// uses raycasting to find obstacles  
/// </summary>
public class Node_Avoid : aiBehaviorNode
{
    private Node_RayCastForTag m_RayCastNode;
    private Vector3 m_avoidVector;
    public Node_Avoid(string tagToAvoid)
    {
        m_RayCastNode = new Node_RayCastForTag(tagToAvoid);
    }
    public Vector3 GetAvoidVector() { return m_avoidVector; }

    public override void Run()
    {
        base.Run();
        m_RayCastNode.Run();

    }
    public override void Reset()
    {
        m_avoidVector = Vector3.zero;
        MakeReady();
        m_RayCastNode.Reset();
    }
    public override void Act(GameObject ob)
    {

        switch (m_RayCastNode.GetState())
        {
            case NodeState.Running:
                m_RayCastNode.Act(ob);
                break;
            case NodeState.Failure:

                Fail();
                break;
            case NodeState.Success:
                Vector3 temp = m_RayCastNode.GetRayHitPosition();
                //Debug.Log("need to avoid1");
                m_avoidVector = new Vector3(ob.transform.position.z, ob.transform.position.y, -ob.transform.position.x);
                Succeed();
                break;

        }
    }
}
