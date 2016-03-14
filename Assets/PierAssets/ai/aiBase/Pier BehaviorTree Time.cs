using UnityEngine;
using System.Collections;
using extensions;

/// <summary>
///used to limit the time a node can be run
/// fails if time runs out or if child node fails
/// succeeds if child succeeds
/// </summary>
public class Node_Timer : aiBehaviorNode
{
    private aiBehaviorNode m_child;

    private float m_timeToWait;
    private float m_timer;


    public Node_Timer(aiBehaviorNode Node, int timeToWait)
    {
        m_timeToWait = timeToWait;
        m_child = Node;
    }
    public override void Run()
    {
        base.Run();
        m_child.Run();
    }

    public override void Reset()
    {
        m_timer = 0;
        m_child.Reset();
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        m_timer += Time.deltaTime;


        switch (m_child.GetState())
        {
            case NodeState.Running:
                m_child.Act(ob);
                break;
            case NodeState.Failure:
                Fail();
                break;
            case NodeState.Success:
                Succeed();
                break;
            case NodeState.Ready:
                m_child.Run();
                break;
        }
        if (m_timer >= m_timeToWait)
        {
            Fail();
        }
    }
}


/// <summary>
/// waits for time in secs then succeeds
/// use when the branch you want to delay has to stop the traversal of the tree
/// ex: delay between basic attack
/// </summary>
public class Node_Delay : aiBehaviorNode
{
    private float m_delay;
    private float m_timer;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeToWait"></param>
    public Node_Delay(float timeToWait)
    {
        m_delay = timeToWait;
    }
    public override void Reset()
    {
        m_timer = 0;
        MakeReady();
    }
    public override void Run()
    {
        base.Run();
    }
    public override void Act(GameObject ob)
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_delay)
        {
            Succeed();
        }
    }
}


/// <summary>
/// will return true if time in secs has passed
/// use when the branch you want to delay must not stop traversal of the tree
/// ex: delay between special ability
/// </summary>
public class Node_CoolDownTimer : aiBehaviorNode
{
    private bool firstTime = true;
    private float m_delay;
    /// <summary>
    /// last time it succeded
    /// </summary>
    private float m_LastTime;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeToWait"></param>
    public Node_CoolDownTimer(float timeToWait)
    {
        m_delay = timeToWait;
    }
    public override void Reset()
    {

        MakeReady();
    }
    public override void Run()
    {
        base.Run();
    }
    public override void Act(GameObject ob)
    {
        if (firstTime)
        {
            firstTime = false;
            Succeed();
        }

        else if (Time.time - m_LastTime > m_delay)
        {
            Succeed();
        }
        else
        {
            Fail();
        }
    }
    protected override void Succeed()
    {
        base.Succeed();

        m_LastTime = Time.time;
    }
}
