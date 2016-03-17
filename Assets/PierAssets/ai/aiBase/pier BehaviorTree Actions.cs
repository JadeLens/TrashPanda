using UnityEngine;
using System.Collections;
using extensions;
//leaf action nodes

/// <summary>
/// scales up a game object to 4 times its scale
/// </summary>
public class Node_PuffUp : aiBehaviorNode
{
    protected float m_targetScale = 4;
    Vector3 m_startSCale;
    bool isBaseScaleStored = false;
    /// <summary>
    /// scales up a game object to 4 times its scale
    /// </summary>
    public Node_PuffUp() { }
    public Node_PuffUp(float scale)
    {
        m_targetScale = scale;
    }
    public override void Reset()
    {
        isBaseScaleStored = false;
        MakeReady();
    }

    public override void Act(GameObject ob)
    {
        if (!isBaseScaleStored)
        {
            m_startSCale = ob.transform.localScale;
            isBaseScaleStored = true;
        }
        ob.transform.localScale = Vector3.Slerp(ob.transform.localScale, m_startSCale * m_targetScale, Time.deltaTime * 4);
        if (isObjectPuffed(ob))
        {
            Succeed();
        }
    }
    private bool isObjectPuffed(GameObject ob)
    {
        if ((m_startSCale * m_targetScale).sqrMagnitude - ob.transform.localScale.sqrMagnitude <= 0.3) { return true; } else { return false; }
    }
}

/// <summary>
/// scales down a game object to 1/4 its scale
/// </summary>
public class Node_PuffDown : aiBehaviorNode
{
    protected float m_targetScale = 0.25f;
    Vector3 m_startSCale;
    bool isBaseScaleStored = false;
    /// <summary>
    /// scales down a game object to 1/4 its scale
    /// </summary>
    public Node_PuffDown() { }
    public Node_PuffDown(float scale)
    {
        m_targetScale = 1 / scale;

    }
    public override void Reset()
    {
        isBaseScaleStored = false;
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        if (!isBaseScaleStored)
        {
            m_startSCale = ob.transform.localScale;
            isBaseScaleStored = true;
        }
        ob.transform.localScale = Vector3.Slerp(ob.transform.localScale, m_startSCale * m_targetScale, Time.deltaTime * 4);
        if (isObjectPuffed(ob))
        {
            Succeed();
        }
    }
    private bool isObjectPuffed(GameObject ob)
    {
        if (ob.transform.localScale.sqrMagnitude - (m_startSCale * m_targetScale).sqrMagnitude <= 0.3) { return true; } else { return false; }
    }
}

/// <summary>
/// DEPRECIATED use Modular Version
/// wander in range around a point
/// </summary>
public class Node_Wander : aiBehaviorNode
{
    private IMoveToNode m_child;
    float m_range;

    private bool isStartLocationStored = false;

