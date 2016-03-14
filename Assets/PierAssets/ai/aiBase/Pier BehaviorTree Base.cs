using UnityEngine;
using System.Collections;
using extensions;

//base nodes
/// <summary>
/// base class for all nodes
/// need a reset, act func
/// </summary>
public abstract class aiBehaviorNode
{
    public enum NodeState
    {
        Ready,
        Success,
        Failure,
        Running
    }
    protected NodeState m_state;
    /// <summary>
    /// base class for all nodes
    /// need a reset, act func
    /// constructor mostly used for arrays 
    /// </summary>
    protected aiBehaviorNode() { }
    /// <summary>
    /// running state use to determine if nodes needs to be updated
    /// also double as a nodes onStart function
    /// </summary>
    public virtual void Run()
    {
        this.m_state = NodeState.Running;
        //Debug.Log(this.ToString() + "  started");
    }
    public abstract void Reset();
    /// <summary>
    /// is a nodes update function
    /// </summary>
    /// <param name="ob"></param>
    public abstract void Act(GameObject ob);
    /// <summary>
    /// usesed for resets  ready is also the default state
    /// </summary>
    protected virtual void MakeReady()
    {
        this.m_state = NodeState.Ready;
        //Debug.Log(this.ToString() + "  Ready");
    }
    /// <summary>
    /// used to report success to parent node
    /// can be use as an onExit
    /// </summary>
    protected virtual void Succeed()
    {
        this.m_state = NodeState.Success;
        //Debug.Log(this.ToString() + "  success");
    }
    /// <summary>
    /// used to report failure to parent node
    /// can be use as an onExit
    /// </summary>
    protected virtual void Fail()
    {
        this.m_state = NodeState.Failure;
        //Debug.Log(this.ToString() + "  failed");
    }
    //same as returning true if m_state == NodeState.Succes
    public bool isReady() { return m_state.Equals(NodeState.Ready); }
    public bool isSuccess() { return m_state.Equals(NodeState.Success); }
    public bool isFailure() { return m_state.Equals(NodeState.Failure); }
    public bool isRunning() { return m_state.Equals(NodeState.Running); }

    public NodeState GetState() { return m_state; }
}

/// <summary>
/// runs children in a sequence until one fails or all have been run
/// </summary>
public class Node_Sequence : aiBehaviorNode
{
    public aiBehaviorNode[] m_children;
    private int m_currentChildIndex = 0;
    /// <summary>
    /// runs child in a sequence until one fails or all have been run
    /// </summary>
    public Node_Sequence(aiBehaviorNode[] nodes)
    {
        m_children = nodes;
    }

    public override void Run()
    {
        base.Run();
        if (m_children != null && m_children[0] != null)
        {

            m_children[0].Run();
        }
        else
        {
            Fail();
        }
    }
    public override void Reset()
    {
        m_currentChildIndex = 0;
        for (int i = 0; i < m_children.Length; i++)
        {
            m_children[i].Reset();
        }
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
      //  Debug.Log("Node_Sequence act");
        //are we done running all childrens if so we succeeded
        if (m_currentChildIndex >= m_children.Length)
        {
            // Debug.Log(m_currentChildIndex + " all child process");
            Succeed();
            return;
        }
        switch (m_children[m_currentChildIndex].GetState())
        {
            //is the current child runing if so make it act
            case NodeState.Running:
                m_children[m_currentChildIndex].Act(ob);
                break;
            //if one child fails the whole sequence fails
            case NodeState.Failure:
                Fail();

                break;
            //if child succeeded increase the index to go to the next one
            case NodeState.Success:
                
				m_currentChildIndex++;
			// if we succeeded, don't wait for the next tick to process the next child
				 Act(ob);
			return;
            //if getState is null it means it hasent been started yet
            case NodeState.Ready:
                m_children[m_currentChildIndex].Run();
                break;
        }
    }
}

/// <summary>
/// repeats a child node n number of times or infinitively
/// fails if child fails once  
/// succeeds if repeated for n times
/// </summary>
public class Node_Repeat : aiBehaviorNode
{
    private aiBehaviorNode m_child;
    private int m_timesToRepeat;
    private int m_curentTime;/// <summary>
    /// repeats a child node n number of times or infinitively
    /// fails if child fails once  
    /// //if no time specified will repeat infinitively
    /// </summary>

    public Node_Repeat(aiBehaviorNode Node)
    {
        m_timesToRepeat = -1;
        m_child = Node;
        m_curentTime = 0;
    }
    /// <summary>
    /// repeats a child node n number of times or infinitively
    /// fails if child fails once  
    /// succeeds if repeated for n times
    /// </summary>
    public Node_Repeat(aiBehaviorNode Node, int times)
    {
        m_timesToRepeat = times;
        m_child = Node;
        m_curentTime = 0;
    }
    public override void Run()
    {
        base.Run();
        m_child.Run();
    }

