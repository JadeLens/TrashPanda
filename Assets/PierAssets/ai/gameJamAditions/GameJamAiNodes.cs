using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node_template: aiBehaviorNode{
	//put constructor here

	public override void Run()
	{

		base.Run ();

	}
	public override void Reset()
	{

		MakeReady();
	}
	public override void Act(GameObject ob)
	{


	}

}
public class Node_Follow_Path :  aiBehaviorNode
{
	int currentNodeNumber = 0;
    public aiBehavior unit;
    private Transform[] m_wayPoints;
    private IMoveToNode m_child1;

    public Node_Follow_Path(NavMeshAgent agent, Transform[] wayPoints)
    {
        m_wayPoints = wayPoints;
        m_child1  = new Node_MoveTo_With_Avoid(agent); ;
    }
	public override void Run()
	{
		m_child1.SetDestination(m_wayPoints[currentNodeNumber].position);
		base.Run ();

	}
    public override void Reset()
    {
        m_child1.Reset();
        MakeReady();
    }
    public override void Act(GameObject ob)
    {
        switch (m_child1.GetState())
        {
            case NodeState.Ready:
			
                m_child1.Run();
                break;
            case NodeState.Running:
                //Debug.Log("looking for wolf");
                m_child1.Act(ob);
                break;
            case NodeState.Failure:
                //Debug.Log("did not find wolf");
                Fail();
                break;
		case NodeState.Success:
        
                // mske a move to thats steers a certain dist along an orientation
			currentNodeNumber += 1;

			if (currentNodeNumber > m_wayPoints.Length) {
				currentNodeNumber = 0;
			}
			m_child1.SetDestination(m_wayPoints[currentNodeNumber].position);
			//m_child1.Run ();
			m_child1.Reset ();
			//Succeed();
                break;
        }
        
    }
}
public class Node_CheckBool: aiBehaviorNode{
	//put constructor
	Dictionary<string, bool> m_dict;
	string m_keyToCheck;
	public Node_CheckBool(Dictionary<string, bool> dict,string key){
	
		m_dict = dict;
		m_keyToCheck = key;
	}


	public override void Run()
	{
		
		base.Run ();

	}
	public override void Reset()
	{
		
		MakeReady();
	}
	public override void Act(GameObject ob)
	{
		if (m_dict.ContainsKey (m_keyToCheck) == false) {
		
			Debug.Log ("npc ai blackBoard mising key :" + m_keyToCheck);
			Debug.Break();
		}
		if ((m_dict [m_keyToCheck])) {
			Succeed ();
		} else {
			Fail ();
		}

	}

}
/// <summary>
/// Node invert 
/// inverts the child node output.
/// </summary>
public class Node_Invert : aiBehaviorNode
{
	private aiBehaviorNode m_child;

	/// <summary>
	/// Node invert 
	/// inverts the child node output.
	/// </summary>
	public Node_Invert(aiBehaviorNode Node)
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

			Fail();
			break;
		case NodeState.Ready:
			m_child.Run(); 
			break;
		}
	
	}
}

/// <summary>
/// runs child all child node each update tick until one fails
/// </summary>
public class Node_Concurent : aiBehaviorNode
{
	public aiBehaviorNode[] m_children;
	private int m_currentChildIndex = 0;

	/// <summary>
	/// runs child all child node each update tick until one fails
	/// </summary>
	public Node_Concurent(aiBehaviorNode[] nodes)
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
				//Debug.Log("children " + i + " updated");
					return;
				//return here else second child gets run before we know the result
					//if one child fails go to the next one
			case NodeState.Failure:
						
						//reset it so it is ready for next turn
				Fail ();
			//	Debug.Log("children " + i + " failed");
				return;
					//if one child succeeded the rest are skiped
				case NodeState.Success:

					m_children[i].Reset();
						if (i == m_children.Length )
						{ 
							Succeed();
							return;
					//return here else second child gets run before we know the result
						}
					
				//	Debug.Log("children " + i + " Success");
					break;
					//if getState is null it means it hasent been started yet

			}


			//if we succeed at end of loop


		}
		//we havent succeded after looping all children that means we failed
		/*  if (!isSuccess())
        {
            Fail();
        }*/

	}  

}