    /// <summary>
    /// wander in range around a point
    /// </summary>
    public Node_Wander(float range, NavMeshAgent agent)
    {
        m_range = range;
    
        m_child = new Node_MoveTo_With_Avoid(agent);
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
        m_child.SetDestination(rDir.x,ob.transform.position.y, rDir.y);
        //1.5 is cube height to the ground
        m_child.SetArriveRadius(1+1.5f);
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
/// DEPRECIATED use Modular Version
/// finds a target of specific type 
/// then it moves toward it until it is close enought
/// fails if no units in detection range
/// </summary>
public class Node_Seek : aiBehaviorNode
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
    public Node_Seek(NavMeshAgent agent, float _range, float ArriveRadius, AItype _type = AItype.none)
    {
        m_range = _range;
        m_child1 = new Node_MoveTo_With_Avoid(agent);
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
                break;
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
                    //   Debug.Log("dest set");
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
/// finds the closest target and turns toward it 
/// fails if it cant find a target
/// for Unity NAvMesh
/// </summary>
public class Node_AlignToTarget :aiBehaviorNode
{
    private Node_FindClosestTarget m_child1;

    private Node_Align m_child2;
   
    /// <summary>
    /// finds the closest target and turns toward it 
    /// fails if it cant find a target
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="_range"></param>
    /// <param name="ArriveRadius"></param>
    /// <param name="_type"></param>
    public Node_AlignToTarget(NavMeshAgent agent, float _range, float ArriveRadius, AItype _type = AItype.none, float TurnSpeed = 3)
    {
        m_child1 = new Node_FindClosestTarget(_range, _type);
        m_child2 = new Node_Align(TurnSpeed);
        
    }
    public override void Reset()
    {
        m_child1.Reset();
        m_child2.Reset();
        //throw new System.NotImplementedException();
        MakeReady();
    }
    public override void Run()
    {
        base.Run();
        m_child1.Run();
    }
    public override void Act(GameObject ob)
    {
        switch (m_child1.GetState())
        {
            case NodeState.Running:
                //Debug.Log("searching");
                m_child1.Act(ob);
                break;
            case NodeState.Failure:
                Fail();
                //  Debug.Log(m_child1.GetDestination());
                return;
            //break;
            case NodeState.Success:
                Vector3 Direction = m_child1.GetTarget().transform.position - ob.transform.position;
                //add a check to see if we are close enought for detection
            //    m_child2.SetDirection( ob.transform.position - m_child1.GetTarget().transform.position);
                m_child2.SetDirection(m_child1.GetTarget().transform.position);
               // Debug.Log("dest set");
                // m_child2.Run();
               
                break;
        }
        if (m_child1.isSuccess())
        {
            switch (m_child2.GetState())
            {
                case NodeState.Ready:
                    m_child2.Run();
                    break;
                case NodeState.Running:
                    m_child2.Act(ob);
                    break;
                case NodeState.Failure:
                    Fail();
                    break;
                case NodeState.Success:
                    // Debug.Log("seek worked");
                    Succeed();
                   // Debug.Break();
                    break;

            }
        }
    }
}

/// <summary>
/// oposite of a seek behavior
/// </summary>
public class Node_Flee : aiBehaviorNode
{
    private IMoveToNode m_child1;
    private Node_FindClosestTarget m_child2;
    private float m_range;
   /// <summary>
   /// oposite of a seek
   /// </summary>
   /// <param name="agent"></param>
   /// <param name="_range"></param>
   /// <param name="ArriveRadius"></param>
   /// <param name="_type"></param>
    public Node_Flee(NavMeshAgent agent, float _range, float ArriveRadius, AItype _type = AItype.none)
    {
        m_range = _range;
        m_child1 = new Node_MoveTo_With_Avoid(agent);
        m_child2 = new Node_FindClosestTarget(m_range, _type);
        // m_child1.SetArriveRadius(0.1f);
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

    }
    public override void Act(GameObject ob)
    {
        //if (m_child1.isReady())//means we havent started to move
        // {
        switch (m_child2.GetState())
        {
            case NodeState.Ready:
                m_child2.Run();
                break;
            case NodeState.Running:
                //Debug.Log("looking for wolf");
                m_child2.Act(ob);
                break;
            case NodeState.Failure:
                //Debug.Log("did not find wolf");
                Fail();
                break;
            case NodeState.Success:
                // Debug.Log("wolf found");
                Vector3 Direction = ob.transform.position - m_child2.GetTarget().transform.position;

                //bug is setting direction as destination
                // mske a move to thats steers a certain dist along an orientation
                m_child1.SetDestination(ob.transform.position + Direction.normalized * 2);
                m_child1.Run();
                m_child2.Reset();
                break;
        }
        
        switch (m_child1.GetState())
        {
            case NodeState.Running:
                m_child1.Act(ob);
                break;
            case NodeState.Failure:
                Fail();
                break;
            case NodeState.Success:
                Succeed();
                break;

        }
    }
}

/// <summary>
/// used to make a basic rts like unit that will ]
/// follow orders from its order qeue 
/// needs a separate scrip to give the orders
/// fails if it does not have any orders
/// </summary>
public class Node_FollowOrders : aiBehaviorNode
{
    public aiBehavior unit;
    aiBehaviorNode currentOrder;
    /// <summary>
    /// used to make a basic rts like unit that will ]
    /// follow orders from its order qeue 
    /// needs a separate scrip to give the orders
    /// fails if it does not have any orders 
    /// </summary>
    /// <param name="u"></param>
    public Node_FollowOrders(aiBehavior u)
    {
        unit = u;

    }
    public override void Run()
    {
        base.Run();
        getOrder();
    }
    private void getOrder()
    {
       
           // currentOrder = unit.Orders.Dequeue();
        if (unit.Orders.Count != 0)
        {
            currentOrder = unit.Orders.Peek();
        }
        else
        {
            Fail();//fail if no new orders
        }
    }
    public override void Reset()
    {
        currentOrder = null;
        MakeReady();
    }

    public override void Act(GameObject ob)
    {
        getOrder();//get the latest oder
        if (currentOrder != null)
        {
            switch (currentOrder.GetState())
            {
                case NodeState.Ready:
                    currentOrder.Run();
                    break;
                case NodeState.Running:
                    currentOrder.Act(ob);
                    break;

                case NodeState.Failure:
                    Debug.Log("order failed");
                    //Fail();
                    break;
                case NodeState.Success:
                  //  Debug.Log("sucess");
                    //Succeed();
                    currentOrder = null;
                    unit.Orders.Dequeue();//remove completed order
                    break;

            }
        }
       
    }

}
/// <summary>
/// moveTo decorator node that sets a small arrive radius and
/// never succeds or fail
/// </summary>
public class Node_MoveInFormation : aiBehaviorNode
{
    private IMoveToNode m_child;
    private Transform m_target;
    public Node_MoveInFormation(Transform t,NavMeshAgent agent)
    {
        m_target = t;
        m_child = new Node_MoveTo_With_Avoid(agent);
    }
    public override void Reset()
    {
        MakeReady();
    }
    public override void Run()
    {
        base.Run();
        m_child.SetArriveRadius(0.1f);
    }
    public override void Act(GameObject ob)
    {
        m_child.SetDestination(m_target.position);
        switch (m_child.GetState())
        {
            case NodeState.Ready:
                m_child.Run();
                break;
            case NodeState.Running:
                m_child.Act(ob);
                break;
            case NodeState.Failure:
                
                break;
            case NodeState.Success:
               
                break;

        }
    }
}

/// <summary>
/// call the onActiavate of a weapon
/// </summary>
public class Node_Attack_Activate_Weapon : aiBehaviorNode
{
    public command m_weapon;
    private IGameUnit m_caster;
    public Node_Attack_Activate_Weapon(command weapon, IGameUnit caster)
    {
        m_weapon = weapon;
        m_caster = caster;
    }
    public override void Run()
    {
     //   Debug.Log("weapon activated");
        base.Run();
    }
    public override void Reset()
    {
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        m_weapon.onActivate(m_caster);
        Succeed();
    }
}
/// <summary>
/// activates all 3 ability commands
/// onActivate
/// onChannel
/// onRealease
/// ability needs to set its IsDone bool for 
/// proper end of chanel
/// </summary>
public class Node_Activate_Ability:aiBehaviorNode{
    public command m_ability;
    private IGameUnit m_caster;
    public Node_Activate_Ability(command ability, IGameUnit caster)
    {
        m_ability = ability;
        m_caster = caster;
    }
    public override void Run()
    {
        //Debug.Log("weapon activated");
        base.Run();
        m_ability.onActivate(m_caster);
    }
    public override void Reset()
    {
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        m_ability.onChanel(m_caster);
        if (m_ability.getIsDone())
        {
            Succeed();
        }
        
    }
    protected override void Succeed()
    {
        base.Succeed();
        m_ability.onRelease(m_caster);
    }
}
//utility nodes

/// <summary>
/// shoots a raycast to find a gameobject with a specific tag
/// </summary>
public class Node_RayCastForTag : aiBehaviorNode
{
    private string m_tag;
    private Vector3 m_rayHit;
    private GameObject m_hitObject;

    public GameObject GetHitObject() { return m_hitObject; }
    public Vector3 GetRayHitPosition() { return m_rayHit; }

    public Node_RayCastForTag(string tagToLookFor)
    {
        m_tag = tagToLookFor;
    }
    public override void Reset()
    {
        m_rayHit = Vector3.zero;
        m_hitObject = null;
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        bool hasFound = false;
        RaycastHit[] hits = Physics.RaycastAll(ob.transform.position, ob.transform.forward, 1f);
        for (int i = 0; i < hits.Length; i++)
        {
            //Debug.Log("Raycast to avoid");
            if (hits[i].collider.gameObject.tag == m_tag)
            {
                m_hitObject = hits[i].collider.gameObject;
                m_rayHit = hits[i].point;
                hasFound = true;
                break;
            }

        }
        if (hasFound)
        {
            Succeed();
        }
        else
        {
            Fail();
        }

    }
}

/// <summary>
/// does a circle cast to find targets
/// </summary>
public class Node_FindClosestTarget : aiBehaviorNode
{
    private float m_radius;
    private GameObject m_target;
    private AItype m_typeToLookFor;
    public Node_FindClosestTarget(float radius, AItype type = AItype.none)
    {
        m_radius = radius;
        m_typeToLookFor = type;
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

    }
}