    public override void Reset()
    {
        m_curentTime = 0;
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
                if (m_timesToRepeat == -1)
                {
                    m_child.Reset();
                }
                else
                {
                    Fail();
                }
                break;
            case NodeState.Success:
                m_curentTime++;
                //if under specified amout of times or infinite
                if (m_curentTime < m_timesToRepeat || m_timesToRepeat == -1)
                {
                    m_child.Reset();
                }
                else
                {
                    Succeed();
                }
                break;
            case NodeState.Ready:
                m_child.Run();
                break;
        }
    }
}


/// <summary>
/// runs child all child node in sequence until one succeeds
/// </summary>
public class Node_PrioritySelector : aiBehaviorNode
{
    public aiBehaviorNode[] m_children;
    private int m_currentChildIndex = 0;
    /// <summary>
    /// runs child all child node in sequence until one succeeds
    /// /// </summary>
    public Node_PrioritySelector() { }
    /// <summary>
    /// runs child all child node in sequence until one succeeds
    /// acts wierdly if only 1 child
    /// </summary>
    public Node_PrioritySelector(aiBehaviorNode[] nodes)
    {
        m_children = nodes;
    }

    public override void Run()
    {
        base.Run();
        if (m_children != null && m_children[0] != null)
        {

            m_children[0].Run();
        }
        else
        {
            Fail();
        }
    }
    public override void Reset()
    {
        m_currentChildIndex = 0;
        for (int i = 0; i < m_children.Length; i++)
        {
            m_children[i].Reset();
        }
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        //are we done running all childrens if so we failed
        if (m_currentChildIndex >= m_children.Length)
        {
            // Debug.Log(m_currentChildIndex + " all child process");
            Fail();
            return;
        }
        switch (m_children[m_currentChildIndex].GetState())
        {
            //is the current child runing if so make it act
            case NodeState.Running:
                m_children[m_currentChildIndex].Act(ob);
                break;
            //if one child fails go to the next one
            case NodeState.Failure:
                m_currentChildIndex++;
			// if we succeeded, don't wait for the next tick to process the next child
                
				Act( ob);
			return;
            //if one child succeeded the rest are skiped
            case NodeState.Success:
                Succeed();
                break;
            //if getState is null it means it hasent been started yet
            case NodeState.Ready:
                m_children[m_currentChildIndex].Run();
                break;
        }
    }

}



/// <summary>
/// DEPRECIATED
/// runs child all child node each update tick until one succeeds
/// </summary>
public class Node_Selector : aiBehaviorNode
{
    public aiBehaviorNode[] m_children;
    private int m_currentChildIndex = 0;
    /// <summary>
    /// runs child all child node in sequence until one succeeds
    /// /// </summary>
    public Node_Selector() { }
    /// <summary>
    /// runs child all child node in sequence until one succeeds
    /// acts wierdly if only 1 child
    /// </summary>
    public Node_Selector(aiBehaviorNode[] nodes)
    {
        m_children = nodes;
    }

    public override void Run()
    {
        base.Run();
       
    }
    public override void Reset()
    {
       // m_currentChildIndex = 0;
        for (int i = 0; i < m_children.Length; i++)
        {
            m_children[i].Reset();
        }
       // Debug.Log("reset");
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        //are we done running all childrens if so we failed
        /*if (m_currentChildIndex >= m_children.Length)
        {
            // Debug.Log(m_currentChildIndex + " all child process");
            Fail();
            return;
        }*/
        //Debug.Log("Node_Selector  act");
        for (int i = 0; i < m_children.Length;i++ )
        {
            bool isChildRunning = false;
            switch (m_children[i].GetState())
            {
                case NodeState.Ready:
                    m_children[i].Run();
                  //  Debug.Log("children " + i + " run");
                    break;
                //is the current child runing if so make it act
                case NodeState.Running:
                    m_children[i].Act(ob);
                    if(i>0){
                  //  Debug.Log("children "+ i+" act");
                    }
                    return;
				//return here else second child gets run before we know the result
                 //   break;
                //if one child fails go to the next one
                case NodeState.Failure:
                    m_children[i].Reset();
                    //reset it so it is ready for next turn
                     if(i>0){
                 //   Debug.Log("children " + i + " Failure");
                     }
                     break;
                //if one child succeeded the rest are skiped
                case NodeState.Success:
                    Succeed();
                    //Debug.Log("children " + i + " Success");
                    return;
                //if getState is null it means it hasent been started yet
              
            }
            //if we succeed stop the loop
            if (isSuccess() )
            {
                break;
            }
          
        }
        //we havent succeded after looping all children that means we failed
      /*  if (!isSuccess())
        {
            Fail();
        }*/
       
    }

}